using UnityEngine;
using System.Collections;

public class Shuffle : DropLocation {
	public override void SetOccupant (GameObject o)
	{ 
	
		base.SetOccupant (o);
		if (o) {
			GameObject.Find("GameController").GetComponent<GameController>().ReturnPublicCardsToDeck ();
			GameObject.Find("GameController").GetComponent<GameController>().ShuffleDeck ();

			GameObject.Find("GameController").GetComponent<GameController>().DealPublicCards ();
			GameObject.Find("GameController").GetComponent<GameController>().getNextPlayer();

		}
	}

}
