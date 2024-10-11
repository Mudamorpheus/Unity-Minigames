using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



#if UNITY_ANDROID

namespace Prime31
{
	public class GoogleBuyIntentExtraParams
	{
		public List<String> skusToReplace = new List<string>();
		public bool replaceSkusProration;
		public string accountId;
	}
}
#endif
