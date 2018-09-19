using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Progression : MonoBehaviour {

	public static int Score;
	public static float Growth;
	public static bool IsGrowing;

	public static Progression instance;

	public Level[] levels;

	public GameObject levelUpEffect;

	public PlayerShooting playerShooting;
	public Player player;
	public EnemySpawner enemySpawner;

	public TextMeshProUGUI scoreText;
	public Slider scoreSlider;
	public Animator levelUpAnimator;
	public Animator endGameAnimator;

	private void Awake()
	{
		if (instance == null)
			instance = this;

		Score = 0;
		scoreText.text = Score.ToString();

		Growth = 1f;
		IsGrowing = false;

		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	private void Start()
	{
		enemySpawner.currentWave = levels[0].wave;
		levels[0].isUnlocked = true;
		scoreSlider.maxValue = levels[1].unlockScore;
	}

	public void AddScore (int amount)
	{
		Score += amount;

		scoreText.text = Score.ToString();
		scoreSlider.value = Score;

		for (int i = 0; i < levels.Length; i++)
		{
			if (!levels[i].isUnlocked && Score >= levels[i].unlockScore)
			{
				if (levels[i].endTheGame)
				{
					StartCoroutine(EndTheGame());
				} else
				{
					UnlockReward(levels[i].reward);
				}
				
				enemySpawner.currentWave = levels[i].wave;

				if (i < levels.Length - 1)
				{
					scoreSlider.minValue = Score;
					scoreSlider.maxValue = levels[i + 1].unlockScore;
				}

				levels[i].isUnlocked = true;
			}
		}
	}

	IEnumerator EndTheGame ()
	{
		IsGrowing = true;

		Time.timeScale = .3f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;

		GameObject effect = Instantiate(levelUpEffect, PlayerController.Position, Quaternion.identity);
		Destroy(effect, 10f);

		endGameAnimator.SetTrigger("EndGame");

		yield return new WaitForSecondsRealtime(10f);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	IEnumerator LevelUp()
	{
		IsGrowing = true;

		Time.timeScale = .3f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;

		GameObject effect = Instantiate(levelUpEffect, PlayerController.Position, Quaternion.identity);
		Destroy(effect, 10f);

		levelUpAnimator.SetTrigger("LevelUp");

		yield return new WaitForSecondsRealtime(.1f);

		float baseScale = Growth;
		float factor = 1.3f;

		float t = 0f;
		while (t < 1f)
		{
			float growth = Mathf.Lerp(1f, factor, t);
			Growth = baseScale * growth;
			t += Time.fixedDeltaTime * 1f;
			yield return 0;
		}

		Growth = baseScale * factor;

		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;

		IsGrowing = false;
	}

	void UnlockReward (RewardTier reward)
	{
		Debug.Log("LEVEL UP!");

		player.health += reward.healthBonus;
		playerShooting.currentWeapon = reward.weapon;

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			enemy.GetComponent<Enemy>().Remove();
		}

		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
		foreach (GameObject bullet in bullets)
		{
			bullet.GetComponent<Bullet>().Remove();
		}

		StartCoroutine(LevelUp());
	}

}
