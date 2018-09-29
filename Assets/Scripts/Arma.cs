using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Arma", menuName = "Arma")]
public class Arma : ScriptableObject {


	public new string nome;
	public string descricao;

	public Sprite spriteArma;

	//public enum ShootMode { Auto, Semi }
	public int shootingMode;

	public int bulletsPerMag;
	public int bulletsLeft;
	public int currentBullets;

	public AudioClip shootSound;
	public AudioClip reloadSound;

	public float fireRate;
	public float reloadTime;

	float fireTimer;

	private bool shootInput;

	public GameObject playerObj;
	public GameObject bulletPrefab;
	public float speedBullet;

}
