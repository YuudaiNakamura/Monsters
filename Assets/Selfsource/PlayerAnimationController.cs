using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerAnimationController : MonoBehaviour {

	public GameObject cam;
	public Animator animator;
	public CharacterController character;
	//public float time_temp1;
	//public float time_interval;
	//public bool click_flag = false;
	public AnimatorStateInfo currentBaseState;
	public bool attackFlag = false;
	public bool attackFlag1 = false;
	public bool attack1 = false;
	public bool attack2 = false;
	public bool attack3 = false;
	public bool moveattack = false;
	public bool specialattack = false;
	public bool specialattack1 = false;
	public bool rangeattack = false;
	public bool Hit = false;
	public bool Death = false;
	public bool Block = false;
	public bool BlockHit = false;
	public bool Revive = false;
	public AudioSource audio;
	public AudioClip staff1;
	public AudioClip staff2;

	public float speed;

	public float speed1 = 6.0f;
	public float jumpspeed = 8.0f;
	float gravity = 20.0f;
	public Vector3 movedirection = Vector3.zero;

	public bool audioflag = false;
	public float timestep;
	public float smooth;
	static int groundState = Animator.StringToHash("MainLayer.Ground");

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		animator.speed = speed;
		character = GetComponent<CharacterController> ();
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Death == false) {
			var v = CrossPlatformInputManager.GetAxis ("Vertical");
			var h = CrossPlatformInputManager.GetAxis ("Horizontal");
			animator.SetFloat ("Vertical", v);

			Quaternion targetl = Quaternion.Euler (0, cam.transform.rotation.y + 270, 0); // Vector3 Direction when facing left
			Quaternion targetr = Quaternion.Euler (0, cam.transform.rotation.y + 90, 0); // Vector3 Direction when facing right way
			Quaternion targetf = Quaternion.Euler (0, cam.transform.rotation.y + 0, 0); // Vector3 Direction when facing left
			Quaternion targetb = Quaternion.Euler (0, cam.transform.rotation.y + 180, 0); // Vector3 Direction when facing right way
		
			if (h < 0.0f) { // if input is lower than 0 turn to targetf
				transform.rotation = Quaternion.Lerp (transform.rotation, targetl, Time.deltaTime * smooth); 
				animator.SetFloat ("Vertical", -1 * h);
			}
			if (h > 0.0f) { // if input is higher than 0 turn to targetb
				animator.SetFloat ("Vertical", h);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetr, Time.deltaTime * smooth);
			}

			if (v < 0.0f) { // if input is lower than 0 turn to targetf
				transform.rotation = Quaternion.Lerp (transform.rotation, targetb, Time.deltaTime * smooth); 
				animator.SetFloat ("Vertical", -1 * v);
			}
			if (v > 0.0f) { // if input is higher than 0 turn to targetb
				animator.SetFloat ("Vertical", v);
				transform.rotation = Quaternion.Lerp (transform.rotation, targetf, Time.deltaTime * smooth);
			}



			animator.SetFloat ("Horizontal", h);
			/////////////////////////////jump///////////////////
			animator.SetBool ("Jump", CrossPlatformInputManager.GetButtonDown ("Jump"));

			if (character.isGrounded) {
				movedirection.x = 0;
				movedirection.z = 0;
				movedirection = Vector3.Normalize (transform.TransformDirection (movedirection));
				//movedirection = Vector3.Normalize (transform.forward);
				movedirection *= speed1;
				if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
					movedirection.y = jumpspeed;
				}

				//Debug.Log (movedirection);
			} else {
				//movedirection = Vector3.Normalize (transform.forward);
				movedirection.x = transform.forward.x * Mathf.Sqrt (v * v + h * h) * speed1;
				movedirection.z = transform.forward.z * Mathf.Sqrt (v * v + h * h) * speed1;
				//movedirection *= speed1;
			}
			movedirection.y -= gravity * Time.deltaTime;
			character.Move (movedirection * Time.deltaTime);

			////////////////////////////////////////////////////
			/*if (CrossPlatformInputManager.GetButtonDown("Vertical") || CrossPlatformInputManager.GetButtonDown("Horizontal")) {
			click_flag = true;
		}

		if (click_flag) {
			click_flag = false;
			time_interval = time_temp1;
			time_temp1 = 0;
		} else {
			time_temp1 += Time.deltaTime;
		}

		if (time_interval < 0.3f) {

			if(Mathf.Abs(v) == 1 || Mathf.Abs(h) == 1){
				var dash = true;
				animator.SetBool("Dash",dash);
			} else {
				var dash = false;
				animator.SetBool("Dash",dash);
			}

		}
*/

			currentBaseState = animator.GetCurrentAnimatorStateInfo (0);
			
			if (currentBaseState.nameHash == groundState) {
				//Debug.Log ("Standing");
			}
			/*if(CrossPlatformInputManager.GetButtonDown("Fire1")){
				audio.PlayOneShot(staff2);
			}
			if(CrossPlatformInputManager.GetButtonDown("Fire2")){
				audio.PlayOneShot(staff1);
			}*/

			///attack1,2,3,when mouse0 click
			if (Mathf.Abs (v) == 1 || Mathf.Abs (h) == 1) {
				animator.SetBool ("MoveAttack", CrossPlatformInputManager.GetButtonDown ("Fire1"));
				animator.SetBool ("SpecialAttack", CrossPlatformInputManager.GetButtonDown ("Fire2"));
				if(CrossPlatformInputManager.GetButtonDown ("Fire1") || CrossPlatformInputManager.GetButtonDown ("Fire2")){
					//timestep = 0;
					
					if(!audioflag){
						timestep = 0;
						audioflag = true;
					}
				}
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("MoveAttack")) {
					if(audioflag){
						
						timestep += Time.deltaTime;
						if(timestep >= 0.4f ){
							audio.PlayOneShot(staff1);
							audioflag = false;
							timestep = 0;
						} 
						
					}
				}
			} else {
				animator.SetBool ("Attack1", CrossPlatformInputManager.GetButtonDown ("Fire1"));
				if(CrossPlatformInputManager.GetButtonDown ("Fire1") || CrossPlatformInputManager.GetButtonDown ("Fire2")){
					//timestep = 0;

					if(!audioflag){
						timestep = 0;
						audioflag = true;
					}
				}

				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack1")) {
					//Debug.Log ("Attack1");
					Debug.Log( animator.GetCurrentAnimatorStateInfo(0).length);
					attack1 = false;

					if(audioflag){

						timestep += Time.deltaTime;
						if(timestep >= 0.5f){
							audio.PlayOneShot(staff2);
							audioflag = false;
							timestep = 0;
						} 

					}

					
					animator.SetBool ("Attack2", CrossPlatformInputManager.GetButtonDown ("Fire1"));
					if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
						attack2 = true;
					}
					if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
						specialattack = true;
					}
				}

				animator.SetBool ("Attack2", attack2);
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack2")) {
					attack2 = false;

					if(audioflag){
						
						timestep += Time.deltaTime;
						if(timestep >= 0.6f ){
							audio.PlayOneShot(staff2);
							audioflag = false;
							timestep = 0;
						} 
						
					}
					if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
						attack3 = true;
					}
					if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
						specialattack = true;
					}
				}
				animator.SetBool ("Attack3", attack3);

				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack3")) {
					attack3 = false;
					if(audioflag){
						
						timestep += Time.deltaTime;
						if(timestep >= 0.6f ){
							audio.PlayOneShot(staff2);
							audioflag = false;
							timestep = 0;
						} 
						
					}
					if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {
						specialattack = true;
					}
				}

				animator.SetBool ("SpecialAttack", CrossPlatformInputManager.GetButtonDown ("Fire2"));
				if (animator.GetCurrentAnimatorStateInfo (0).IsName ("SpecialAttack")) {
					specialattack = false;
					if(audioflag){
						
						timestep += Time.deltaTime;
						if(timestep >= 0.85f){
							audio.PlayOneShot(staff1);
							audioflag = false;
							timestep = 0;
						} 
						
					}
				}

			
			}

			//if (attack1 || attack2 || attack3 || moveattack || specialattack) {
			if (currentBaseState.IsName ("Attack1") || currentBaseState.IsName ("Attack2") || currentBaseState.IsName ("Attack3") || currentBaseState.IsName ("MoveAttack")) {
				attackFlag = true;
			} else {
				attackFlag = false;
			}

			if (currentBaseState.IsName ("SpecialAttack")) {
				specialattack1 = true;
			} else {
				specialattack1 = false;
			}

			if (currentBaseState.IsName ("HitReact")) {
				Hit = false;
				//Debug.Log(Hit);
			}
			animator.SetBool ("Hit", Hit);


			animator.SetBool ("Block", Block);
			animator.SetBool ("BlockHit", BlockHit);

			animator.SetBool("Revive",Revive);
			if(currentBaseState.IsName("Revive")){
				Revive = false;
			}


		} /*else if (Death == true) {
			if( this.gameObject.GetComponent<HealthDamage> ().health > 0){
				if(Revive){
					animator.SetBool("Revive", true);
					Death = false;
				} else{
					animator.SetBool("Death", true);
				}



				if(currentBaseState.IsName ("Death")){
					Revive = true;
					Debug.Log("Revive = " + true);
					animator.SetBool("Death", false);
				}
			}

		}*/

		if (this.gameObject.GetComponent<HealthDamage> ().health > 0) {
			//Debug.Log("Death = " + true);
			if (currentBaseState.IsTag("death")) {
				Death = false;
				//Debug.Log("Death = " + false);
				Revive = true;
			}
			animator.SetBool ("Death", Death);
			Death = false;
		}



		if (this.gameObject.GetComponent<HealthDamage> ().health <= 0) {
			animator.SetBool("Death", true);
			Destroy(this.gameObject, 3f);
			Death = true;
		}


	}
}
