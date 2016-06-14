using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class FireBaseWrapper {

#if UNITY_IPHONE
    [DllImport("__Internal")]
	static extern void _Firebase_Init();
	[DllImport("__Internal")]
	static extern void _Firebase_Connect();
	[DllImport("__Internal")]
	static extern void _Firebase_Disconnect();
	[DllImport("__Internal")]
	static extern string _Firebase_Token();
#endif

	public static void Firebase_Init()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_Firebase_Init();
#else
#endif
	}

	public static void Firebase_Connect()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_Firebase_Connect();
#else
#endif
	}

	public static void Firebase_Disconnect()
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		_Firebase_Disconnect();
#else
#endif
	}

	public static string Firebase_Token()
	{
		string token = string.Empty;

		#if UNITY_IPHONE && !UNITY_EDITOR
		token = _Firebase_Token();
		#elif UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass cls_FirebaseInstanceID = new AndroidJavaClass("com.google.firebase.iid.FirebaseInstanceId")) {

			using (AndroidJavaObject obj_FirebaseInstanceID = cls_FirebaseInstanceID.CallStatic<AndroidJavaObject>("getInstance")) {
				
				token = obj_FirebaseInstanceID.Call<string>("getToken");
			}
		}
		#endif
		return token;
	}
}
