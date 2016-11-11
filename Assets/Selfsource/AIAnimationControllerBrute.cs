using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(Animator))]
public class AIAnimationControllerBrute : MonoBehaviour {

	public NavMeshAgent navControl;
	public Animator animator;
	public AnimatorStateInfo animateState;
	public int state;
	public bool attackFlag;
	public bool specialAttack;
	public bool lightHit;
	public float stateTimeInterval = 2.0f;
	public float time_tmp = 0;
	public Transform target1;
	public Transform target2;
	public Transform target3;
	public GameObject playerObject;
	public DetectPlayer detectplayer;
	public int movedirection = 1;
	public AudioClip hit1;
	public AudioClip hit2;
	public AudioClip hit3;
	public AudioClip hit4;
	public AudioSource audio;
	// Use this for initialization
	void Start () {
		navControl = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		playerObject = GameObject.FindWithTag("Player");
		detectplayer = GetComponentInChildren<DetectPlayer> ();
		target1 = GameObject.Find ("Target11").transform;
		target2 = GameObject.Find ("Target12").transform;
		target3 = GameObject.Find ("Target13").transform;
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!target1 || !target2 ) {
			target1 = GameObject.Find ("Target11").transform;
			target2 = GameObject.Find ("Target12").transform;
			target3 = GameObject.Find ("Target13").transform;
			return;
		}
		animateState = animator.GetCurrentAnimatorStateInfo (0);

		if (animateState.IsName ("Attack1") || animateState.IsName ("Attack2") || animateState.IsName ("Attack3") || animateState.IsName ("MoveAttack") || animateState.IsName ("RangeAttack") || animateState.IsName ("SpecialAttack")) {
			attackFlag = false;
			AttackAnimationPlay (state, attackFlag);
		}

		if (!detectplayer.detectplayerFlag) {
			navControl.speed = 1.5f;
			if(movedirection == 1){
				navControl.destination = target1.position;
				//Debug.Log( navControl.pathStatus);
				if(Vector3.Distance (transform.position, target1.position) < 1.0f)
					movedirection = 2;
			}else if(movedirection == 2){
				navControl.destination = target2.position;
				if(Vector3.Distance (transform.position, target2.position) < 1.0f)
					movedirection = 3;
			} else if(movedirection == 3){
				navControl.destination = target3.position;
				if(Vector3.Distance (transform.position, target3.position) < 1.0f)
					movedirection = 1;
			}

			animator.SetFloat("Vertical", 0.5f);
		} else {
			if(Vector3.Distance(playerObject.transform.position,transform.position) <= 3f){
				animator.SetFloat("Vertical", 0.0f);
				//transform.LookAt(playerObject.transform.position);
				navControl.speed = 0f;

				if (animateState.IsName ("Ground") ) {
					Vector3 relativepos = playerObject.transform.position - transform.position;
					Quaternion rotation = Quaternion.LookRotation(relativepos);
					//transform.rotation = new Quaternion ( transform.rotation.x,rotation.y, transform.rotation.z,rotation.w);
					transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
				}
			}
			else{
				if(Vector3.Distance(playerObject.transform.position,transform.position) > 4f) 
					animator.SetFloat("Vertical", 1f);
				navControl.speed = 3;

			}
	


			time_tmp += Time.deltaTime;
			if (time_tmp >= stateTimeInterval) {
				state = Random.Range (0, 7);
				time_tmp = 0;
				if (animateState.IsName ("Ground")) {
					attackFlag = true;
					AttackAnimationPlay (state, attackFlag);
				}

			}
			if (animateState.IsName ("Attack1") || animateState.IsName ("Attack2") || animateState.IsName ("Attack3") || animateState.IsName ("MoveAttack") || animateState.IsName ("RangeAttack") || animateState.IsName ("SpecialAttack")) {
				attackFlag = false;
				AttackAnimationPlay (state, attackFlag);
				navControl.speed = 0;
				if(animateState.IsName("MoveAttack") || animateState.IsName ("RangeAttack") || animateState.IsName ("SpecialAttack")){
					specialAttack = true;
					Debug.Log("specialAttack = " + specialAttack);
				}

			}else{
				if(Vector3.Distance(playerObject.transform.position,transform.position) > 3f){
					navControl.speed = 3;
					navControl.destination = playerObject.transform.position;
				}else{
					navControl.speed = 0;
				}
			}

		}


		if (animateState.IsName ("LightHit")) {
			animator.SetBool("LightHit",false);
			navControl.speed = 0;
		}


		if (this.gameObject.GetComponent<HealthDamage> ().health <= 0) {
			animator.SetBool("Death", true);
			Destroy( this.gameObject,10.0f);
			navControl.speed = 0;
		}
		//animator.SetBool("LightHit",lightHit);



	}

	void AttackAnimationPlay(int state,bool attackFlag){
		switch (state) {
		case 1:
			animator.SetBool("Attack1",attackFlag);
			break;
		case 2:
			animator.SetBool("Attack2",attackFlag);
			break;
		case 3:
			animator.SetBool("Attack3",attackFlag);
			break;
		case 4:
			animator.SetBool("MoveAttack",attackFlag);
			break;
		case 5:
			animator.SetBool("RangeAttack",attackFlag);
			break;
		case 6:
			animator.SetBool("SpecialAttack",attackFlag);
			break;
		}
	}
}
