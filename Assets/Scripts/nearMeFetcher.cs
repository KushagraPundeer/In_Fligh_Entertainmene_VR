using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

using SimpleJSON;
//using System.Threading.Tasks;

public class nearMeFetcher : MonoBehaviour {
	public string flightID;
	public WWW www;
	public FIStatic flight;
	public FIDynamic flight2;
	public PlaneXMLv1 PlaneXML;
	public NetworkCredential c;

	public string[] cardTitles;
	public string[] wikiTexts;


	public bool locked = false;
	public int midVisibleCard;


	public List<string> titles = new List<string>();
	public List<string> bodies = new List<string>();
	// Use this for initialization
	void Start () {
		PlaneXML = new PlaneXMLv1 ();
		c = new NetworkCredential("kushagra.pundeer@gmail.com", "B8955E7C-C03F-4CF9-9B8D-C38C50FDA67A");
		PlaneXML.Credentials = c;
		flight = PlaneXML.FlightInfo(flightID, true, true);
		StartCoroutine("refresh");
	}
	
	IEnumerator refresh() {
		flight2 = PlaneXML.FlightStatus(flightID);
		print("Latitude " + flight2.Lat);
		print("Longitude " + flight2.Lon);
		print("Altitude " + flight2.Alt);
		print("Arrival Mins. " + flight2.ArrivalMins);
		print("Ground Speed " + flight2.GS);
		float lat = (float)flight2.Lat;
		float lng = (float)flight2.Lon;
		float alt = (float)flight2.Alt * 0.3048f; //Feet to meters
		float radius = Mathf.Tan (60f * Mathf.Deg2Rad) * alt;
		//DEBUG
		//lat = 37.750179f;
		//lng = -122.462813f;
		//radius = 20000;
		//
		string url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=" + lat + "," + lng + "&radius=" + (int)radius + "&types=airport|aquarium|museum|casino|shopping_mall|stadium|zoo|university&key=AIzaSyC-CL4XpshMmVqpGnBiuL1GRe0PLinj-U0";
		www = new WWW (url);
		yield return www;
		string json = www.text;
		print (json);
		var N = JSON.Parse (json);

		int i = -1;
		int k = -1;
		
		while (N["results"][++k]["id"] != null) {
			;
		}
		int size = k;
		cardTitles = new string[k];
		wikiTexts = new string[k];

		while (N["results"][++i]["id"] != null) {
	
			cardTitles[i] = N["results"] [i] ["name"];
			WWW w = new WWW(("https://en.wikipedia.org/w/api.php?action=opensearch&search=" + cardTitles[i] + "&limit=1&namespace=0&format=json").Replace(" ", "%20"));
			yield return w;
			var temp = JSON.Parse(w.text);
			print (temp[1][0]);
			if(temp[0] != null)
				cardTitles[i] = temp[1][0];
			else {
				cardTitles[i] = null;
			}

			List<string> titleList = new List<string>(cardTitles);
			List<string> newTitleList = new List<string>();
			for(int a = 0; a < titleList.Count; a++) {
				if(titleList.ElementAt(a) != null)
					newTitleList.Add(titleList.ElementAt(a));
			}

					              
			WWW w2 = new WWW (("https://en.wikipedia.org/w/api.php?action=query&prop=extracts&exintro&explaintext&titles=" + newTitleList[i] + "&format=json").Replace(" ", "%20"));
			yield return w2;
			var temp2 = JSON.Parse(w2.text);
			if(temp2["query"]["pages"][0][3] != null) { 
				wikiTexts[i] = StripHTML(temp2["query"]["pages"][0][3], true);

				titles.Add(newTitleList.ElementAt(i));
				bodies.Add(wikiTexts[i]);
			}

			if (i > 100)
				break;
		}

		createCards ();

	}        

	public static string StripHTML(string HTMLText, bool decode = true)  {
		Regex reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
		var stripped = reg.Replace(HTMLText, "");
		return decode ? HttpUtility.HtmlDecode(stripped) : stripped;
	}


	GameObject[] cards;
	public GameObject cardPrefab;
	public GameObject[] pubCards;

	public void createCards() {
		int size = titles.Count;
		cards = new GameObject[size];

		for (int index = 0; index < size; index++) {
			Vector3 pos = Vector3.zero;
			if(size % 2 == 0)
				pos = new Vector3(9f * (size / 2 - (index + 1)) + 9f, -7f, 8f);
			else
				pos = new Vector3(9f * (size / 2 - index), -7f, 8f);

			cards[index] = (GameObject)Instantiate(cardPrefab, pos, cardPrefab.transform.rotation);
			cards[index] .transform.SetParent(GameObject.Find("dealer").transform);
			cards[index].transform.FindChild("titleText").GetComponent<TextMesh>().text = titles.ElementAt(index);
			cards[index].transform.FindChild("bodyText").GetComponent<TextMesh>().text = modifiedText(bodies.ElementAt(index));

			midVisibleCard = size/2;
		}
	}

	public string modifiedText(string text) {
		string newText = "";
		text = text.Replace ("\n", " ");
		for (int letter = 0; letter < text.Length; letter+=23) {
			try {
				newText += text.Substring(letter, 23);
				newText += "\n";
			} catch (Exception e) {
				;
			}
		}
		return newText;
	}
	
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
		
		GameObject.Find ("dealer").transform.position = new Vector3 (GameObject.Find ("dealer").transform.position.x + (dir * 1f), GameObject.Find ("dealer").transform.position.y, GameObject.Find ("dealer").transform.position.z);
	}
}





