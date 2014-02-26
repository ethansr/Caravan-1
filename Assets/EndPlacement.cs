using UnityEngine;
using System.Collections;

public class EndPlacement : MonoBehaviour {

	float doubleClickStart = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnMouseUp ()
	{
		if ((Time.time - doubleClickStart) < 0.3f) {
			this.OnDoubleClick ();
			doubleClickStart = -1;
		} else {
			doubleClickStart = Time.time;
		}
	}
	
	//in general double click is how a player ends his turn early (ie before running out of wter)
	void OnDoubleClick ()
	{      
		GameObject.Find ("GameController").GetComponent<GameController> ().EndPlacementPhase ();

	}

}
