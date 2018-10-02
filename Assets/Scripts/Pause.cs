using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
	
	public bool isPaused = false;
	
	public GameObject pausePanel;

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GamePause ();
		}
	}

	public void GamePause(){
		isPaused = !isPaused;
		if (isPaused) {
			Time.timeScale = 0f;
			pausePanel.SetActive (true);
		} 
		else 
		{
			Time.timeScale = 1f;
			pausePanel.SetActive (false);
		}
	}
}
