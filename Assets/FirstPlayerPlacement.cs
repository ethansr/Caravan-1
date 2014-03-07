using UnityEngine;
using System.Collections;

public class FirstPlayerPlacement : DropLocation {
	GameController controller;
	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	protected override bool  CanOccupy (GameObject potentialOccupant)
	{
		Meeple meepleComponent = potentialOccupant.GetComponent<Meeple> ();
		return meepleComponent ? true : false;
	}


	public override void SetOccupant (GameObject o)
	{ 
		base.SetOccupant (o);
		if (o) {
			Meeple meeple = o.GetComponent <Meeple> ();
			controller.MakeFirstPlayer(meeple.player);
			controller.LogEvent("first_player_changed");
			controller.getNextPlayer();
		}
	}
}
