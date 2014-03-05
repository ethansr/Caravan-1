using UnityEngine;
using System.Collections;

public class InvaderPlacement : DropLocation
{
		public GameObject invaderToken;
		public static float invaderTokenXPos;
		public Sprite tokenSprite;
		// Use this for initialization
		void Start ()
		{
				allowMultipleOccupants = false;
				invaderTokenXPos = MagicCarpetLocation.magicCarpetTokenXPos - 5;
	
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{
				if (isMeepleOrNull (potentialOccupant)) {
						bool alreadyHasPower = (potentialOccupant && potentialOccupant.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().canInvade);
		
						return (base.CanOccupy (potentialOccupant) && !alreadyHasPower);
				}
				return false;

		}

		public override void SetOccupant (GameObject o)
		{
				base.SetOccupant (o);
				if (o) {
						o.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().canInvade = true;
						passPlayerInvaderToken (o);
						//for testing quickly
						//DesertMovementController.playerWithMagicCarpet = o.GetComponent<Meeple> ().player;
						GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();

				}


		
		}

		void passPlayerInvaderToken (GameObject occupantMeeple)
		{
				GameObject newInvaderToken = (GameObject)Instantiate (invaderToken, transform.position, Quaternion.identity);
				newInvaderToken.GetComponent<SpriteRenderer> ().sprite = tokenSprite;
				occupantMeeple.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().playersInvaderToken = newInvaderToken;
				newInvaderToken.transform.position = occupantMeeple.transform.position;
				Vector3 newPos = occupantMeeple.GetComponent<Meeple> ().player.GetComponent<Player> ().meepleSource.transform.position;
				newPos.x = invaderTokenXPos;
				newPos.y -= MagicCarpetLocation.tokenYPosJitter;
		
				iTween.MoveTo (newInvaderToken, newPos, 2.0f);

		}


}
