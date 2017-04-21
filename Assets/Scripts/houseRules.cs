

using UnityEngine;
using System.Collections;

// Get the latest webcam shot from outside "Friday's" in Times Square
public class houseRules : MonoBehaviour {
	public string url = "https://maps.gstatic.com/mapfiles/place_api/icons/airport-71.png";
	
	IEnumerator Start() {
		// Start a download of the given URL
		WWW www = new WWW(url);
		
		// Wait for download to complete
		yield return www;
		
		// assign texture
		GameObject.Find("iceCube").GetComponent<Renderer>().material.mainTexture = www.texture;
	}
}
