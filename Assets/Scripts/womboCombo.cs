using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SimpleJSON;

public class womboCombo : MonoBehaviour {
	public string flightID;
	public TextMesh textPrefab;
	public FIStatic flight;
	public FIDynamic flight2;
	public PlaneXMLv1 PlaneXML;
	public NetworkCredential c;

	public enum MapType {
		RoadMap,
		Satellite,
		Terrain,
		Hybrid
	}
	public bool loadOnStart = true;
	public bool autoLocateCenter = true;
	public GoogleMapLocation centerLocation;
	public int zoom = 13;
	public MapType mapType;
	public int size = 4096;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;

	public womboCombo(string fNum) {
		this.flightID = fNum;
	}
	// Use this for initialization
	
	public IEnumerator Start () {

		if(loadOnStart) Refresh();	

		PlaneXML = new PlaneXMLv1 ();
		c = new NetworkCredential("kushagra.pundeer@gmail.com", "B8955E7C-C03F-4CF9-9B8D-C38C50FDA67A");
		PlaneXML.Credentials = c;
		flight = PlaneXML.FlightInfo(flightID, true, true);
		while(true) {
			StartCoroutine("refresh");
			yield return new WaitForSeconds(2);
		}
	}
	
	void Update() {
		checkIfCoordsChanged ();
	}

	public void checkIfCoordsChanged() {
		if ((flight2.Lat != null && flight2.Lon != null) && (Decimal.Round((decimal)flight2.Lat, 4) 
		     != Decimal.Round((decimal)centerLocation.latitude, 4) || Decimal.Round((decimal)flight2.Lon, 4) != Decimal.Round((decimal)centerLocation.longitude, 4))) {
			grabNewGPSCoords();
			Refresh ();
		}
	}
	
	public void grabNewGPSCoords() {
		centerLocation.latitude = (float)flight2.Lat;//60.18626f; // new lat
		centerLocation.longitude = (float)flight2.Lon; //24.97072f; // new long
	}
		
	public void Refresh() {
		StartCoroutine(_Refresh());
	}
	
	IEnumerator _Refresh ()
	{
		var url = "http://maps.googleapis.com/maps/api/staticmap";
		var qs = "";
		if (!autoLocateCenter) {
			if (centerLocation.address != "")
				qs += "center=" + centerLocation.address;
			else {
				qs += "center=" + string.Format ("{0},{1}", centerLocation.latitude, centerLocation.longitude);
			}
			
			qs += "&zoom=" + zoom.ToString ();
		}
		qs += "&size=" + string.Format ("{0}x{0}", size);
		qs += "&scale=" + (doubleResolution ? "2" : "1");
		qs += "&maptype=" + mapType.ToString ().ToLower ();
		var usingSensor = false;

		qs += "&sensor=" + (usingSensor ? "true" : "false");
		
		foreach (var i in markers) {
			qs += "&markers=" + string.Format ("size:{0}|color:{1}|label:{2}", i.size.ToString ().ToLower (), i.color, i.label);
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + loc.address;
				else
					qs += "|" + string.Format ("{0},{1}", loc.latitude, loc.longitude);
			}
		}
		
		foreach (var i in paths) {
			qs += "&path=" + string.Format ("weight:{0}|color:{1}", i.weight, i.color);
			if(i.fill) qs += "|fillcolor:" + i.fillColor;
			foreach (var loc in i.locations) {
				if (loc.address != "")
					qs += "|" + loc.address;
				else
					qs += "|" + string.Format ("{0},{1}", loc.latitude, loc.longitude);
			}
		}
		
		
		var req = new WWW (url + "?" + qs);
		Debug.Log (url + "?" + qs);
		while (!req.isDone)
			yield return null;
		if (req.error == null) {
			var tex = new Texture2D (size, size);
			tex.LoadImage (req.bytes);
			GetComponent<Renderer>().material.mainTexture = tex;
		}
	}

	
	void refresh() {
		flight2 = PlaneXML.FlightStatus(flightID);
	}	
}

public enum GoogleMapColor
{
	black,
	brown,
	green,
	purple,
	yellow,
	blue,
	gray,
	orange,
	red,
	white
}

[System.Serializable]
public class GoogleMapLocation
{
	public string address;
	public float latitude;
	public float longitude;
}

[System.Serializable]
public class GoogleMapMarker
{
	public enum GoogleMapMarkerSize
	{
		Tiny,
		Small,
		Mid
	}
	public GoogleMapMarkerSize size;
	public GoogleMapColor color;
	public string label;
	public GoogleMapLocation[] locations;
	
}

[System.Serializable]
public class GoogleMapPath
{
	public int weight = 5;
	public GoogleMapColor color;
	public bool fill = false;
	public GoogleMapColor fillColor;
	public GoogleMapLocation[] locations;	
}