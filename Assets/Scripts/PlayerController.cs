using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {



	public CharacterController2D controller;
	private Rigidbody2D rgbd;

	public float runSpeed;
	public float dashSpeed;

	public enum dir { left, right };
	public dir direcaoAtual;

	bool jump = false;
	bool crouch = false;
	bool dash = false;

	float horizontalMove = 0f;

	void Awake(){
		direcaoAtual = dir.right;
		rgbd = GetComponent<Rigidbody2D> ();
	}
	
	
	private Animator animator;
	
	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update(){

		if(Input.GetAxisRaw ("Horizontal") < 0){
			direcaoAtual = dir.left;
		}
		if (Input.GetAxisRaw ("Horizontal") > 0) {
			direcaoAtual = dir.right;
		}

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;
		
		animator.SetFloat("walk", (Input.GetAxisRaw ("Horizontal")));

		if (Input.GetButtonDown ("Jump")) {
			jump = true;
		}

		if (Input.GetButtonDown ("Crouch")) {
			crouch = true;
		} else if (Input.GetButtonUp ("Crouch")) {
			crouch = false;
		}

		if (Input.GetButtonDown ("Dash")) {
			if(direcaoAtual == dir.left)
				rgbd.AddForce (Vector2.left * dashSpeed * 10f);
			else
				rgbd.AddForce (-Vector2.left * dashSpeed * 10f);
		}

	}

	void FixedUpdate(){
		controller.Move (horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;

	}
}