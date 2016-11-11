using UnityEngine;
using System.Collections;

public class AudioPlay : StateMachineBehaviour {
	public AudioClip attackaudio;
	AudioSource audio;
	float timetemp = 0;
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		audio = player.GetComponent<AudioSource> ();
		//audio.PlayOneShot(attackaudio);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		timetemp += Time.deltaTime;
		if (timetemp > 0.3f) {
			audio.PlayOneShot (attackaudio);
			timetemp = -5f;
		} else if (timetemp < 0) {
			timetemp = -5f;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		timetemp = 0;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
