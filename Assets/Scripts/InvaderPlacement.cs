using UnityEngine;
using System.Collections;

public class InvaderPlacement : DropLocation
{

		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
	
		}

		public override void SetOccupant (GameObject o)
		{
				base.SetOccupant (o);
				if (o) {
						o.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().canInvade = true;
						//for testing quickly
						//DesertMovementController.playerWithMagicCarpet = o.GetComponent<Meeple> ().player;
						GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();

				}


		
		}
}
