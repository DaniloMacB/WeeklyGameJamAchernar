using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
	
	public bool isPaused = false;
	
	public GameObject pausePanel;
	
	public GameObject player;
	
	void Awake(){
		player = GameObject.FindGameObjectWithTag("Player");
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
			pausePanel.SetActive (true);
			player.SetActive (false);
		} 
		else 
		{
			Time.timeScale = 1f;
			pausePanel.SetActive (false);
			player.SetActive (true);
		}
	}
}
