using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

	public Transform firePoint;
	public Weapon currentWeapon;

	public LineRenderer lr;

	private float nextTimeOfFire = 0f;

	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1"))
		{
			if (Time.time >= nextTimeOfFire)
			{
				if (currentWeapon.shootsRaycasts)
				{
					ShootRaycast();
				} else
				{
					currentWeapon.Shoot(firePoint);
				}

				nextTimeOfFire = Time.time + 1f / currentWeapon.fireRate;
			}
		}
	}

	void ShootRaycast ()
	{
		RaycastHit2D[] hits = Physics2D.CircleCastAll(firePoint.position, lr.startWidth, firePoint.up);
		foreach (RaycastHit2D hit in hits)
		{
			Enemy enemy = hit.collider.GetComponent<Enemy>();
			if (enemy != null)
			{
				enemy.TakeDamage(currentWeapon.raycastDamage);
			}
		}

		lr.SetPosition(0, firePoint.position);
		lr.SetPosition(1, firePoint.position + firePoint.up * 100);

		StartCoroutine(FlashLineRenderer());
	}

	IEnumerator FlashLineRenderer()
	{
		lr.enabled = true;

		yield return new WaitForSeconds(0.02f);

		lr.enabled = false;
	}
}
