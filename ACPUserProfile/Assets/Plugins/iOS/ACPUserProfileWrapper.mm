/*
ACPUserProfileWrapper.mm

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.
*/

#import "ACPUserProfileWrapper.h"
#import "ACPUserProfile.h"
#import "ACPCore.h"
#import "ACPCoreWrapper.h"

NSDictionary *getDictionaryFromJsonStringForUserProfile(const char *jsonString);
NSArray *getArrayFromJsonString(const char *jsonString);


const char *acp_UserProfile_ExtensionVersion() {
   return [[ACPUserProfile extensionVersion] cStringUsingEncoding:NSUTF8StringEncoding];
}

void acp_UserProfile_RegisterExtension() {
    [ACPUserProfile registerExtension];
}

void acp_GetUserAttributes(const char *attributeKeys, void (*callback)(const char *userAttributes)) {
    NSString *nsAttributeName = [[NSString alloc] initWithUTF8String:attributeKeys];
    NSArray *userAttributesToRetrieve = getArrayFromJsonString(attributeKeys);
    [ACPUserProfile getUserAttributes:userAttributesToRetrieve withCompletionHandler:^(NSDictionary* _Nullable userAttributes, NSError* error) {
        if(userAttributes != nil && userAttributes.count != 0) {
            NSData* jsonData = [NSJSONSerialization dataWithJSONObject:userAttributes options:0 error:&error];
            NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            NSLog(@"jsonData as string:\n%@", jsonString);
            if (callback != nil) {
                callback([jsonString cStringUsingEncoding:NSUTF8StringEncoding]);
            }
        }
        if(error) {
            const char *nsError = [error.localizedDescription cStringUsingEncoding:NSUTF8StringEncoding];
            if (callback != nil) {
                callback([[NSString stringWithFormat:@"User profile request error code: %s", nsError] cStringUsingEncoding:NSUTF8StringEncoding]);
            }
        }
    }];
}

void acp_RemoveUserAttribute(const char *attributeName) {
    NSString *nsAttributeName = [NSString stringWithCString:attributeName encoding:NSUTF8StringEncoding];
    [ACPUserProfile removeUserAttribute:nsAttributeName];
}

void acp_RemoveUserAttributes(const char *attributeNames) {
    NSArray *userAttributesToRemove = getArrayFromJsonString(attributeNames);
    [ACPUserProfile removeUserAttributes:userAttributesToRemove];
}

void acp_UpdateUserAttribute(const char *attributeName, const char *attributeValue) {
    NSString *nsAttributeName = [NSString stringWithCString:attributeName encoding:NSUTF8StringEncoding];
    NSString *nsattributeValue = [NSString stringWithCString:attributeValue encoding:NSUTF8StringEncoding];
    [ACPUserProfile updateUserAttribute:nsAttributeName withValue:nsattributeValue];
}

void acp_UpdateUserAttributes(const char *attributeMap) {
    NSDictionary *nsattributeMap = getDictionaryFromJsonStringForUserProfile(attributeMap);
    if (nsattributeMap == nil) return;
    [ACPUserProfile updateUserAttributes:nsattributeMap];
}

// Helper methods

NSDictionary *getDictionaryFromJsonStringForUserProfile(const char *jsonString) {
    if (!jsonString) {
        return nil;
    }
    
    NSError *error = nil;
    NSString *tempString = [NSString stringWithCString:jsonString encoding:NSUTF8StringEncoding];
    NSData *data = [tempString dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:data
                                                         options:NSJSONReadingMutableContainers
                                                           error:&error];
    
    return (dict && !error) ? dict : nil;
}

NSArray *getArrayFromJsonString(const char *jsonString) {
    if (!jsonString) {
        return nil;
    }

    NSString *tempString = [[NSString alloc] initWithUTF8String:jsonString];
    NSArray *array = [tempString componentsSeparatedByString:@","];
    
    return array;
}
