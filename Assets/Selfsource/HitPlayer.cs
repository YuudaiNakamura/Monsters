using UnityEngine;
using System.Collections;
using SnazzlebotTools.ENPCHealthBars;

public class HitPlayer : MonoBehaviour {

	public GameObject Player;
	public GameObject mainCharacter;
	public int damage;
	// Use this for initialization
	void Start () {
		mainCharacter = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter(Collider other){
		
		if (other.gameObject.tag == "PlayerCollider") {

			if(Player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
				if(other.gameObject.GetComponentInParent<ENPCHealthBar>().Value > 0){
					other.gameObject.GetComponentInParent<ENPCHealthBar>().Value -= damage;
					other.gameObject.GetComponentInParent<Animator>().SetTrigger("Hit");
					Vector3 relativepos = Player.transform.position - mainCharacter.transform.position;
					Quaternion rotation = Quaternion.LookRotation(relativepos);
					mainCharacter.transform.rotation = new Quaternion ( mainCharacter.transform.rotation.x,rotation.y, mainCharacter.transform.rotation.z,rotation.w);
				}
			}
			
		}
	}
}
