using UnityEngine;
using System.Collections;

public class FireBaseManager : Firebase.FirebaseMonoSingleton<FireBaseManager> {

	// Use this for initialization
	void Start () {
		FireBaseWrapper.Firebase_Init ();
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			FireBaseWrapper.Firebase_Disconnect ();
		} else {
			FireBaseWrapper.Firebase_Connect ();
		}
	}
}
