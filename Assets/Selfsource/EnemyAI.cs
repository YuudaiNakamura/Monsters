using UnityEngine;
using System.Collections;
using SnazzlebotTools.ENPCHealthBars;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour {

	Animator animator;
	NavMeshAgent agent;
	DetectPlayer detectplayer;
	RangeDetectPlayer rangedetect;
	Transform mainplayer;
	float speed;
	float timeinterval = 0;
	public float Distance;
	public GameObject[] patrolposition;
	public int movedirection = 1;
	public GameObject fov;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		detectplayer = GetComponentInChildren<DetectPlayer> ();
		rangedetect = GetComponentInChildren<RangeDetectPlayer> ();
		mainplayer = GameObject.FindGameObjectWithTag ("Player").transform;
		if (this.gameObject.name == "NinjaAI(Clone)") {
			patrolposition = GameObject.FindGameObjectsWithTag ("NinjaPatrol");
		} else if(this.gameObject.name == "BruteAI(Clone)"){
			patrolposition = GameObject.FindGameObjectsWithTag ("BrutePatrol");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<ENPCHealthBar> ().Value > 0) { // survive
			if(detectplayer.detectplayerFlag || rangedetect.detectplayerFlag){ // when player comes in range
				var distance = Vector3.Distance(transform.position, mainplayer.position);

				if(distance > Distance){
					agent.speed = 3;
					agent.destination = mainplayer.position;
					speed += Time.deltaTime;
					if(speed >= 1)
						speed = 1;
					animator.SetFloat("Vertical",speed);
				}else{
					timeinterval += Time.deltaTime;
					agent.speed = 0;
					speed -= Time.deltaTime;
					if(speed <= 0)
						speed = 0;
					animator.SetFloat("Vertical",speed);
					if (animator.GetCurrentAnimatorStateInfo(0).IsName ("Ground") ) {
						Vector3 relativepos = mainplayer.position - transform.position;
						Quaternion rotation = Quaternion.LookRotation(relativepos);
						transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10);
					}
					if(timeinterval >= 2){
						var state = Random.Range(0,6);
						AttackAnimationPlay(state);
						timeinterval = 0;
					}
				}
			} else{ // out
				agent.speed = 1.5f;
				if(movedirection == 1){
					agent.destination = patrolposition[0].transform.position;
					//Debug.Log( navControl.pathStatus);
					if(Vector3.Distance (transform.position, patrolposition[0].transform.position) < 1.0f)
						movedirection = 2;
				}else if(movedirection == 2){
					agent.destination = patrolposition[1].transform.position;
					if(Vector3.Distance (transform.position, patrolposition[1].transform.position) < 1.0f)
						movedirection = 3;
				} else if(movedirection == 3){
					agent.destination = patrolposition[2].transform.position;
					if(Vector3.Distance (transform.position, patrolposition[2].transform.position) < 1.0f)
						movedirection = 1;
				}
				
				animator.SetFloat("Vertical", 0.5f);
			}
		} else if (GetComponent<ENPCHealthBar> ().Value <= 0) { // die
			ResetTrigger();
			animator.SetTrigger("Death");
			fov.SetActive(false);
			DestroyObject(this.gameObject,3f);
		}


	}

	void AttackAnimationPlay(int state){
		switch (state) {
		case 1:
			animator.SetTrigger("Attack1");
			break;
		case 2:
			animator.SetTrigger("Attack2");
			break;
		case 3:
			animator.SetTrigger("Attack3");
			break;
		case 4:
			animator.SetTrigger("MoveAttack");
			break;
		case 5:
			animator.SetTrigger("SpecialAttack");
			break;
		}
	}
	void ResetTrigger(){
		animator.ResetTrigger ("Attack1");
		animator.ResetTrigger ("Attack2");
		animator.ResetTrigger ("Attack3");
		animator.ResetTrigger ("MoveAttack");
		animator.ResetTrigger ("SpecialAttack");
		animator.ResetTrigger ("Jump");
		animator.ResetTrigger ("Hit");
		animator.ResetTrigger ("Death");
	}
}
