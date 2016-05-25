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
}
