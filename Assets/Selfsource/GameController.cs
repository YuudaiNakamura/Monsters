using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public Transform[] spawnPoints;
	public GameObject ninja;
	public GameObject brute;
	// Use this for initialization
	void Start () {
		//int i = Random.Range (0, 7);
		for (int i = 0; i<=6; i++) {
			Instantiate (ninja, spawnPoints [i].position, spawnPoints [i].rotation);
			Instantiate (brute, spawnPoints [i].position, spawnPoints [i].rotation);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.FindGameObjectsWithTag ("Enemy").Length < 12) {
			int i = Random.Range(0,7);
			Instantiate (ninja, spawnPoints [i].position, spawnPoints [i].rotation);
			Instantiate (brute, spawnPoints [i].position, spawnPoints [i].rotation);
		}
	}
}
