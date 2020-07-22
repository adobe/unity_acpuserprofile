 /*
ACPSignal.h

Copyright 2020 Adobe. All rights reserved.
This file is licensed to you under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. You may obtain a copy
of the License at http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software distributed under
the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
OF ANY KIND, either express or implied. See the License for the specific language
governing permissions and limitations under the License.

Signal Version: 2.0.4
*/

#import <Foundation/Foundation.h>


@interface ACPSignal : NSObject {}

#pragma mark - Signal

/**
 * @brief Returns the current version of the ACPSignal Extension.
 */
+ (nonnull NSString*) extensionVersion;

/**
 * @brief Registers the ACPSignal extension with the Core Event Hub.
 */
+ (void) registerExtension;

@end
