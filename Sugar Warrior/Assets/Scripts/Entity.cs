using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

	public int health = 20;
	public GameObject deathEffect;

	public void TakeDamage (int amount)
	{
		health -= amount;

		if (health <= 0)
		{
			Die();
		}
	}

	public virtual void Die ()
	{
		GameObject effect = Instantiate(deathEffect, transform.position, transform.rotation);
		effect.transform.localScale = transform.localScale;
		Destroy(effect, 10f);
		Destroy(gameObject);
	}

}
