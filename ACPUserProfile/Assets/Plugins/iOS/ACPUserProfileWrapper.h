/*
ACPUserProfileWrapper.h

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

#ifndef Unity_iPhone_ACPUserProfileWrapper_h
#define Unity_iPhone_ACPUserProfileWrapper_h

extern "C" {
    const char *acp_UserProfile_ExtensionVersion();
    void acp_UserProfile_RegisterExtension();
    void acp_GetUserAttributes(const char *attributeKeys, void (*callback)(const char *userAttributes));
    void acp_RemoveUserAttribute(const char *attributeName);
    void acp_RemoveUserAttributes(const char *attributeNames);
    void acp_UpdateUserAttribute(const char *attributeName, const char *attributeValue);
    void acp_UpdateUserAttributes(const char *attributeMap);
}

#endif