using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject {

	public GameObject bulletPrefab;
	public float fireRate;

	public bool shootsRaycasts = false;
	public int raycastDamage = 20;

	public void Shoot (Transform firePoint)
	{
		GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		bullet.transform.localScale *= Progression.Growth;
		Destroy(bullet, 10f);
	}

}
