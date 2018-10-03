using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance { get; set; }

	public int mobsScene;
	int killedMobs;

	public GameObject levelPortal;

	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance = this)
			Destroy (gameObject);

		killedMobs = 0;
		levelPortal.SetActive (false);

	}

	public void killedMonster(){
		killedMobs++;
		if (killedMobs >= mobsScene) {
			int sceneIndex = SceneManager.GetActiveScene ().buildIndex + 1;
			if (sceneIndex >= SceneManager.sceneCountInBuildSettings) {
				Debug.LogError ("Cena máxima atingida, spawnando portal pro level 1");
				levelPortal.SetActive (true);
			} else {
				levelPortal.SetActive (true);
			}
		}
	}
}
