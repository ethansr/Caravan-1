using UnityEngine;
using System.Collections;

public class MagicCarpetLocation : DropLocation
{
		GameObject goodToTradeForCarpet;
		public GameObject magicCarpet;
		public static float magicCarpetTokenXPos = -70.0f;
	    

		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
				goodToTradeForCarpet = null;
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{       
				
				
				if (isMeepleOrNull (potentialOccupant)) {
						bool alreadyHasPower = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasMagicCarpetPower;
						bool playerHasGoods = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();
						return (base.CanOccupy (potentialOccupant) && playerHasGoods && !alreadyHasPower);
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
				occupantPlayer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().removeGoods (goodGiven, 1);
				passPlayerMagicCarpetToken (occupantPlayer);

				Object.Destroy (goodToTradeForCarpet);
		               
				
		}
	


		bool playerHasAnyGoods (GameObject meeple)
		{
				return meeple.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();

		}

		void passPlayerMagicCarpetToken (GameObject occupantPlayer)
		{
				GameObject magicCarpetToken = (GameObject)Instantiate (magicCarpet, transform.position, Quaternion.identity);
				occupantPlayer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().playersMagicCarpetToken = magicCarpetToken;
				magicCarpetToken.transform.position = occupantPlayer.transform.position;
				Vector3 newPos = occupantPlayer.GetComponent<Meeple> ().player.GetComponent<Player> ().meepleSource.transform.position;
				newPos.x = magicCarpetTokenXPos;
	
				iTween.MoveTo (magicCarpetToken, newPos, 2.0f);

		}
}
