using UnityEngine;
using System.Collections;
using System;

public class CheckPlayerAwake : MonoBehaviour {

	public bool isActive = true;
	public bool isCoStarted = false;

	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0 || Input.GetButtonUp ("Fire1") != false) && !isActive) {
			if (!isCoStarted)
				StartCoroutine (sleepTimer ());
			else {
				StopCoroutine (sleepTimer ());
				changeToRiftCameras ();
			}
		} else {
			isActive = false;
		}

	}

	public void changeToRiftCameras() {
		GameObject.Find ("Main Camera").GetComponent<Camera> ().enabled = false;
		GameObject.Find ("CenterEyeAnchor").GetComponent<Camera> ().enabled = true;
	}

	public void changeToMainCameras() {
		GameObject.Find ("Main Camera").GetComponent<Camera> ().enabled = true;
		GameObject.Find ("CenterEyeAnchor").GetComponent<Camera> ().enabled = false;
	}

	public IEnumerator sleepTimer() {
		yield return new WaitForSeconds (600);
		changeToMainCameras ();
	}
}
