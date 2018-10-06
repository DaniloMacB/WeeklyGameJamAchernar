using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int bulletDamage;
	public bool right;

	void Start(){
		Destroy (gameObject, 4f);
	}

	void Update(){
		if (right) {
			transform.Translate (Vector2.right * 10f * Time.deltaTime);
		} else {
			transform.Translate (Vector2.left * 10f * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			col.GetComponent<PlayerController> ().TakeDamage(bulletDamage);
		}
	}
}
