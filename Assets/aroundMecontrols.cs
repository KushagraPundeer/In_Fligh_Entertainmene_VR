using UnityEngine;
using System.Collections;

public class aroundMecontrols : MonoBehaviour {

	public bool locked = false;
	public int midVisibleCard;



	void Start() {

	}
	
	// Update is called once per frame
	void Update () {
		if (!locked) {
			if (Input.GetAxisRaw ("Horizontal") == 1)
				changeSelectedItem (1);
			else if (Input.GetAxisRaw ("Horizontal") == -1)
				changeSelectedItem (-1);
			if(Input.GetButtonUp("Fire2"))
				Application.LoadLevel("mainMenue");
		}
	}

	public IEnumerator takeOne() {
		locked = true;
		yield return new WaitForSeconds (1);
		locked = false;
	}

	public void changeSelectedItem(int dir) {

	}
}
