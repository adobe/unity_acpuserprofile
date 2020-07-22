# Adobe Experience Platform - UserProfile plugin for Unity apps

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
    - [Initialization](#initialization)
    - [UserProfile methods](#UserProfile-methods)
- [Running Tests](#running-tests)
- [Sample App](#sample-app)
- [Contributing](#contributing)
- [Licensing](#licensing)

## Prerequisites

The `Unity Hub` application is required for development and testing. Inside of `Unity Hub`, you will be required to download the current version of the `Unity` app.

[Download the Unity Hub](http://unity3d.com/unity/download). The free version works for development and testing, but a Unity Pro license is required for distribution. See [Distribution](#distribution) below for details.

#### FOLDER STRUCTURE
Plugins for a Unity project use the following folder structure:

`{Project}/Assets/Plugins/{Platform}`

## Installation
- Download [ACPCore-0.0.1-Unity.zip](https://github.com/adobe/unity-acpcore/tree/master/bin/ACPCore-0.0.1-Unity.zip) 
- Unzip `ACPCore-0.0.1-Unity.zip`
- Import `ACPCore.unitypackage` via Assets->Import Package

- Download [ACPUserProfile-0.0.1-Unity.zip](https://github.com/adobe/unity_acpuserprofile/tree/master/bin/ACPUserProfile-0.0.1-Unity.zip) 
- Unzip`ACPUserProfile-0.0.1-Unity.zip`
- Import `ACPUserProfile.unitypackage` via Assets->Import Package
## Usage

### [UserProfile](https://aep-sdks.gitbook.io/docs/using-mobile-extensions/profile)

#### Initialization
##### Initialize by registering the extensions and calling the start function for core
```
using com.adobe.marketing.mobile;
using AOT;

[MonoPInvokeCallback(typeof(AdobeStartCallback))]
public static void HandleStartAdobeCallback()
{
    ACPCore.ConfigureWithAppID("<appId>");    
}

public class MainScript : MonoBehaviour
{
    // Start is called before the first frame update
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
    }
}
```

#### UserProfile methods

##### Getting UserProfile version:
```cs
ACPUserProfile.ExtensionVersion();
```

#### Get user profile attributes which match the provided keys:
```cs
[MonoPInvokeCallback(typeof(AdobeGetUserAttributesCallback))]
public static void HandleAdobeGetUserAttributesCallback(string userAttributes)
{
    print("Attributes are : " + userAttributes);
    results = "Attributes are : " + userAttributes;
}

var attributeKeys = new List<string>(); 
attributeKeys.Add("attrNameTest");
attributeKeys.Add("mapKey");
ACPUserProfile.GetUserAttributes(attributeKeys, HandleAdobeGetUserAttributesCallback);
```

#### Remove user profile attribute if it exists:
```cs
ACPUserProfile.RemoveUserAttribute("attrNameTest");
```

#### Remove provided user profile attributes if they exist:
```cs
var attributeKeys = new List<string>(); 
attributeKeys.Add("attrNameTest");
attributeKeys.Add("mapKey");
ACPUserProfile.RemoveUserAttributes(attributeKeys);
```

#### Set a single user profile attribute:
```cs
ACPUserProfile.UpdateUserAttribute("attrNameTest", "attrValueTest");
```

#### Set multiple user profile attributes:
```cs
var dict = new Dictionary<string, object>();
dict.Add("mapKey", "mapValue");
dict.Add("mapKey1", "mapValue1");
ACPUserProfile.UpdateUserAttributes(dict);
```

## Running Tests
1. Open the demo app in unity.
2. Open the test runner from `Window -> General -> TestRunner`.
3. Click on the `PlayMode` tab.
4. Connect an Android or iOS device as we run the tests on a device in play mode.
5. Select the platform for which the tests need to be run from `File -> Build Settings -> Platform`. 
5. Click `Run all in player (platform)` to run the tests.

## Sample App
Sample App is located at *unity-acpuserprofile/ACPUserProfile/Assets/Demo*.
To build demo app for specific platform follow the below instructions.

###### Add core plugin
- Download [ACPCore-0.0.1-Unity.zip](https://github.com/adobe/unity-acpcore/tree/master/bin/ACPCore-0.0.1-Unity.zip) 
- Unzip `ACPCore-0.0.1-Unity.zip`
- Import `ACPCore.unitypackage` via Assets->Import Package

###### Android
1. Make sure you have an Android device connected.
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `Android` from the __Platform__ window
1. If `Android` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
2. You will be asked to provide a location to save the build. Make a new directory at *unity_acpuserprofile/ACPUserProfile/Builds* (this folder is in the .gitignore file)
3. Name build whatever you want and press __Save__
4. `Unity` will build an `apk` file and automatically deploy it to the connected device

###### iOS
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `iOS` from the __Platform__ window
1. If `iOS` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
1. You will be asked to provide a location to save the build. Make a new directory at *unity_acpuserprofile/ACPUserProfile/Builds* (this folder is in the .gitignore file)
1. Name build whatever you want and press __Save__
1. `Unity` will create and open an `Xcode` project
1. From the Xcode project run the app on a simulator.
1. If you get an error `Symbol not found: _OBJC_CLASS_$_WKWebView`. Select the Unity-iPhone target -> Go to Build Phases tab -> Add `Webkit.Framework` to `Link Binary with Libraries`.

## Contributing
Looking to contribute to this project? Please review our [Contributing guidelines](.github/CONTRIBUTING.md) prior to opening a pull request.

We look forward to working with you!

## Licensing
This project is licensed under the Apache V2 License. See [LICENSE](LICENSE) for more information.
