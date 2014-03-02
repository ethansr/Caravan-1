using UnityEngine;
using System.Collections;

public class PublicWell : DropLocation {

	// Use this for initialization
	void Start () {
	
	}
	
	public override void SetOccupant (GameObject o)
	{ 
		base.SetOccupant (o);
		if (o){
		Meeple meeple = o.GetComponent <Meeple>();
		print (meeple);
		GameObject player = meeple.player;
		print (player);
		PlayerInventory inventory = player.GetComponent<PlayerInventory> ();
		print (inventory);
		inventory.availableWater = inventory.availableWater + 1;
		print ("Boom");

		GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
		
	}

	}

}
