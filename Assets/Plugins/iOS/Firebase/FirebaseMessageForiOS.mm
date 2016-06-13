//
//  FirebaseMessageForiOS.mm
//  Unity plugin for Firebase Remote notifications.
//
//  Created by KA YOU on 2016/05/23.
//  Copyright (c) 2016年 Romeo Hua. All rights reserved.
//
#if ! __has_feature(objc_arc)
#error This file must be compiled with ARC. Either turn on ARC for the project or use -fobjc-arc flag
#endif

#import "Firebase.h"

@interface FireBaseMessageController : NSObject
{
}
@end

@implementation FireBaseMessageController


+ (FireBaseMessageController*)sharedInstance{
    
    static FireBaseMessageController* sharedInstance;
    static dispatch_once_t once;
    dispatch_once( &once, ^{
        sharedInstance = [[self alloc] init];
    });
    
    return sharedInstance;
}

- (void)Init
{
    // Register for remote notifications
    if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1) {
        // iOS 7.1 or earlier
        UIRemoteNotificationType allNotificationTypes =
        (UIRemoteNotificationTypeSound | UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeBadge);
        [[UIApplication sharedApplication] registerForRemoteNotificationTypes:allNotificationTypes];
    } else {
        // iOS 8 or later
        // [END_EXCLUDE]
        UIUserNotificationType allNotificationTypes =
        (UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge);
        UIUserNotificationSettings *settings =
        [UIUserNotificationSettings settingsForTypes:allNotificationTypes categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
        [[UIApplication sharedApplication] registerForRemoteNotifications];
    }
    
    // [START configure_firebase]
    [FIRApp configure];
    // [END configure_firebase]
    
    // Add observer for InstanceID token refresh callback.
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tokenRefreshNotification:)
                                                 name:kFIRInstanceIDTokenRefreshNotification object:nil];
}

- (void)tokenRefreshNotification:(NSNotification *)notification {
    // Note that this callback will be fired everytime a new token is generated, including the first
    // time. So if you need to retrieve the token as soon as it is available this is where that
    // should be done.
    NSString *refreshedToken = [[FIRInstanceID instanceID] token];
    NSLog(@"InstanceID token: %@", refreshedToken);
    
    // Connect to FCM since connection may have failed when attempted before having a token.
    [self connectToFcm];
    
    // TODO: If necessary send token to appliation server.
}

- (void)connectToFcm {
    [[FIRMessaging messaging] connectWithCompletion:^(NSError * _Nullable error) {
        if (error != nil) {
            NSLog(@"Unable to connect to FCM. %@", error);
        } else {
            NSLog(@"Connected to FCM.");
        }
    }];
}

- (void)disconnect {
    [[FIRMessaging messaging] disconnect];
    NSLog(@"Disconnected from FCM");
}

@end

extern "C" {
    void _Firebase_Init();
    void _Firebase_Connect();
    void _Firebase_Disconnect();
    char* _Firebase_Token();
}

void _Firebase_Init()
{
    [[FireBaseMessageController sharedInstance] Init];
}

void _Firebase_Connect()
{
    [[FireBaseMessageController sharedInstance] connectToFcm];
}

void _Firebase_Disconnect()
{
    [[FireBaseMessageController sharedInstance] disconnect];
}

char* _Firebase_Token()
{
    NSString *token = [[FIRInstanceID instanceID] token];
    if (!token.length)
    {
        return NULL;
    }
    const char* str = [token UTF8String];
    char *ret_val = strdup(str);
    return ret_val;
}
