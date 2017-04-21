#pragma strict

function Start() {
	var view = GetComponent(typeof CoherentUIView) as CoherentUIView;
	view.ReceivesInput = true;
}
