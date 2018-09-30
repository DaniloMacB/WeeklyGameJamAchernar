using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public void Play()
	{
		SceneManager.LoadScene(1);
	}
	
	public void Quit()
	{
		#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
		#if (UNITY_EDITOR)
		UnityEditor.EditorApplication.isPlaying = false;
		#elif (UNITY_STANDALONE) 
		Application.Quit();
		#elif (UNITY_WEBGL)
		Application.OpenURL("about:blank");
		#endif
	}
}
