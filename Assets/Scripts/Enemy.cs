using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {


	public int curLife;
	public int maxLife;


	Animator anim;
	Rigidbody2D rgbd;
	public enum dir { Right, Left }
	public dir curDir;
	public dir startDir;

	public enum state { Idle, Walk, Attacking }
	public state curState;

	public float distanceFromPlayer;
	public float distanceToAttack;
	public float speed;

	bool canMove = true;

	public Transform waypointRight;
	public Transform waypointLeft;

	public Transform waypointToFollow;

	Transform player;

	void Awake () {
		anim = GetComponent<Animator> ();
		rgbd = GetComponent<Rigidbody2D> ();
		player = FindObjectOfType<PlayerController> ().transform;

		if (startDir == dir.Right)
			curDir = dir.Right;
		else
			curDir = dir.Left;

		curState = state.Walk;

		curLife = maxLife;
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove && curState == state.Walk) {
			if (curDir == dir.Right) {
				transform.Translate (Vector2.right * speed * Time.deltaTime);
				GetComponent<SpriteRenderer> ().flipX = true;
			} else {
				transform.Translate (Vector2.left * speed * Time.deltaTime);
				GetComponent<SpriteRenderer> ().flipX = false;
			}
			anim.SetBool ("Walk", true);
			anim.SetBool ("Attacking", false);
			anim.SetBool ("Idle", false);
		} else if (canMove && curState == state.Attacking) {
			anim.SetBool ("Walk", false);
			anim.SetBool ("Attacking", true);
			anim.SetBool ("Idle", false);
		} else{
			anim.SetBool ("Walk", false);
			anim.SetBool ("Attacking", false);
			anim.SetBool ("Idle", true);
		}

		if(Vector2.Distance(transform.position, waypointRight.position) <= 0.5f){
			curDir = dir.Left;
		}else if (Vector2.Distance(transform.position, waypointLeft.position) <= 0.5f){
			curDir = dir.Right;
		}
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Bullet") {
			anim.SetTrigger("Hit");
			TakeDamage (col.GetComponent<Bullet>().bulletDamage);
			Destroy (col.gameObject);
		}
	}

	public void TakeDamage(int damageToTake){
		curLife -= damageToTake;
		if (curLife <= 0) {
			anim.SetTrigger ("Death");
		}
	}


	public void DestroyObject(){
		Destroy (this.gameObject);
	}

	public void StopObject(){
		canMove = false;
	}

	public void MoveObject(){
		canMove = true;
	}

	public void Death(){
		rgbd.simulated = false;
		GetComponent<Collider2D> ().enabled = false;
	}



}
