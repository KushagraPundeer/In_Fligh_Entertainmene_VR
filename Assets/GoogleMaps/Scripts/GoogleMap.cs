/*using UnityEngine;
using System.Collections;
using System;
public class GoogleMap : MonoBehaviour {
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
	public int size = 512;
	public bool doubleResolution = false;
	public GoogleMapMarker[] markers;
	public GoogleMapPath[] paths;

	public GameObject mainCam;
	public ClientObject co;
	
	void Start() {
		co = new ClientObject ("DAL80");
		co.Start();
		grabNewGPSCoords ();
		mainCam.GetComponent<Rigidbody> ().velocity = new Vector3 (0, .75f, 0);
		if(loadOnStart) Refresh();	
	}

	void Update() {
		checkIfNeedToRespawn ();
		print (co.flight2.Lat);
	}

	public void checkIfNeedToRespawn() {
		if (Math.Pow (mainCam.transform.position.x, 2) + Math.Pow (mainCam.transform.position.y, 2) > 22*22) 
			respawnCamera ();
	}

	public void respawnCamera() {
		mainCam.transform.position = new Vector3 (0, 12f, 12f);

		grabNewGPSCoords ();

		Refresh ();
	}


	public void grabNewGPSCoords() {
		try {
			Debug.Log(co.flight2.Lat + "asdfad");
			centerLocation.latitude = (float)co.flight2.Lat;//60.18626f; // new lat
			centerLocation.longitude = (float)co.flight2.Lon; //24.97072f; // new long
		} catch(Exception e) {
			StartCoroutine(redoIt());
		}
	}

	public IEnumerator redoIt() {
		yield return new WaitForSeconds (1);
		grabNewGPSCoords ();
		
		Refresh ();
	}
	
	public void Refresh() {
		if(autoLocateCenter && (markers.Length == 0 && paths.Length == 0)) {
			Debug.LogError("Auto Center will only work if paths or markers are used.");	
		}
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
#if UNITY_IPHONE
		usingSensor = Input.location.isEnabledByUser && Input.location.status == LocationServiceStatus.Running;
#endif
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
}*/