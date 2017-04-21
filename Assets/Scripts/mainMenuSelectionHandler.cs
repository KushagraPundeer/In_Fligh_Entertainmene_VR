using UnityEngine;
using System.Collections;

public class mainMenuSelectionHandler : MonoBehaviour {
		
	public bool locked = false;
	public int itemSelected = 0;
	public GameObject[] menuItemOutlines;
	public string[] levelLoaders = new string[7] {"infoScene", "aroundMe", "maps", "relaxinScene", "youtubeScene", "airplaneView", "tennisScene"};
		
	// Use this for initialization
	void Start () {
		menuItemOutlines = new GameObject[7] {GameObject.Find("mainMenuSelector1"), GameObject.Find("mainMenuSelector2"), GameObject.Find("mainMenuSelector3"), 
			GameObject.Find("mainMenuSelector4"), GameObject.Find("mainMenuSelector5"), GameObject.Find("mainMenuSelector6"), GameObject.Find("mainMenuSelector7") };

		setUpSubmenuItems ();
	}
	
	public void setUpSubmenuItems() {
		for(int index = 0; index < 7; index++) {
			if(index != 0)
				menuItemOutlines[index].GetComponent<SpriteRenderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!locked) {
			if (Input.GetAxisRaw ("Horizontal") == 1 && itemSelected != 6)
				changeSelectedItem (itemSelected + 1);
			else if (Input.GetAxisRaw ("Horizontal") == -1 && itemSelected != 0)
				changeSelectedItem (itemSelected - 1);

			if(Input.GetButtonUp("Fire1")){
				Application.LoadLevel(levelLoaders[itemSelected]);
			}
		}
	}
	
	public void changeSelectedItem(int newSelectedIndex) {
		menuItemOutlines [itemSelected].GetComponent<SpriteRenderer> ().enabled = false;
		menuItemOutlines [newSelectedIndex].GetComponent<SpriteRenderer> ().enabled = true;
		itemSelected = newSelectedIndex;
		StartCoroutine (takeOne());
	}
	
	public IEnumerator takeOne() {
		locked = true;
		yield return new WaitForSeconds (.5f);
		locked = false;
	}
}

