using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text tokenLabel = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (string.IsNullOrEmpty (tokenLabel.text)) {
			tokenLabel.text = FireBaseWrapper.Firebase_Token ();
			Debug.Log (tokenLabel.text);
		}
	}
}
