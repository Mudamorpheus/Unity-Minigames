using UnityEngine;

public class GetAtt : MonoBehaviour {

    private void Start() {
    #if UNITY_IOS && !UNITY_EDITOR
            AppTrackingTransparency.OnAuthorizationRequestDone += OnAuthorizationRequestDone;
            var currentStatus = AppTrackingTransparency.TrackingAuthorizationStatus;
            if (currentStatus != AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED) return;
            AppTrackingTransparency.RequestTrackingAuthorization();
        #endif 
    }


#if UNITY_IOS
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
    }
#endif
}
