using UnityEngine;
using WindowsInput;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class YouTubeController : MonoBehaviour {

	public bool locked = false;
	/*
	[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
	public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);
	
	private const int MOUSEEVENTF_LEFTDOWN = 0x02;
	private const int MOUSEEVENTF_LEFTUP = 0x04;
	private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	private const int MOUSEEVENTF_RIGHTUP = 0x10;

	public void DoMouseClick() {
		mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (long)Input.mousePosition.x, (long)Input.mousePosition.y, 0, 0);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (delayedClick ());
	}
	*/
	// Update is called once per frame
	void Update () {
		if (!locked) {
			if (Input.GetAxisRaw ("Horizontal") == 1) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.RIGHT);
				StartCoroutine(takeOne());
			} else if (Input.GetAxisRaw ("Horizontal") == -1)
				InputSimulator.SimulateKeyPress (VirtualKeyCode.LEFT);
				StartCoroutine(takeOne());
			if (Input.GetAxisRaw ("Vertical") == 1) {
				InputSimulator.SimulateKeyPress (VirtualKeyCode.UP);
				StartCoroutine(takeOne());

			} else if (Input.GetAxisRaw ("Vertical") == -1)
				InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
				StartCoroutine(takeOne());

			if(Input.GetButtonUp("Fire1")) {
				InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);


				//InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
				StartCoroutine(takeOne());

			}
			if(Input.GetButtonDown("Fire2"))
				Application.LoadLevel("mainMenue");
			if(Input.GetButtonUp("Fire3")) {
				InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_S);
				StartCoroutine(takeOne());

			}
		}

	}

	public IEnumerator takeOne() {
		locked = true;
		yield return new WaitForSeconds (.2f);
		locked = false;
	}
	/*
	public IEnumerator delayedClick() {
		yield return new WaitForSeconds (2);
		DoMouseClick ();
	}
	*/

}
