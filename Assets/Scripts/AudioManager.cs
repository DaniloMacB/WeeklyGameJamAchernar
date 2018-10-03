using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour {

	public static AudioManager instance { get; set; }
	//[Range(0,1)]
	//public float musicVolume;
	//[Range(0,1)]
	//public float sfxVolume;

	public AudioSource musicSource;
	public AudioSource sfxSource;

	public Slider musicSlider;
	public Slider sfxSlider;



	void Awake(){
		

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this);
		} else if (instance == this) {
			Destroy (gameObject);
		}

	}

	public void MusicSliderChange(){
		musicSource.volume = musicSlider.value;
	}

	public void SFXSliderChange(){
		sfxSource.volume = sfxSlider.value;
	}
}
