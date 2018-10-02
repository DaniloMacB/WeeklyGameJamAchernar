using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
	
	bool isPaused = false;
	
	void Awake(){
		
	}
	
	void Start(){
		
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GamePause ();
		}
	}

	public void GamePause(){
		isPaused = !isPaused;
		if (isPaused) {
			Time.timeScale = 0f;
		} 
		else 
		{
			Time.timeScale = 1f;
		}
	}
}
