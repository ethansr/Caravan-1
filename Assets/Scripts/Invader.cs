using UnityEngine;
using System.Collections;

public class Invader : Event
{

		const string invaderMessagePartOne = "A skirmish breaks out!!!";
		string invaderMessagePartTwo = "This is our land now....";
		GameObject tileFromWhichInvaderAttacks;

		public bool entersFromInvadingTile ()
		{
				bool result = !gameObject.GetComponent<DesertExplorer> ().currentTile.GetComponent<DesertTile> ().isBazaar ();
				
				return !gameObject.GetComponent<DesertExplorer> ().currentTile.GetComponent<DesertTile> ().isBazaar ();
		}

		public bool prepareForInvasion ()
		{
				bool willInvade = entersFromInvadingTile ();
				if (willInvade)
						tileFromWhichInvaderAttacks = gameObject.GetComponent<DesertExplorer> ().currentTile;
				return willInvade;

		}

		public override void activateEvent (GameObject desertExplorer)
		{
				
				initializeEvent ();
				explorer = desertExplorer;

		}

		protected override void takeEffect ()
		{
				moveIntoVictimsTile ();
				removePlayersInvaderAbility ();
			
		        

		}

		void moveIntoVictimsTile ()
		{
				GameObject tileOfVictim = explorer.GetComponent<DesertExplorer> ().currentTile;
				gameObject.GetComponent<DesertExplorer> ().updateLocation (tileOfVictim);

		}

		void removePlayersInvaderAbility ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().canInvade = false;
				Object.Destroy (gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().playersInvaderToken);
		}

		void Update ()
		{
				if (effectOccurring) {

						displayResultOfTwoCaseEvent (true, invaderMessagePartOne, invaderMessagePartTwo, "");
			
				} else if (inControlOfTextBox) {
						closeEvent ();
						moveVictimToInvadersOldTile ();
						
				}
	

		}

		void moveVictimToInvadersOldTile ()
		{
				explorer.GetComponent<DesertExplorer> ().updateLocation (tileFromWhichInvaderAttacks);
		}
}
