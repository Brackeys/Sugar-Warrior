using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	private static List<Rigidbody2D> EnemyRBs;

	public float moveSpeed = 5f;

	[Range(0f, 1f)]
	public float turnSpeed = .1f;

	public float repelRange = .5f;
	public float repelAmount = 1f;

	public float startMaxChaseDistance = 20f;
	private float maxChaseDistance;

	[Header("Shooting")]

	public bool isShooter = false;
	public float strafeSpeed = 1f;
	public float shootDistance = 5f;
	public GameObject bulletPrefab;
	public Transform firePoint;
	public float fireRate = 1f;
	private float nextTimeToFire = .5f;

	private Rigidbody2D rb;

	private Vector3 velocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();

		if (EnemyRBs == null)
		{
			EnemyRBs = new List<Rigidbody2D>();
		}

		moveSpeed *= (Progression.Growth - 1f) * 0.5f + 1f;

		EnemyRBs.Add(rb);
	}

	private void OnDestroy()
	{
		EnemyRBs.Remove(rb);
	}

	// Update is called once per frame
	void FixedUpdate () {

		maxChaseDistance = startMaxChaseDistance * Progression.Growth;

		float distance = Vector2.Distance(rb.position, PlayerController.Position);

		if (distance > maxChaseDistance)
		{
			Destroy(gameObject);
			return;
		}

		Vector2 direction = (PlayerController.Position - rb.position).normalized;

		Vector2 newPos;

		if (isShooter)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
			rb.rotation = angle;

			if (distance > shootDistance)
			{
				newPos = MoveRegular(direction);
			} else
			{
				newPos = MoveStrafing(direction);
			}

			Shoot();

			newPos -= rb.position;

			rb.AddForce(newPos, ForceMode2D.Force);

		} else
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
			rb.rotation = Mathf.LerpAngle(rb.rotation, angle, turnSpeed);

			newPos = MoveRegular(direction);

			rb.MovePosition(newPos);
		}
	}

	void Shoot ()
	{
		if (Time.time >= nextTimeToFire)
		{
			GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
			Destroy(bullet, 30f);

			nextTimeToFire = Time.time + 1f / fireRate;
		}
	}

	Vector2 MoveStrafing (Vector2 direction)
	{
		Vector2 newPos = transform.position + transform.right * Time.fixedDeltaTime * strafeSpeed;
		return newPos;
	}

	Vector2 MoveRegular (Vector2 direction)
	{
		Vector2 repelForce = Vector2.zero;
		foreach (Rigidbody2D enemy in EnemyRBs)
		{
			if (enemy == rb)
				continue;

			if (Vector2.Distance(enemy.position, rb.position) <= repelRange)
			{
				Vector2 repelDir = (rb.position - enemy.position).normalized;
				repelForce += repelDir;
			}
		}

		Vector2 newPos = transform.position + transform.up * Time.fixedDeltaTime * moveSpeed;
		newPos += repelForce * Time.fixedDeltaTime * repelAmount;

		return newPos;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, maxChaseDistance);
	}
}
