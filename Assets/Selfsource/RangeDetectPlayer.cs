using UnityEngine;
using System.Collections;

public class RangeDetectPlayer : MonoBehaviour {

	public bool detectplayerFlag = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == "PlayerCollider")
		{
			detectplayerFlag = true;
			Debug.Log("TRUE");
		}
	}
	void OnTriggerExit(Collider col) {
		if(col.gameObject.tag == "PlayerCollider")
		{
			detectplayerFlag = false;
		}
	}
}
