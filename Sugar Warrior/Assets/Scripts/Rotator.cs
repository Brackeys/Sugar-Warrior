using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float speed = 100f;

	public Transform enemy;
	
	void Start ()
	{
		transform.SetParent(null);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (enemy == null)
		{
			Destroy(gameObject);
			return;
		}

		transform.position = enemy.position;
		transform.Rotate(0f, 0f, speed * Time.fixedDeltaTime);
	}
}
