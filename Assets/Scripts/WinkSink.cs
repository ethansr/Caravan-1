using UnityEngine;
using System.Collections;

public class WinkSink : DropLocation {
	
	public override void SetOccupant (GameObject o)
	{ 
				base.SetOccupant (o);
				if (o) {
						Meeple meeple = o.GetComponent <Meeple> ();
						GameObject player = meeple.player;
						PlayerInventory inventory = player.GetComponent<PlayerInventory> ();
						inventory.availableWater = inventory.availableWater + 4;

						GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();

				}
		}

}
