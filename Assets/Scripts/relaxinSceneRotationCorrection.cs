using UnityEngine;
using System.Collections;

public class relaxinSceneRotationCorrection : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Update () {
		if (Application.loadedLevelName != "relaxinSubmenu") {
			Vector3 targetPoint = new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation (-targetPoint, Vector3.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * 2.0f);
		}
	}
	

}
