﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int bulletDamage;

	void Start(){
		Destroy (gameObject, 4f);
	}
}
