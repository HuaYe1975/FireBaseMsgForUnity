using UnityEngine;
using System.Collections;

public class FireBaseManager : MonoSingleton<FireBaseManager> {

	// Use this for initialization
	void Start () {
		FireBaseWraper.Firebase_Init ();
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			FireBaseWraper.Firebase_Disconnect ();
		} else {
			FireBaseWraper.Firebase_Connect ();
		}
	}
}
