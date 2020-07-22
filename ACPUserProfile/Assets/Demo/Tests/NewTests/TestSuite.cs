/*
TestSuite.cs

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class TestSuite
    {    
        [UnityTest]
        public IEnumerator Test_ExtensionVersion()
        {
            if (Application.platform == RuntimePlatform.Android) {
                return AssertEqualResult("ExtensionVersion", "UserProfile extension version : 1.1.0"); 
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                return AssertEqualResult("ExtensionVersion", "UserProfile extension version : 2.1.0"); 
            } else {
                return null;
            }
        }

        [UnityTest]
        public IEnumerator Test_GetUserAttributes()
        {
            yield return LoadScene();
            InvokeButtonClick("UpdateUserAttribute");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("GetUserAttributes");
            yield return new WaitForSeconds(1f);
            Assert.Greater(GetActualResult().Length, "Attributes are : ".Length);
        }

        [UnityTest]
        public IEnumerator Test_RemoveUserAttribute()
        {
            yield return LoadScene();
            InvokeButtonClick("RemoveUserAttributes");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("UpdateUserAttribute");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("RemoveUserAttribute");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("GetUserAttributes");
            yield return new WaitForSeconds(1f);
            Assert.AreEqual("Attributes are : ", GetActualResult());
        }

        [UnityTest]
        public IEnumerator Test_RemoveUserAttributes()
        {
            yield return LoadScene();
            InvokeButtonClick("UpdateUserAttributes");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("RemoveUserAttributes");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("GetUserAttributes");
            yield return new WaitForSeconds(1f);
            Assert.AreEqual("Attributes are : ", GetActualResult());
        }

        [UnityTest]
        public IEnumerator Test_UpdateUserAttributes()
        {
            yield return LoadScene();
            InvokeButtonClick("UpdateUserAttributes");
            yield return new WaitForSeconds(1f);
            InvokeButtonClick("GetUserAttributes");
            yield return new WaitForSeconds(1f);
            Assert.AreEqual("Attributes are : {\"mapKey\": \"mapValue\"}", GetActualResult());
        }

        // Helper functions
        private IEnumerator LoadScene() {
            AsyncOperation async = SceneManager.LoadSceneAsync("Demo/DemoScene");

            while (!async.isDone)
            {
                yield return null;
            }
        }

        private void InvokeButtonClick(string gameObjName) {
            var gameObj = GameObject.Find(gameObjName);
            var button = gameObj.GetComponent<Button>();
            button.onClick.Invoke();
        }

        private string GetActualResult()
        {
            var callbackResultsGameObject = GameObject.Find("CallbackResults");
            var callbackResults = callbackResultsGameObject.GetComponent<Text>();
            return callbackResults.text;
        }

        private IEnumerator AssertEqualResult(string gameObjectName, string expectedResult) {
            yield return LoadScene();
            InvokeButtonClick(gameObjectName);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(expectedResult, GetActualResult());
        }
    }
}
