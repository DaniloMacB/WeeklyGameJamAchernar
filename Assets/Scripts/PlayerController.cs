using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	private Rigidbody2D rgbd;

	public float speed;
	public float jumpForce;

	private float movX;

	// Use this for initialization
	void Awake () 
	{
		rgbd = GetComponent<Rigidbody2D> ();
	}


	void Update(){
		movX = Input.GetAxisRaw ("Horizontal");

		if (Input.GetButtonDown ("Jump")) {
			rgbd.velocity = Vector2.zero;
			rgbd.AddForce (new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}
	}
		
	void FixedUpdate(){
		rgbd.velocity = new Vector2(movX * speed, rgbd.velocity.y);
	}

}