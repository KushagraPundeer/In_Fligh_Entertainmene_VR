using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class relaxinSceneMenuSelector : MonoBehaviour {

	public string[] submenuTitles = new string[]{"Light Instrument", "White Noise", "ASMR Audio", "Nature"};
	public int numberOfSubmenues = 2;
	public string[,] submenuItemTitles = new string[4,2]{{"Piano", "Meditating Music"}, {"White Noise 1", "White Noise 2"}, {"Massage ASMR", "ASMR"}, {"Nature Sounds 1", "Nature Sounds 2"}};
	public string[,] submenuItemURLs = new string[4,2]{{"SoundFiles/Piano", "SoundFiles/MeditaionMusic"}, {"SoundFiles/WhiteNoise", "SoundFiles/WhiteNoise2"}, 
		{"SoundFiles/MassageASMR", "SoundFiles/ASMR"}, {"SoundFiles/NatureSounds1", "SoundFiles/NatureSounds2"}};
	
	public GameObject[] menuItems;
	public GameObject[] menuItemsBorders;
	public bool locked = false;

	public int itemSelected = 0;

	// Use this for initialization
	void Start () {
		menuItems = new GameObject[4] {GameObject.Find("instrumental_menu_item"), GameObject.Find("white_noise_menu_item"), GameObject.Find("asmr_menu_item"), GameObject.Find("nature_menu_item") };
		menuItemsBorders = new GameObject[4] {GameObject.Find("border1"), GameObject.Find("border2"), GameObject.Find("border3"), GameObject.Find("border4") };

		foreach (GameObject obj in menuItemsBorders) {
			if(obj.name != "border1")
				obj.GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!locked) {
			if (Input.GetAxisRaw ("Horizontal") == 1 && itemSelected != 3)
				changeSelectedItem (itemSelected + 1);
			else if (Input.GetAxisRaw ("Horizontal") == -1 && itemSelected != 0)
				changeSelectedItem (itemSelected - 1);
			if(Input.GetButtonUp("Fire1"))
				ifSelected(itemSelected);
			if(Input.GetButtonUp("Fire2"))
				Application.LoadLevel("mainMenue");
		}
	}

	public void changeSelectedItem(int newSelectedIndex) {
		menuItemsBorders [itemSelected].GetComponent<SpriteRenderer> ().enabled = false;
		menuItemsBorders [newSelectedIndex].GetComponent<SpriteRenderer> ().enabled = true;
		itemSelected = newSelectedIndex;
		StartCoroutine (takeOne());
	}

	public void ifSelected(int index) {
		RelaxinPassedInfo pinfo = GameObject.Find ("PassedInfo").GetComponent<RelaxinPassedInfo>();
		pinfo.numberOfSubmenuItems = numberOfSubmenues;
		pinfo.submenuItemTitles = new string[]{submenuItemTitles[index,0], submenuItemTitles[index,1]};
		pinfo.submenuTitle = submenuTitles [index];
		pinfo.songURLS = new string[]{submenuItemURLs[index,0], submenuItemURLs[index,1]};
		Application.LoadLevel ("relaxinSubmenu");
	}

	public IEnumerator takeOne() {
		locked = true;
		yield return new WaitForSeconds (1);
		locked = false;
	}

}
