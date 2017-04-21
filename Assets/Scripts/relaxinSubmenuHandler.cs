using UnityEngine;
using System.Collections;

public class relaxinSubmenuHandler : MonoBehaviour {

	public RelaxinPassedInfo passedInfoObj;
	public GameObject submenuTitle;
	public GameObject[] submenuItems;
	public GameObject[] submenuSelectors;

	public int numberOfSubmenuItems;
	public string[] submenuItemsTitleString;

	public GameObject submenuPrefab;

	public bool locked = false;
	
	public int itemSelected = 0;

	public string[] songURLs;
	
	// Use this for initialization
	void Start () {
		passedInfoObj = GameObject.Find ("PassedInfo").GetComponent<RelaxinPassedInfo>();
		numberOfSubmenuItems = passedInfoObj.numberOfSubmenuItems;
		submenuItemsTitleString = passedInfoObj.submenuItemTitles;
		songURLs = passedInfoObj.songURLS;
		GameObject.Find("submenuTitleObj").GetComponent<TextMesh>().text = passedInfoObj.submenuTitle + ":";

		setUpSubmenuItems ();
	}

	public void setUpSubmenuItems() {
		submenuItems = new GameObject[numberOfSubmenuItems];
		submenuSelectors = new GameObject[numberOfSubmenuItems];
		for (int index = 0; index < numberOfSubmenuItems; index++) {
			Vector3 pos = Vector3.zero;
			if(numberOfSubmenuItems % 2 == 0)
				pos = new Vector3(0, 20f * (numberOfSubmenuItems / 2 - (index + 1)) + 10f, 60f);
			else
				pos = new Vector3(0, 20f * (numberOfSubmenuItems / 2 - index), 60f);
			submenuItems[index] = (GameObject)Instantiate(submenuPrefab, pos, Quaternion.identity);
			submenuItems[index] .transform.SetParent(GameObject.Find("submenu holder").transform);
			submenuItems[index].GetComponent<TextMesh>().text = submenuItemsTitleString[index];
			submenuSelectors[index] = submenuItems[index].transform.FindChild("border1").gameObject;
			if(index != 0)
				submenuSelectors[index].GetComponent<SpriteRenderer>().enabled = false;
		}
	}
				
	// Update is called once per frame
	void Update () {
		if (!locked) {
			if (Input.GetAxisRaw ("Vertical") == -1 && itemSelected != numberOfSubmenuItems - 1)
				changeSelectedItem (itemSelected + 1);
			else if (Input.GetAxisRaw ("Vertical") == 1 && itemSelected != 0)
				changeSelectedItem (itemSelected - 1);
			if(Input.GetButtonUp("Fire1"))
				ifSelected(itemSelected);
			else if(Input.GetButtonUp("Fire2"))
				Application.LoadLevel("relaxinScene");
		}
	}
	
	public void changeSelectedItem(int newSelectedIndex) {
		submenuSelectors [itemSelected].GetComponent<SpriteRenderer> ().enabled = false;
		submenuSelectors [newSelectedIndex].GetComponent<SpriteRenderer> ().enabled = true;
		itemSelected = newSelectedIndex;
		StartCoroutine (takeOne());
	}

	public void ifSelected(int myIndex) {
		GameObject.Find ("AUDIO").GetComponent<AudioSource> ().clip = Resources.Load<AudioClip> (songURLs [myIndex]);
		GameObject.Find ("AUDIO").GetComponent<AudioSource> ().Play ();
	}
	
	public IEnumerator takeOne() {
		locked = true;
		yield return new WaitForSeconds (1);
		locked = false;
	}
}
