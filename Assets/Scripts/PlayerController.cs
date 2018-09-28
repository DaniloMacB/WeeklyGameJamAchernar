using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	private Rigidbody2D rgbd;

	public float speed;
	public float jumpForce;

	// Use this for initialization
	void Awake () 
	{
		rgbd = GetComponent<Rigidbody2D> ();
	}


	void Update(){
		float MovX = Input.GetAxisRaw ("Horizontal");
		rgbd.velocity = new Vector2(MovX * speed, rgbd.velocity.y);

		if (Input.GetButtonDown ("Jump")) {
			rgbd.velocity = Vector2.zero;
			rgbd.AddForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}
	}

}