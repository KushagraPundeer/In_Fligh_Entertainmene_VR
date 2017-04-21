using UnityEngine;
using System.Collections;

public class RelaxinPassedInfo : MonoBehaviour {

	public int numberOfSubmenuItems;
	public string[] submenuItemTitles;
	public string submenuTitle;

	public string[] songURLS;

	void Start() {
		DontDestroyOnLoad (this.gameObject);
	}

	public void getShitDoneYo(int number, string[] SITs, string ST, string[] urls) {
		this.numberOfSubmenuItems = number;
		this.submenuItemTitles = SITs;
		this.submenuTitle = ST;
		this.songURLS = urls;
	}
}
