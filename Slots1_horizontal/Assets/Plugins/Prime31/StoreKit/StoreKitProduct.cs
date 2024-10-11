using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_IOS || UNITY_TVOS

namespace Prime31
{
	public class StoreKitProduct
	{
		public class IntroductoryPrice
		{
			public string subscriptionPeriodUnit;
			public int numberOfPeriods;
			public int subscriptionPeriodNumberOfUnits;
			public string price;
			public string paymentMode;

			public IntroductoryPrice( Dictionary<string, object> dict )
			{
				if( dict.ContainsKey( "subscriptionPeriodUnit" ) )
					subscriptionPeriodUnit = dict["subscriptionPeriodUnit"].ToString();
				
				if( dict.ContainsKey( "numberOfPeriods" ) )
					numberOfPeriods = int.Parse( dict["numberOfPeriods"].ToString() );
				
				if( dict.ContainsKey( "subscriptionPeriodNumberOfUnits" ) )
					subscriptionPeriodNumberOfUnits = int.Parse( dict["subscriptionPeriodNumberOfUnits"].ToString() );
				
				if( dict.ContainsKey( "price" ) )
					price = dict["price"].ToString();
				
				if( dict.ContainsKey( "paymentMode" ) )
					paymentMode = dict["paymentMode"].ToString();
			}
		}

	    public string productIdentifier;
	    public string title;
	    public string description;
	    public string price;
		public string currencySymbol;
		public string currencyCode;
		public string formattedPrice;
	
		public string countryCode;
		public string downloadContentVersion;
		public bool downloadable;
		public List<Int64> downloadContentLengths = new List<Int64>();
		public IntroductoryPrice introductoryPrice;
	
	
		public static List<StoreKitProduct> productsFromJson( string json )
		{
			var productList = new List<StoreKitProduct>();
	
			var products = json.listFromJson();
			foreach( Dictionary<string,object> ht in products )
				productList.Add( productFromDictionary( ht ) );
	
			return productList;
		}
	
	
	    public static StoreKitProduct productFromDictionary( Dictionary<string,object> dict )
	    {
	        StoreKitProduct product = new StoreKitProduct();
	
			if( dict.ContainsKey( "productIdentifier" ) )
	        	product.productIdentifier = dict["productIdentifier"].ToString();
	
			if( dict.ContainsKey( "localizedTitle" ) )
	        	product.title = dict["localizedTitle"].ToString();
	
			if( dict.ContainsKey( "localizedDescription" ) )
	        	product.description = dict["localizedDescription"].ToString();
	
			if( dict.ContainsKey( "price" ) )
	        	product.price = dict["price"].ToString();
	
			if( dict.ContainsKey( "currencySymbol" ) )
				product.currencySymbol = dict["currencySymbol"].ToString();
	
			if( dict.ContainsKey( "currencyCode" ) )
				product.currencyCode = dict["currencyCode"].ToString();
	
			if( dict.ContainsKey( "formattedPrice" ) )
				product.formattedPrice = dict["formattedPrice"].ToString();
	
			if( dict.ContainsKey( "countryCode" ) )
				product.countryCode = dict["countryCode"].ToString();
	
			if( dict.ContainsKey( "downloadContentVersion" ) )
				product.downloadContentVersion = dict["downloadContentVersion"].ToString();
	
			if( dict.ContainsKey( "downloadable" ) )
				product.downloadable = bool.Parse( dict["downloadable"].ToString() );
	
			if( dict.ContainsKey( "downloadContentLengths" ) && dict["downloadContentLengths"] is IList )
			{
				var tempLengths = dict["downloadContentLengths"] as List<object>;
				foreach( var dlLength in tempLengths )
					product.downloadContentLengths.Add( System.Convert.ToInt64( dlLength ) );
			}
			
			if( dict.ContainsKey( "introductoryPrice" ) )
			{
				product.introductoryPrice = new IntroductoryPrice( dict["introductoryPrice"] as Dictionary<string,object> );
			}
	
	        return product;
	    }
	
	
		public override string ToString()
		{
			return String.Format( "<StoreKitProduct>\nID: {0}\ntitle: {1}\ndescription: {2}\nprice: {3}\ncurrencysymbol: {4}\nformattedPrice: {5}\ncurrencyCode: {6}\ncountryCode: {7}\ndownloadContentVersion: {8}\ndownloadable: {9}",
				productIdentifier, title, description, price, currencySymbol, formattedPrice, currencyCode, countryCode, downloadContentVersion, downloadable );
		}
	
	}

}
#endif
