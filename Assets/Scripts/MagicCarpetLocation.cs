using UnityEngine;
using System.Collections;

public class MagicCarpetLocation : DropLocation
{
		GameObject goodToTradeForCarpet;

		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{       
				bool playerHasGoods = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();
				
				if (isMeeple (potentialOccupant))
						return (base.CanOccupy (potentialOccupant) && playerHasGoods);
				else
						return (!goodToTradeForCarpet && potentialOccupant.CompareTag ("goodToken"));
		}
	
		public override void SetOccupant (GameObject o)
		{
				if (isMeeple (o))
						base.SetOccupant (o);
				else
						goodToTradeForCarpet = o;

				if (occupant && goodToTradeForCarpet) {
						MagicCarpet.playerWithMagicCarpet = o.GetComponent<Meeple> ().player;
						//goodToTradeForCarpet.DestroyFOREVER!!!!!
						GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
			
			
				}
		
		
		
		}

		bool isMeeple (GameObject potentialOccupant)
		{
				return potentialOccupant.GetComponent<Meeple> ();
		}

		bool playerHasAnyGoods (GameObject meeple)
		{
				return meeple.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();

		}
}
