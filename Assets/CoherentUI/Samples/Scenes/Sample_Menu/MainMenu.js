#pragma strict

@SerializeField
var quitMenu: CoherentUIMenu;

function NewGame() {
	Debug.Log("New Game Clicked");
	Application.LoadLevelAsync("game");
}

function Quit() {
	var mainMenu = GetComponent(typeof CoherentUIMenu) as CoherentUIMenu;
	mainMenu.Hide();
	quitMenu.Show();
}
