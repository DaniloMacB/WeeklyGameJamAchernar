using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HQControl : MonoBehaviour {

	public GameObject[] hqs;

	int qualTaAtiva = 1;

	void Awake(){

	}

	void Update(){
		if (Input.GetButtonDown ("Jump")) {
			if (qualTaAtiva < hqs.Length) {
				for (int i = 0; i < hqs.Length; i++) {
					if (i != qualTaAtiva) {
						hqs [i].SetActive (false);
					} else {
						hqs [i].SetActive (true);
						break;
					}
					

				}
				qualTaAtiva++;
			} else {
				SceneManager.LoadScene ((SceneManager.GetActiveScene ().buildIndex + 1));
			}
		}
	}
}
