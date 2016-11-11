using UnityEngine;
using System.Collections;
using FoV2;

public class DetectPlayer : MonoBehaviour {

	public DoubleFoV fov;
	public bool detectplayerFlag = false;
	public Transform player;
	// Use this for initialization
	void Start () {
		fov = GetComponent<DoubleFoV> ();
		player = GameObject.FindGameObjectWithTag ("PlayerCollider").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (fov.GetDetectedObjects ().Contains (player)) {
			detectplayerFlag = true;
		} else {
			detectplayerFlag = false;
		}
	}

	/*void OnTriggerEnter (Collider col)
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
	}*/
}
