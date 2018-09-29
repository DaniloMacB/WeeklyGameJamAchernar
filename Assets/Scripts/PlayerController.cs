using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {


	AudioSource _audioSource;


	#region arma

	public Text ammoText;
	public Text weaponText;

	public Arma armaAtual;

	public string nome;
	public string descricao;

	public Sprite spriteArma;

	//public enum ShootMode { Auto, Semi }
	public int shootingMode; // 0 = semi, 1 = automatico

	public int bulletsPerMag;
	public int bulletsLeft;
	public int currentBullets;

	public AudioClip shootSound;
	public AudioClip reloadSound;

	public float fireRate;
	public float reloadTime;

	float fireTimer;

	private bool shootInput;
	public bool isReloading;

	public GameObject bulletPrefab;
	public Transform muzzleFlash;
	public float speedBullet;
	#endregion





	bool invulnerable = false;
	public float invulnerableTime;

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

	private Animator animator;



	void Awake(){
		direcaoAtual = dir.right;
		rgbd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource> ();

		currentBullets = bulletsPerMag;
		UpdateWeapon ();
		UpdateAmmoText();
		UpdateWeaponText ();

	}
	
	


	void Update(){

		if (Input.GetKeyDown (KeyCode.C)) {
			UpdateWeapon ();
			UpdateAmmoText();
			UpdateWeaponText ();
		}

		#region arma
		if (fireTimer < fireRate)
		{
			fireTimer += Time.deltaTime;
		}

		switch (shootingMode)
		{
		case 0:
			shootInput = Input.GetButton("Fire1");
			break;
		case 1:
			shootInput = Input.GetButtonDown("Fire1");
			break;
		}

		if (shootInput)
		{
			if (currentBullets > 0)
				Fire();
			else if (bulletsLeft > 0)
				DoReload();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (currentBullets <= bulletsPerMag && bulletsLeft > 0)
				DoReload();
		}
		#endregion

		if(Input.GetAxisRaw ("Horizontal") < 0){
			direcaoAtual = dir.left;
		}
		if (Input.GetAxisRaw ("Horizontal") > 0) {
			direcaoAtual = dir.right;
		}

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;
		
		animator.SetFloat("Walk", Mathf.Abs(Input.GetAxisRaw("Horizontal")));

		if (Input.GetButtonDown ("Jump")) {
			jump = true;
		}

		if (Input.GetButtonDown ("Crouch")) {
			crouch = true;
		} else if (Input.GetButtonUp ("Crouch")) {
			crouch = false;
		}

		if (Input.GetButtonDown ("Dash")) {
			if (direcaoAtual == dir.left) {
				rgbd.velocity = Vector2.zero;
				rgbd.AddForce (Vector2.left * dashSpeed * 10f);

			} else {
				rgbd.velocity = Vector2.zero;
				rgbd.AddForce (-Vector2.left * dashSpeed * 10f);
			}
			StartCoroutine (invulnerable_CR());
		}

	}

	void FixedUpdate(){
		controller.Move (horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;

	}

	IEnumerator invulnerable_CR(){
		invulnerable = true;
		yield return new WaitForSeconds (invulnerableTime);
		invulnerable = false;
		
	}

	void UpdateWeapon(){
		nome = armaAtual.nome;
		descricao = armaAtual.descricao;
		spriteArma = armaAtual.spriteArma;
		shootingMode = armaAtual.shootingMode;
		bulletsPerMag = armaAtual.bulletsPerMag;
		bulletsLeft = armaAtual.bulletsLeft;
		currentBullets = armaAtual.currentBullets;
		shootSound = armaAtual.shootSound;
		reloadSound = armaAtual.reloadSound;
		fireRate = armaAtual.fireRate;
		reloadTime = armaAtual.reloadTime;
		bulletPrefab = armaAtual.bulletPrefab;
		speedBullet = armaAtual.speedBullet;	
	}

	public void Reload()
	{
		if (bulletsLeft <= 0) return;

		StartCoroutine(reload_QR());
		_audioSource.PlayOneShot(reloadSound);
		int bulletsToLoad = bulletsPerMag - currentBullets;
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

		bulletsLeft -= bulletsToDeduct;
		currentBullets += bulletsToDeduct;

		UpdateAmmoText();
	}

	private void DoReload()
	{
		//AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if (isReloading)
		{
			return;
		} else
		{
			Reload();
		}

		//anim.CrossFadeInFixedTime("Reload", 0.01f);
	}

	private void UpdateAmmoText()
	{
		ammoText.text = currentBullets + " / " + bulletsLeft;
	}

	private void UpdateWeaponText(){
		weaponText.text = nome;
	}

	private void PlayShootSound()
	{
		_audioSource.PlayOneShot(shootSound);
	}

	private void Fire()
	{
		if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;
		GameObject bulletShoot = Instantiate(bulletPrefab, muzzleFlash.position, Quaternion.identity);

		if(direcaoAtual == dir.right)
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speedBullet * 5f * Time.deltaTime);
		else
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.left * speedBullet * 5f * Time.deltaTime);


		PlayShootSound();
		currentBullets--;
		UpdateAmmoText();
		fireTimer = 0.0f; // reset timer
	}

	IEnumerator reload_QR()
	{
		isReloading = true;
		yield return new WaitForSeconds(reloadTime);
		isReloading = false;
	}
}