using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using SnazzlebotTools.ENPCHealthBars;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
public class PlayerUserControl : MonoBehaviour {
	Animator animator;
	AnimatorStateInfo currentstateinfo;
	GameObject cam;
	//Rigidbody rigibody;
	public float smooth;
	public float timetemp;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		//rigibody = GetComponent<Rigidbody> ();
		animator.speed = 1.3f;
	}
	
	// Update is called once per frame
	void Update () {
		currentstateinfo = animator.GetCurrentAnimatorStateInfo (0);
		if (GetComponent<ENPCHealthBar> ().Value > 0) {

			var v = CrossPlatformInputManager.GetAxis ("Vertical");
			var h = CrossPlatformInputManager.GetAxis ("Horizontal");
			if (v == 0) {
				animator.SetFloat ("Vertical", v);
			}
			if (h == 0) {
				animator.SetFloat ("Horizontal", h);
			}
		
			Quaternion targetl = Quaternion.Euler (0, cam.transform.rotation.y + 270, 0); // Vector3 Direction when facing left
			Quaternion targetr = Quaternion.Euler (0, cam.transform.rotation.y + 90, 0); // Vector3 Direction when facing right way
			Quaternion targetf = Quaternion.Euler (0, cam.transform.rotation.y + 0, 0); // Vector3 Direction when facing left
			Quaternion targetb = Quaternion.Euler (0, cam.transform.rotation.y + 180, 0); // Vector3 Direction when facing right way
		
			if (h < 0.0f) { // if input is lower than 0 turn to targetf
				transform.rotation = Quaternion.Lerp (transform.rotation, targetl, Time.deltaTime * smooth); 
				animator.SetFloat ("Horizontal", -1 * h);
			}
			if (h > 0.0f) { // if input is higher than 0 turn to targetb
				animator.SetFloat ("Horizontal", h);
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

			if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
				animator.SetTrigger ("Jump");
				//rigibody.AddForce(new Vector3(0,30000,0));
			}

			if (CrossPlatformInputManager.GetButtonDown ("Fire2")) {  // rangeattack
				animator.SetTrigger ("SpecialAttack");
			}

			if (Mathf.Abs (v) >= 0.7f || Mathf.Abs (h) >= 0.7f) { // moveattack
				if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
					animator.SetTrigger ("MoveAttack");
				}
			} else {						
				if (CrossPlatformInputManager.GetButtonDown ("Fire1")) { // normal attack 1
					animator.SetTrigger ("Attack1");
				}
				if (currentstateinfo.IsName ("Attack1")) {
					if (CrossPlatformInputManager.GetButtonDown ("Fire1")) { // normal attack 2
						animator.SetTrigger ("Attack2");
					}
				} else if (currentstateinfo.IsName ("Attack2")) {
					if (CrossPlatformInputManager.GetButtonDown ("Fire1")) { // normal attack 3
						animator.SetTrigger ("Attack3");
					}
				}

			}
			timetemp += Time.deltaTime;
			if (timetemp >= 1.0f) {
				timetemp = 0;
				ResetTrigger();
			}
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				timetemp = 0;	
			}

		} else if (GetComponent<ENPCHealthBar> ().Value <= 0) {
			animator.SetTrigger("Death");
		}

		//Debug.Log (transform.forward.y);
	}

	void ResetTrigger(){
		animator.ResetTrigger ("Attack1");
		animator.ResetTrigger ("Attack2");
		animator.ResetTrigger ("Attack3");
		animator.ResetTrigger ("MoveAttack");
		animator.ResetTrigger ("SpecialAttack");
		animator.ResetTrigger ("RangeAttack");
		animator.ResetTrigger ("Jump");
		animator.ResetTrigger ("Hit");
		animator.ResetTrigger ("Death");
		animator.ResetTrigger ("HitBack");
		animator.ResetTrigger ("HitLeft");
		animator.ResetTrigger ("HitRight");
	}
}
