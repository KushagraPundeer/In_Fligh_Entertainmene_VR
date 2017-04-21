using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SimpleJSON;
//using System.Threading.Tasks;

public class ClientObject : MonoBehaviour {
	public string flightID;
	public TextMesh textPrefab;
	public FIStatic flight;
	public FIDynamic flight2;
	public PlaneXMLv1 PlaneXML;
	public NetworkCredential c;

	public ClientObject(string fNum) {
		this.flightID = fNum;
	}
	// Use this for initialization

	public void Start() {
		StartCoroutine (Starts());
	}

	public IEnumerator Starts () {
		PlaneXML = new PlaneXMLv1 ();
		c = new NetworkCredential("kushagra.pundeer@gmail.com", "B8955E7C-C03F-4CF9-9B8D-C38C50FDA67A");
		PlaneXML.Credentials = c;
		flight = PlaneXML.FlightInfo(flightID, true, true);
		print ("OK");
		while(true) {
			StartCoroutine("refresh");
			yield return new WaitForSeconds(2);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void refresh() {
		flight2 = PlaneXML.FlightStatus(flightID);
		GameObject.Find ("card1").transform.FindChild ("value").GetComponent<TextMesh> ().text = "\n" + flight2.GS + "kt";
		GameObject.Find ("card2").transform.FindChild ("latVal").GetComponent<TextMesh> ().text = "Lat: " + Math.Round((Decimal)flight2.Lat, 4);
		GameObject.Find ("card2").transform.FindChild ("longVal").GetComponent<TextMesh> ().text = "Lon: " + Math.Round((Decimal)flight2.Lon, 4);
		GameObject.Find ("card3").transform.FindChild ("value").GetComponent<TextMesh> ().text = "\n20° C";
		GameObject.Find ("card4").transform.FindChild ("value").GetComponent<TextMesh> ().text = "\n" + flight2.Alt + "ft";
		GameObject.Find ("card5").transform.FindChild ("value").GetComponent<TextMesh> ().text = "\n" + flight2.ArrivalMins + "min";
	}




}