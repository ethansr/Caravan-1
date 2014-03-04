using UnityEngine;
using System.Collections;

public class MagicCarpetLocation : DropLocation
{
		GameObject goodToTradeForCarpet;
	    

		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
				goodToTradeForCarpet = null;
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{       
				
				
				if (isMeepleOrNull (potentialOccupant)) {
						bool playerHasGoods = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();
						return (base.CanOccupy (potentialOccupant) && playerHasGoods);
				} else
						return (!goodToTradeForCarpet && potentialOccupant.GetComponent<GoodToken> ());
		}
	
		public override void SetOccupant (GameObject o)
		{
				
				if (isMeepleOrNull (o)) {
						base.SetOccupant (o);
							
				} else if (o) {
						goodToTradeForCarpet = o;
							
				}

				if (occupant && goodToTradeForCarpet) {
				           
						givePlayerMagicCarpetPowerForGood (occupant);
						GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
			
			
				}
			
		          

		}

		void givePlayerMagicCarpetPowerForGood (GameObject occupantPlayer)
		{      
				
				occupantPlayer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasMagicCarpetPower = true;
				DesertGenerator.GoodItem goodGiven = goodToTradeForCarpet.GetComponent<GoodToken> ().good;
				occupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().removeGoods (goodGiven, 1);
				Object.Destroy (goodToTradeForCarpet);
		               
		}
	
		bool isMeepleOrNull (GameObject potentialOccupant)
		{
				return (potentialOccupant && potentialOccupant.GetComponent<Meeple> ()||!potentialOccupant);
		}

		bool playerHasAnyGoods (GameObject meeple)
		{
				return meeple.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();

		}
}
