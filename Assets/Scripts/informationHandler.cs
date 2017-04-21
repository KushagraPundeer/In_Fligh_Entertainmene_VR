using UnityEngine;
using System.Collections;

public class informationHandler : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonUp("Fire2"))
			Application.LoadLevel("mainMenue");
	}
}
