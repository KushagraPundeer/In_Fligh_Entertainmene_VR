using UnityEngine;
using System.Collections;

public class muchWow : MonoBehaviour {

	// Use this for initialization
	void Update () {
		if(GameObject.Find("fromTennis") != null) 
			this.GetComponent<TextMesh>().text = "If you enjoyed exploring the Wimbledon Arena, \ndon't miss Finair's great deals on flights to the United Kingdom.";

	}
	

}
