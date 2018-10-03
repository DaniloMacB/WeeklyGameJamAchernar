using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {


	#region vida do personagem
	public int maxLife;
	public int curLife;
    public Image lifeImage;
    #endregion


    AudioSource _audioSource;														// audiosource


	#region arma

	public Text ammoText;															// texto municao da arma
	public Text weaponText;															// texto nome da arma

	public Arma armaAtual;															// arma atual scriptableobject

	public string nome;																// nome da arma
	public string descricao;														// descricao da arma

	public Sprite spriteArma;														// sprite da arma

	public int shootingMode; 														// 0 = auto, 1 = semi

	public int bulletsPerMag;														// balas por clipe 
	public int bulletsLeft;															// balas restantes
	public int currentBullets;														// balas atuais

	public AudioClip shootSound;													// som de tiro
	public AudioClip reloadSound;													// som de reload

	public float fireRate;															// taxa de disparos
	public float reloadTime;														// tempo de reload

	float fireTimer;																// fire timer

	private bool shootInput;														// input de shoot
	public bool isReloading;														// var está carregando

	public GameObject bulletPrefab;													// prefab da bala
	public Transform muzzleFlash;													// de onde a bala sai e sai o flash da arma
	public float speedBullet;														// velocidade q a bala sai
	#endregion





	public bool invulnerable = false;														// var invulneravel
	public float hitInvulnerableTime;													// var tempo invulneravel

	public CharacterController2D controller;										// var character controller 2d
	private Rigidbody2D rgbd;														// var rigidbody

	public float runSpeed;															// var velocidade de andar
	public float dashSpeed;															// var velocidade do dash

	bool canMove = true;

	public enum dir { left, right };												// enum de direcao
	public dir direcaoAtual;														// direcao atual

	bool jump = false;																// var jump
	//bool crouch = false;															// var crouch
	//bool dash = false;																// var dash

	float horizontalMove = 0f;														// movimento horizontal (nao mexer)

	private Animator animator;														// var animator



	void Awake(){
		direcaoAtual = dir.right;													// comeca olhando pra direita
		rgbd = GetComponent<Rigidbody2D> ();										// define o rigidbody
		animator = GetComponent<Animator>();										// define o animator
		_audioSource = GetComponent<AudioSource> ();								// define o audiosource

        

		currentBullets = bulletsPerMag;												// balas atuais = balas por municao
		curLife = maxLife;
		UpdateWeapon ();															// atualiza a arma
		UpdateAmmoText();															// atualiza texto de  municao
		UpdateWeaponText ();														// atualiza texto da arma (nome)
        UpdateLifeImage();
    }
	
	


	void Update(){

		animator.SetBool ("OnFloor", controller.m_Grounded);

		#region apenas teste da função de pegar armas novas
		if (Input.GetKeyDown (KeyCode.C)) {
			UpdateWeapon ();
			UpdateAmmoText();
			UpdateWeaponText ();
		}
		#endregion

		#region arma
		if (fireTimer < fireRate)													// se o firetimer for menor q a taxa de disparo
		{
			fireTimer += Time.deltaTime;											// firetimer aumenta com o tempo
		}

		switch (shootingMode)														// no modo de tiro
		{
		case 0:																		// caso seja 0 (auto)
			shootInput = Input.GetButton("Fire1");									// atira usando getbutton (por frame)
			break;																	// quebra
		case 1:																		// caso seja 1 (semi)
			shootInput = Input.GetButtonDown("Fire1");								// atira usando getbuttondown (momento que é apertado)
			break;																	// quebra
		}

		if (shootInput)																// se apertar pra atirar
		{
			if (currentBullets > 0){												// se tiver mais balas q 0
				Fire();																// atira
			}
			else if (bulletsLeft > 0){												// seano se tiver menos municao q 0
				DoReload();															// recarrega
			}
		}

		if (Input.GetKeyDown(KeyCode.R))											// se apertar R
		{
			if (currentBullets <= bulletsPerMag && bulletsLeft > 0)					// se tiver municao sobrando
				DoReload();															// recarrega
		}
		#endregion



		if(Input.GetAxisRaw ("Horizontal") < 0){  									// Muda a direção
			direcaoAtual = dir.left;												// do personagem
		}																			// baseado
		if (Input.GetAxisRaw ("Horizontal") > 0) {									// nas inputs
			direcaoAtual = dir.right;												// Horizontais
		}

		horizontalMove = Input.GetAxisRaw ("Horizontal") * runSpeed;				// define o movimento horizontal (input * speed)
		
		animator.SetFloat("Walk", Mathf.Abs(Input.GetAxisRaw("Horizontal")));		// seta o valor de walk para o valor absoluto da input horizontal

		if (Input.GetButtonDown ("Jump")) {											// se pular
			jump = true;															// pulo = true
		}																			// ...............
		
		animator.SetFloat("JumpSpeed", rgbd.velocity.y);							// seta a variavel da vel do pulo pra animacao
		
		if (Input.GetButtonDown ("Crouch")) {										// se agachar
			//crouch = true;															// agachado = true
		} else if (Input.GetButtonUp ("Crouch")) {									// senao
			//crouch = false;															// agachado = false
		}                                                                           // ................

        if (Input.GetButtonDown ("Dash")) {											// se apertar o dash
        	if (direcaoAtual == dir.left) {											// se tiver olhando pra esquerda
        		rgbd.velocity = Vector2.zero;										// zera a velocidade do rigidbody
        		rgbd.AddForce (Vector2.left * dashSpeed * 10f);						// adiciona força pra esquerda multiplicado por dashspeed * 10
        	} else {																// senao se tiver olhandopra direita
        		rgbd.velocity = Vector2.zero;										// zera a velocidade do rigidbody
        		rgbd.AddForce (-Vector2.left * dashSpeed * 10f);					// adiciona força pra direita multiplicado por dashspeed * 10
        	}																		//
        	StartCoroutine (invulnerable_CR(hitInvulnerableTime));										// inicia a coroutine para ficar invulneravel uns segundos
        	animator.SetTrigger("Dash");
        }

	}

	void FixedUpdate(){
		if(canMove)
		controller.Move (horizontalMove * Time.fixedDeltaTime, false, jump);		// faz com que o charactercontroller2d mova o personagem
		jump = false;																// o pulo se torna false

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Damage") {
			if (invulnerable) {
				print ("Invulnerable, not hitting");
			} else {
				StartCoroutine (invulnerable_CR (hitInvulnerableTime));
                //animator.SetTrigger("Hurt");
            }

			Destroy (col.gameObject);
		}

		if (col.tag == "Portal") {
			int sceneIndex = SceneManager.GetActiveScene ().buildIndex + 1;
			print ("index " + sceneIndex);
			print ("bs " + SceneManager.sceneCountInBuildSettings);
			if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
				Debug.LogError ("ERRO: CENA MAXIMA ATINGIDA");
			else
				SceneManager.LoadScene (sceneIndex);
		}
		

	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.transform.tag == "Spike") {
			if (invulnerable) {
				print ("Invulnerable, not hitting");
			} else {
				StartCoroutine (invulnerable_CR (hitInvulnerableTime));
				animator.SetTrigger("Hurt");
				TakeDamage(10);
				Vector2 dir = col.contacts[0].point - (new Vector2(transform.position.x, transform.position.y));
				dir = -dir.normalized;
				rgbd.velocity = Vector2.zero;
				rgbd.AddForce(dir*700f);
			}
		}
	}

	void UpdateWeapon(){
		nome = armaAtual.nome;														// puxa nome da arma
		descricao = armaAtual.descricao;											// puxa descricao da arma
		spriteArma = armaAtual.spriteArma;											// puxa sprite da arma
		shootingMode = armaAtual.shootingMode;										// puxa modo de tiro da arma
		bulletsPerMag = armaAtual.bulletsPerMag;									// puxa quantas balas tem o pente
		bulletsLeft = armaAtual.bulletsLeft;										// puxa quantas balas tem sobrando
		currentBullets = armaAtual.currentBullets;									// puxa quantas balas tem atualmente
		shootSound = armaAtual.shootSound;											// puxa o som do tiro
		reloadSound = armaAtual.reloadSound;										// puxa o som do reload
		fireRate = armaAtual.fireRate;												// puxa a taxa de disparos
		reloadTime = armaAtual.reloadTime;											// puxa o tempo de releoad
		bulletPrefab = armaAtual.bulletPrefab;										// puxa o prefab da bala
		speedBullet = armaAtual.speedBullet;										// puxa a velocidade de bala
	}

	public void Reload()																	// void de reload
	{
		if (bulletsLeft <= 0) return;														// se tiver menos balas que 0 retorna e não executa a funcao
		StartCoroutine(reload_QR());														// começa a coroutine de reload
		_audioSource.PlayOneShot(reloadSound);												// toca o som do tiro
		int bulletsToLoad = bulletsPerMag - currentBullets;									// calcula balas para recarregar
		int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft; // calcula balas restantes

		bulletsLeft -= bulletsToDeduct;														// calcula as balas que sobraram
		currentBullets += bulletsToDeduct;													// calcula as balas atuais
		UpdateAmmoText();																	// atualiza o texto de municao
	}

	public void TakeDamage(int damageToTake){
		curLife -= damageToTake;
		animator.SetTrigger ("Hurt");
        
        if (curLife <= 0) {
			print ("Game Over");
		}
        if (curLife < 0)
        {
            curLife = 0;
        }

        UpdateLifeImage();

    }

    private void UpdateLifeImage()
    {
        float lifeUpdated = (curLife / 100f);
        lifeImage.fillAmount = lifeUpdated;
    }


    private void DoReload()																	// funcao para forçar o reload
	{
		//AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if (isReloading)																	// se tiver recarregando
		{																					
			return;																			// não executa a funcao
		} else
		{																					// senão
			Reload();																		// executa o void reload
		}

		//anim.CrossFadeInFixedTime("Reload", 0.01f);
	}

	private void UpdateAmmoText()															// atualiza texto das municoes
	{
		ammoText.text = currentBullets + " / " + bulletsLeft;								// texto = municao atual / municao restante
	}

	private void UpdateWeaponText(){														// atualiza nome da arma
		weaponText.text = nome;																// texto = nome da arma
	}

	private void PlayShootSound()															// void para tocar o som de tiro
	{
		_audioSource.PlayOneShot(shootSound);												// toca uma vez o som do tiro
	}

	private void Fire()																							// void para atirar
	{
		if (fireTimer < fireRate || currentBullets <= 0 || isReloading) return;									// se não tiver municao ou nao tiver dado o tempo, para a funcao

		animator.SetTrigger("Shoot");
		StartCoroutine (canMove_CR (0.375f));
		GameObject bulletShoot = Instantiate(bulletPrefab, muzzleFlash.position, Quaternion.identity);			// instancia prefab da bala 

		if(direcaoAtual == dir.right)																			// se tiver olhando pra direita
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.right * speedBullet * 5f * Time.deltaTime);// atira pra direita
		else
			bulletShoot.GetComponent<Rigidbody2D>().AddForce(Vector2.left * speedBullet * 5f * Time.deltaTime);	// senão atira pra esquerda


		PlayShootSound();																	// toca o som do tiro
		currentBullets--;																	// diminui quantas balas tem
		UpdateAmmoText();																	// atualiza texto de municao
		fireTimer = 0.0f; 																	// reseta o timer	
	}

	private void Knockback(Transform col, float knockbackForce){
		Vector2 knockbackDir = (this.transform.position - col.transform.position).normalized; 
		rgbd.velocity = Vector2.zero;
		transform.Translate (knockbackDir * knockbackForce);
	}

	IEnumerator reload_QR()																	// coroutine de reload
	{
		isReloading = true;																	// ta carregando = true
		yield return new WaitForSeconds(reloadTime);										// espera o tempo de reload
		isReloading = false;																// ta carregando = falso
	}

	IEnumerator canMove_CR(float moveTime){
		rgbd.velocity = Vector2.zero;
		canMove = false;
		yield return new WaitForSeconds (moveTime);
		canMove = true;
	}

	IEnumerator invulnerable_CR(float howMuch){													// coroutine de invulnerabilidade
		invulnerable = true;														// invulneravel = true
		yield return new WaitForSeconds (howMuch);							// espera o tempo do invulnerabletime
		invulnerable = false;														// invulnerable = false
	}

}