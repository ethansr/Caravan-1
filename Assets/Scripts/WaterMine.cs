using UnityEngine;
using System.Collections;

public class WaterMine : DropLocation {

	public override void SetOccupant (GameObject o)
	{ 
		base.SetOccupant (o);
		if (o) {
						Meeple meeple = o.GetComponent <Meeple> ();
						GameObject player = meeple.player;
						PlayerInventory inventory = player.GetComponent<PlayerInventory> ();
						inventory.wellDepth += 1;
				}
	}
}
