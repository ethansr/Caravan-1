using UnityEngine;
using System.Collections;

public class MagicCarpetLocation : DropLocation
{

		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
		
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{
				bool playerHasGoods = potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();
				Debug.Log (playerHasGoods);
				return (base.CanOccupy (potentialOccupant) && true);//playerHasGoods);
		}
	
		public override void SetOccupant (GameObject o)
		{
				base.SetOccupant (o);
				if (o) {
						MagicCarpet.playerWithMagicCarpet = o.GetComponent<Meeple> ().player;
						//DesertMovementController.playerWithMagicCarpet = o.GetComponent<Meeple> ().player;
			
				}
		
		
		
		}

		bool playerHasAnyGoods (GameObject meeple)
		{
				return meeple.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().hasGoods ();

		}
}
