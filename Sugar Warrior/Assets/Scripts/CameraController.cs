using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {

	public CinemachineVirtualCamera vCam;
	
	// Update is called once per frame
	void LateUpdate () {
		vCam.m_Lens.OrthographicSize = Progression.Growth * 5f;
	}
}
