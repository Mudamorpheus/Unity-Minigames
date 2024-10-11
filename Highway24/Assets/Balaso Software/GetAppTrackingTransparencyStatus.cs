using System.Collections;
using Balaso;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Example MonoBehaviour class requesting iOS Tracking Authorization
/// </summary>
public class GetAppTrackingTransparencyStatus : MonoBehaviour {
    private void Awake() {
    #if UNITY_IOS
        AppTrackingTransparency.RegisterAppForAdNetworkAttribution();
        AppTrackingTransparency.UpdateConversionValue(3);
    #endif
    }

    void Start() {
    #if UNITY_IOS
        #if USE_GEO
            StartCoroutine(GpsPermissionRequest());
        #else
            GetIDFA();
        #endif
        #endif
    }
    
    private void GetIDFA() {
        #if UNITY_IOS && !UNITY_EDITOR
            AppTrackingTransparency.OnAuthorizationRequestDone += OnAuthorizationRequestDone;
            AppTrackingTransparency.AuthorizationStatus currentStatus = AppTrackingTransparency.TrackingAuthorizationStatus;
            Debug.Log(string.Format("Current authorization status: {0}", currentStatus.ToString()));
            if (currentStatus != AppTrackingTransparency.AuthorizationStatus.AUTHORIZED) {
                Debug.Log("Requesting authorization...");
                AppTrackingTransparency.RequestTrackingAuthorization();
            }
        #endif 
    } 

#if UNITY_IOS && USE_GEO 
    private IEnumerator GpsPermissionRequest() {
        
            Input.location.Start(500f, 500f);
            var maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
                yield return new WaitForSeconds(0.5f);
                maxWait--;
            }

            if (maxWait < 1) {
                Input.location.Stop();
                GetIDFA();
                yield break;
            }

            switch (Input.location.status) {
                case LocationServiceStatus.Running:
                    Common.NativeScripts.iOS.GeoWrapper.GetCurrentGeo(
                        latitude: Input.location.lastData.latitude,
                        longitude: Input.location.lastData.longitude,
                        action: GetGeoAndContinue,
                        gameObjectName: gameObject.name
                    );
                    yield break;
                case LocationServiceStatus.Failed:
                    Input.location.Stop();
                    GetIDFA();
                    yield break;
                default:
                    Input.location.Stop();
                    GetIDFA();
                    yield break;
            }
    }
#endif

#if USE_GEO
    private void GetGeoAndContinue(string localGeo) {
        Input.location.Stop();
        GetIDFA();
    }
#endif

#if UNITY_IOS

    /// <summary>
    /// Callback invoked with the user's decision
    /// </summary>
    /// <param name="status"></param>
    private void OnAuthorizationRequestDone(AppTrackingTransparency.AuthorizationStatus status) {
        switch (status) {
            case AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED:
                Debug.Log("AuthorizationStatus: NOT_DETERMINED");
                break;
            case AppTrackingTransparency.AuthorizationStatus.RESTRICTED:
                Debug.Log("AuthorizationStatus: RESTRICTED");
                break;
            case AppTrackingTransparency.AuthorizationStatus.DENIED:
                Debug.Log("AuthorizationStatus: DENIED");
                break;
            case AppTrackingTransparency.AuthorizationStatus.AUTHORIZED:
                Debug.Log("AuthorizationStatus: AUTHORIZED");
                break;
        }

        // Obtain IDFA
        Debug.Log(string.Format("IDFA: {0}", AppTrackingTransparency.IdentifierForAdvertising()));
    }
#endif
}
