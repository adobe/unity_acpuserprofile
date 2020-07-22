/*
ACPUserProfileHelpers.cs

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace com.adobe.marketing.mobile {
    public class ACPUserProfileHelpers {
        protected internal static string JsonStringFromStringList(List<string> list) 
        {
            if (list == null || list.Count <= 0) 
            {
                return null;
            }
            
            string jsonString = string.Join(",", list.ToArray());
            return jsonString;
        }

        protected internal static string JsonStringFromDictionary(Dictionary<string, object> dict) 
        {
            if (dict == null || dict.Count <= 0) 
            {
                return null;
            }
            
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            string jsonString = "{" + string.Join (",", entries.ToArray()) + "}";
            
            return jsonString;
        }

        #if UNITY_ANDROID
        protected internal static AndroidJavaObject GetListFromList(List<string> list)
        {
            // quick out if nothing in the dict param
            if (list == null || list.Count <= 0) 
            {
                return null;
            }
            
            AndroidJavaObject listObj = new AndroidJavaObject ("java.util.ArrayList");
            IntPtr methodAdd = AndroidJNIHelper.GetMethodID(listObj.GetRawClass(), "add", "(Ljava/lang/Object;)Z");
            foreach (string item in list) {
			    AndroidJNI.CallBooleanMethod(listObj.GetRawObject(), methodAdd, AndroidJNIHelper.CreateJNIArgArray(new string[] {item}));
		    }	
            
            return listObj;
        }

        protected internal static AndroidJavaObject GetHashMapFromDictionary(Dictionary<string, object> dict)
        {
            // quick out if nothing in the dict param
            if (dict == null || dict.Count <= 0) 
            {
                return null;
            }
            
            AndroidJavaObject hashMap = new AndroidJavaObject ("java.util.HashMap");
            IntPtr putMethod = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            object[] args = new object[2];
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                using (var key = new AndroidJavaObject("java.lang.String", kvp.Key))
                {
                    using (var value = new AndroidJavaObject("java.lang.String", kvp.Value))
                    {
                        args[0] = key;
                        args[1] = value;
                        AndroidJNI.CallObjectMethod(hashMap.GetRawObject(), putMethod, AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }
            
            return hashMap;
        }

        protected internal static Dictionary<string, object> GetDictionaryFromHashMap(AndroidJavaObject hashmap)
        {
            Dictionary<string, object> dict = new Dictionary<string,object> ();
            AndroidJavaObject entrySet = hashmap.Call<AndroidJavaObject>("entrySet");
            AndroidJavaObject[] array = entrySet.Call<AndroidJavaObject[]> ("toArray");
            foreach (AndroidJavaObject keyValuepair in array) 
            {
                string key = keyValuepair.Call<string> ("getKey");
                string value = keyValuepair.Call<string> ("getValue");
                dict.Add (key, value);
            }

            if (dict == null || dict.Count <= 0) 
            {
                return null;
            }
            return dict;
        }
        #endif
    }
}