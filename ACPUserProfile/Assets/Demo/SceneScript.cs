/*
Copyright 2020 Adobe
All Rights Reserved.
NOTICE: Adobe permits you to use, modify, and distribute this file in
accordance with the terms of the Adobe license agreement accompanying
it. If you have received this file from a source other than Adobe,
then your use, modification, or distribution of it requires the prior
written permission of Adobe. (See LICENSE-MIT for details)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.adobe.marketing.mobile;
using System;
using AOT;
using System.Threading;

public class SceneScript : MonoBehaviour
{
    public static String results;
    public Text callbackResultsText;

    // UserProfile Buttons
    public Button btnExtensionVersion;
    public Button btnGetUserAttributes;
    public Button btnRemoveUserAttribute;
    public Button btnRemoveUserAttributes;
    public Button btnUpdateUserAttribute;
    public Button btnUpdateUserAttributes;
    static CountdownEvent latch;

    [MonoPInvokeCallback(typeof(AdobeStartCallback))]
    public static void HandleStartAdobeCallback()
    {   
        ACPCore.ConfigureWithAppID("launch-ENc28aaf2fb6934cff830c8d3ddc5465b1-development"); 
    }

    [MonoPInvokeCallback(typeof(AdobeGetUserAttributesCallback))]
    public static void HandleAdobeGetUserAttributesCallback(string userAttributes)
    {
        print("Attributes are : " + userAttributes);
        results = "Attributes are : " + userAttributes;
    }

    private void Update()
    {
        callbackResultsText.text = results;
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android) {
            ACPCore.SetApplication();
        }
        ACPCore.SetLogLevel(ACPCore.ACPMobileLogLevel.VERBOSE);
        ACPCore.SetWrapperType();
        ACPIdentity.RegisterExtension();
        ACPUserProfile.RegisterExtension();
        ACPCore.Start(HandleStartAdobeCallback);

        var callbackResultsGameObject = GameObject.Find("CallbackResults");
        callbackResultsText = callbackResultsGameObject.GetComponent<Text>();

        btnExtensionVersion.onClick.AddListener(UserProfileExtensionVersion);
        btnGetUserAttributes.onClick.AddListener(GetUserAttributes);
        btnRemoveUserAttribute.onClick.AddListener(RemoveUserAttribute);
        btnRemoveUserAttributes.onClick.AddListener(RemoveUserAttributes);
        btnUpdateUserAttribute.onClick.AddListener(UpdateUserAttribute);
        btnUpdateUserAttributes.onClick.AddListener(UpdateUserAttributes);
    }

    void UserProfileExtensionVersion()
	{
        Debug.Log("Calling User Profile extensionVersion");
		string UserProfileExtensionVersion = ACPUserProfile.ExtensionVersion();
        Debug.Log("UserProfile extension version : " + UserProfileExtensionVersion);
        results = "UserProfile extension version : " + UserProfileExtensionVersion;
    }

    void GetUserAttributes()
    {
        Debug.Log("Calling GetUserAttributes");
        var attributeKeys = new List<string>(); 
        attributeKeys.Add("attrNameTest");
        attributeKeys.Add("mapKey");
        ACPUserProfile.GetUserAttributes(attributeKeys, HandleAdobeGetUserAttributesCallback);
    }

    void RemoveUserAttribute()
    {
        Debug.Log("Calling RemoveUserAttribute");
        ACPUserProfile.RemoveUserAttribute("attrNameTest");
    }

    void RemoveUserAttributes()
    {
        Debug.Log("Calling RemoveUserAttributes");
        var attributeKeys = new List<string>(); 
        attributeKeys.Add("attrNameTest");
        attributeKeys.Add("mapKey");
        ACPUserProfile.RemoveUserAttributes(attributeKeys);
    }

    void UpdateUserAttribute()
    {
        Debug.Log("Calling UpdateUserAttribute");
        ACPUserProfile.UpdateUserAttribute("attrNameTest", "attrValueTest");
    }

    void UpdateUserAttributes()
    {
        Debug.Log("Calling UpdateUserAttributes");
        var dict = new Dictionary<string, object>();
        dict.Add("mapKey", "mapValue");
        dict.Add("mapKey1", "mapValue1");
        ACPUserProfile.UpdateUserAttributes(dict);
    }
}
