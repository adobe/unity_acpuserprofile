/*
Unity Plug-in v: 0.0.1
Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System;

namespace com.adobe.marketing.mobile
{
	public delegate void AdobeGetUserAttributesCallback(string value);
	

	#if UNITY_ANDROID
	class GetUserAttributesCallback: AndroidJavaProxy
	{
		AdobeGetUserAttributesCallback redirectedDelegate;
		public GetUserAttributesCallback (AdobeGetUserAttributesCallback callback): base("com.adobe.marketing.mobile.AdobeCallback") {
			redirectedDelegate = callback;
		}

		void call(AndroidJavaObject retrievedAttributes)
		{
            if (retrievedAttributes == null) {
				redirectedDelegate ("");
				return;
			}

            Dictionary<string, object> userAttrs = ACPUserProfileHelpers.GetDictionaryFromHashMap(retrievedAttributes);
            string result = ACPUserProfileHelpers.JsonStringFromDictionary(userAttrs);
			redirectedDelegate (result);
		}
	}
	#endif

	public class ACPUserProfile
	{
        #if UNITY_IPHONE
	    /* ===================================================================
		 * extern declarations for iOS Methods
		 * =================================================================== */
        [DllImport ("__Internal")]
		private static extern System.IntPtr acp_UserProfile_ExtensionVersion();

        [DllImport ("__Internal")]
		private static extern void acp_UserProfile_RegisterExtension();

        [DllImport ("__Internal")]
		private static extern void acp_GetUserAttributes(string attributeKeys, AdobeGetUserAttributesCallback callback);

        [DllImport ("__Internal")]
		private static extern void acp_RemoveUserAttribute(string attributeName);

        [DllImport ("__Internal")]
		private static extern void acp_RemoveUserAttributes(string attributeNames);

        [DllImport ("__Internal")]
		private static extern void acp_UpdateUserAttribute(string attributeName, string attributeValue);

        [DllImport ("__Internal")]
		private static extern void acp_UpdateUserAttributes(string attributeMap);

        #endif

        #if UNITY_ANDROID && !UNITY_EDITOR
        /* ===================================================================
        * Static Helper objects for our JNI access
        * =================================================================== */
        static AndroidJavaClass userProfile = new AndroidJavaClass("com.adobe.marketing.mobile.UserProfile");
        #endif

	    /*---------------------------------------------------------------------
	    * User Profile Methods
	    *----------------------------------------------------------------------*/
        public static string ExtensionVersion()
	    {
            #if UNITY_IPHONE && !UNITY_EDITOR
            return Marshal.PtrToStringAnsi(acp_UserProfile_ExtensionVersion());		
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    return userProfile.CallStatic<string> ("extensionVersion");
            #else
		    return "";
            #endif
	    }

        public static void RegisterExtension()
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_UserProfile_RegisterExtension();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            userProfile.CallStatic("registerExtension");
            #endif
        }

        public static void GetUserAttributes(List<string> attributeKeys, AdobeGetUserAttributesCallback callback)
        {
            if (callback == null) {
				Debug.Log("Failed to perform GetUserAttributes, callback is null");
				return;
			}

            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_GetUserAttributes(ACPUserProfileHelpers.JsonStringFromStringList(attributeKeys), callback);
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    userProfile.CallStatic("getUserAttributes", ACPUserProfileHelpers.GetListFromList(attributeKeys), new GetUserAttributesCallback(callback));
            #endif
        }

        public static void RemoveUserAttribute(string attributeName)
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_RemoveUserAttribute(attributeName);
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    userProfile.CallStatic("removeUserAttribute", attributeName);
            #endif
        }

        public static void RemoveUserAttributes(List<string> attributeNames)
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_RemoveUserAttributes(ACPUserProfileHelpers.JsonStringFromStringList(attributeNames));
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    userProfile.CallStatic("removeUserAttributes", ACPUserProfileHelpers.GetListFromList(attributeNames));
            #endif
        }

        public static void UpdateUserAttribute(string attributeName, string attributeValue)
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_UpdateUserAttribute(attributeName, attributeValue);
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    userProfile.CallStatic("updateUserAttribute", attributeName, attributeValue);
            #endif
        }

        public static void UpdateUserAttributes(Dictionary<string, object> attributeMap)
        {
            #if UNITY_IPHONE && !UNITY_EDITOR
            acp_UpdateUserAttributes(ACPUserProfileHelpers.JsonStringFromDictionary(attributeMap));
            #elif UNITY_ANDROID && !UNITY_EDITOR
		    userProfile.CallStatic("updateUserAttributes", ACPUserProfileHelpers.GetHashMapFromDictionary(attributeMap));
            #endif
        }
	}
}