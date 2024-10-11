using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



namespace Prime31
{
	public class GoogleIABEventListener : MonoBehaviour
	{
#if UNITY_ANDROID
		void OnEnable()
		{
			// Listen to all events for illustration purposes
			GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
			GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
			GoogleIABManager.acknowledgePurchaseSucceededEvent += acknowledgePurchaseSucceededEvent;
			GoogleIABManager.acknowledgePurchaseFailedEvent += acknowledgePurchaseFailedEvent;
		}


		void OnDisable()
		{
			// Remove all event handlers
			GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
			GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
			GoogleIABManager.acknowledgePurchaseSucceededEvent -= acknowledgePurchaseSucceededEvent;
			GoogleIABManager.acknowledgePurchaseFailedEvent -= acknowledgePurchaseFailedEvent;
		}



		void billingSupportedEvent()
		{
			Debug.Log( "billingSupportedEvent" );
		}


		void billingNotSupportedEvent( string error )
		{
			Debug.Log( "billingNotSupportedEvent: " + error );
		}


		void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
		{
			Debug.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
			Prime31.Utils.logObject( purchases );
			Prime31.Utils.logObject( skus );
		}


		void queryInventoryFailedEvent( string error )
		{
			Debug.Log( "queryInventoryFailedEvent: " + error );
		}


		void purchaseSucceededEvent( GooglePurchase purchase )
		{
			Debug.Log( "purchaseSucceededEvent: " + purchase );
		}


		void purchaseFailedEvent( string error, int response )
		{
			Debug.Log( "purchaseFailedEvent: " + error + ", response: " + response );
		}


		void consumePurchaseSucceededEvent( GooglePurchase purchase )
		{
			Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		}


		void consumePurchaseFailedEvent( string error )
		{
			Debug.Log( "consumePurchaseFailedEvent: " + error );
		}


		void acknowledgePurchaseFailedEvent(string error)
		{
			Debug.Log( "acknowledgePurchaseFailedEvent: " + error );
		}

		void acknowledgePurchaseSucceededEvent(GooglePurchase purchase)
		{
			Debug.Log( "acknowledgePurchaseSucceededEvent: " + purchase );
		}
#endif
	}

}


