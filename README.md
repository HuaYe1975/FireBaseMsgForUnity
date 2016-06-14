# FireBaseMsgForUnity
Firebase Message (Push notification) For Unity
# How to use it

Set correct bundle ID in Unity's player settings

##iOS
Just drag FirebaseManager prefab to your scene and replace your GoogleService-Info.plist file in Assets/Firebase/Config/.
That's ALL.

##Android
Execute "Firebase → Refresh Android Manifest" From Unity editor menu. This will automatically replaces the $(applicationId) to the current bundle IDs for the necessary manifest files (firebase-common, firebase-iid).
Replace your google-services.json file in Assets/Firebase/Config/. This will automatically update the resources "values.xml" with the correct values.

#Reference
https://github.com/firebase/quickstart-ios

https://github.com/firebase/quickstart-android
