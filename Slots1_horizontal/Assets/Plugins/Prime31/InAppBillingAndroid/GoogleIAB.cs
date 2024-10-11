using UnityEngine;
using System.Collections;
using System.Collections.Generic;



#if UNITY_ANDROID

namespace Prime31
{
	public class GoogleIAB
	{
		private static AndroidJavaObject _plugin;

		static GoogleIAB()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			// find the plugin instance
			using( var pluginClass = new AndroidJavaClass( "com.prime31.GoogleIABPlugin" ) )
				_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
		}


		/// <summary>
		/// Toggles high detail logging on/off
		/// </summary>
		public static void enableLogging( bool shouldEnable )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			if( shouldEnable )
				Debug.LogWarning( "YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!" );
			_plugin.Call( "enableLogging", shouldEnable );
		}


		/// <summary>
		/// Toggles automatic signature verification on/off
		/// </summary>
		public static void setAutoVerifySignatures( bool shouldVerify )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "setAutoVerifySignatures", shouldVerify );
		}


		/// <summary>
		/// Initializes the billing system. allowTestProducts will default to the value of Debug.isDebugBuild.
		/// </summary>
		public static void init( string publicKey )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "init", publicKey );
		}


		/// <summary>
		/// Unbinds and shuts down the billing service
		/// </summary>
		public static void unbindService()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "unbindService" );
		}


		/// <summary>
		/// Returns whether subscriptions are supported on the current device
		/// </summary>
		public static bool areSubscriptionsSupported()
		{
			if( Application.platform != RuntimePlatform.Android )
				return false;

			return _plugin.Call<bool>( "areSubscriptionsSupported" );
		}


		/// <summary>
		/// Sends a request to get all completed purchases and product information as setup in the Play dashboard about the provided skus
		/// </summary>
		public static void queryInventory( string[] skus )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "queryInventory", new object[] { skus } );

			//var method = AndroidJNI.GetMethodID( _plugin.GetRawClass(), "queryInventory", "([Ljava/lang/String;)V" );
			//AndroidJNI.CallVoidMethod( _plugin.GetRawObject(), method, AndroidJNIHelper.CreateJNIArgArray( new object[] { skus } ) );
		}


		public static List<GooglePurchase> getPurchaseHistory()
		{
			if( Application.platform != RuntimePlatform.Android )
				return null;

			var response = _plugin.Call<string>( "getPurchaseHistory" );
			Debug.Log(response);
			return Json.decode<List<GooglePurchase>>( response );
		}


		/// <summary>
		/// Sends our a request to purchase the product. If oldSku is passed in this will trigger a subscription upgrade/downgrade. See Google's
		/// docs for more information.
		/// </summary>
		public static void purchaseProduct( string sku )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

            _plugin.Call( "purchaseProduct", sku );
		}


		/// <summary>
		/// Sends out a request to consume the product
		/// </summary>
		public static void consumeProduct( string sku )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "consumeProduct", sku );
		}


		/// <summary>
		/// Sends out a request to consume the product
		/// </summary>
		public static void acknowledgePurchase( string sku )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "acknowledgePurchase", sku );
		}
	}

}
#endif
