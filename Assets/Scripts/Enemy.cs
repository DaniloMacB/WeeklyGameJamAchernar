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

    public enum mode { Stopped, Walker, Flying }
    public mode mobMode;

	public enum attackStyle { Physical, Distance }
	public attackStyle attackMethod;
	public GameObject bulletPrefab;
	public Transform bulletPos;
	public float bulletSpeed;
	public float bulletShootTime;
	public bool shooting = false;

	public float distanceFromPlayer;
	public float distanceToAttack;
	public float speed;

	bool canMove = true;

	public Transform waypointRight;
	public Transform waypointLeft;

	public Transform waypointToFollow;


	Transform player;

	float playerPosX;

	void Awake () {
		anim = GetComponent<Animator> ();
		rgbd = GetComponent<Rigidbody2D> ();
		player = FindObjectOfType<PlayerController> ().transform;

		if (startDir == dir.Right)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
			curDir = dir.Right;
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            curDir = dir.Left;
        }

        if (mobMode == mode.Walker)
        {
		    curState = state.Walk;
            anim.SetBool("Walk", true);
        }
        else if (mobMode == mode.Stopped)
        {
            curState = state.Idle;
            anim.SetBool("Idle", true);
        }

		curLife = maxLife;

	}
	
	// Update is called once per frame
	void Update () {

		distanceFromPlayer = Vector2.Distance (transform.position, player.position);
		playerPosX = player.position.x - transform.position.x;

		if (distanceFromPlayer <= distanceToAttack) {
			curState = state.Attacking;


			if (playerPosX <= -0.001f) {
				//GetComponent<SpriteRenderer> ().flipX = false;
				transform.localScale = new Vector3(1f, 1f, 1f);
			} else {
				//GetComponent<SpriteRenderer> ().flipX = true;
				transform.localScale = new Vector3(-1f, 1f, 1f);
			}

			if (attackMethod == attackStyle.Physical && !shooting) {
                StartCoroutine(Attack_CR());
            } else if (attackMethod == attackStyle.Distance && !shooting) {
				StartCoroutine (Shoot_CR ());
			}

		} else {
			curState = state.Walk;
		}

        if (mobMode == mode.Stopped)
        {
            return;
        }

		if (canMove && curState == state.Walk) {
			if (curDir == dir.Right) {
				transform.Translate (Vector2.right * speed * Time.deltaTime);
				//GetComponent<SpriteRenderer> ().flipX = true;
				transform.localScale = new Vector3(-1f, 1f, 1f);
			} else {
				transform.Translate (Vector2.left * speed * Time.deltaTime);
				transform.localScale = new Vector3(1f, 1f, 1f);
				//GetComponent<SpriteRenderer> ().flipX = false;
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

	IEnumerator Shoot_CR(){

		shooting = true;
		GameObject bulletShoot = Instantiate(bulletPrefab, bulletPos.position, Quaternion.identity);
		if(transform.localScale.x == -1f)
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.right * (bulletSpeed * 5f) * Time.deltaTime);
		else
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.left * (bulletSpeed * 5f) * Time.deltaTime);

        anim.SetBool("Attacking", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);
        yield return new WaitForSeconds (bulletShootTime);
		shooting = false;

	}

    IEnumerator Attack_CR()
    {
        shooting = true;
        anim.SetBool("Attacking", true);
        anim.SetBool("Walk", false);
        anim.SetBool("Idle", false);

        if(distanceFromPlayer <= distanceToAttack)
        {
            float distanceY = transform.position.y - player.position.y;
            print(distanceY);
            if(distanceY <= 1f)
            player.GetComponent<PlayerController>().TakeDamage(10);
        }

        yield return new WaitForSeconds(bulletShootTime);
        shooting = false;
    }



}
