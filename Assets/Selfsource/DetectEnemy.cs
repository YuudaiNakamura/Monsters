using UnityEngine;
using System.Collections;
using SnazzlebotTools.ENPCHealthBars;

public class DetectEnemy : MonoBehaviour {

	public GameObject Player;
	public int damage;
	// Use this for initialization
	void Start () {
		//Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Enemy") {
			//Debug.Log("ENEMY");
			if(Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
				if(other.gameObject.GetComponentInParent<ENPCHealthBar>().Value > 0){
					other.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
					if(Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack")){
						other.gameObject.GetComponentInParent<Animator>().SetTrigger("RangeHit");
					}
					other.gameObject.GetComponentInParent<ENPCHealthBar>().Value -= damage;
				}
				//Debug.Log("DETECT");
			}

		}
	}
}
