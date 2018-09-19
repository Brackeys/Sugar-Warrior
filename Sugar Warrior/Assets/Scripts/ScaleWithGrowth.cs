using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithGrowth : MonoBehaviour {

	private Vector3 baseScale;

	public float scaleFactor = 1f;

	// Use this for initialization
	void Start () {
		baseScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = baseScale * ((Progression.Growth - 1f) * scaleFactor + 1f);
	}
}
