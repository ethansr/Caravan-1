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
				
				
				if (isMeeple (potentialOccupant)) {
						bool playerHasGoods = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();
						return (base.CanOccupy (potentialOccupant) && playerHasGoods);
				} else
						return (!goodToTradeForCarpet && potentialOccupant.GetComponent<GoodToken> ());
		}
	
		public override void SetOccupant (GameObject o)
		{
				if (o) {
						if (isMeeple (o)) {
								base.SetOccupant (o);
								Debug.Log ("meeple");
						} else {
								goodToTradeForCarpet = o;
								Debug.Log ("good ");
		
						}

						if (occupant && goodToTradeForCarpet) {
								givePlayerMagicCarpetPowerForGood (occupant);
								GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
			
			
						}
				}

		}

		void givePlayerMagicCarpetPowerForGood (GameObject occupantPlayer)
		{      
				MagicCarpet.playerWithMagicCarpet = occupantPlayer.GetComponent<Meeple> ().player;
				DesertGenerator.GoodItem goodGiven = goodToTradeForCarpet.GetComponent<GoodToken> ().good;
				occupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().removeGoods (goodGiven, 1);
				
				Object.Destroy (goodToTradeForCarpet);
		               
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
