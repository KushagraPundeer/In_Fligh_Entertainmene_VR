using UnityEngine;
using System.Collections;

public class mapSceneController : MonoBehaviour {

	/// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("Fire2"))
			Application.LoadLevel ("mainMenue");
	}
}
