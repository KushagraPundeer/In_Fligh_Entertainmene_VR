using UnityEngine;

public class QuitMenu : MonoBehaviour {

	[SerializeField]
	private CoherentUIMenu MainMenu;

	public void DoQuit()
	{
		Debug.Log("Bye Bye");
		Application.Quit();
	}

	public void CancelQuit()
	{
		var menu = GetComponent<CoherentUIMenu>();
		menu.Hide();

		MainMenu.Show();
	}
}
