using UnityEngine;
using System.Collections;
using SnazzlebotTools.ENPCHealthBars;

public class HealthDamage : MonoBehaviour {

	public float health;
	public float damage;
	public GameObject bar;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//health = 100.0f - damage; 
		bar.GetComponent<ENPCHealthBar> ().Value = (int) health;
	}
}
