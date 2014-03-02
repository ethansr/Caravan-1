using UnityEngine;
using System.Collections;

public class Invader : Event
{

		const string invaderMessagePartOne = "A skirmish breaks out!!!";
		string invaderMessagePartTwo = "This is our land now....";
		GameObject tileFromWhichInvaderAttacks;

		public bool entersFromInvadingTile ()
		{

				return !gameObject.GetComponent<DesertExplorer> ().currentTile.GetComponent<DesertTile> ().isBazaar ();
		}

		public void prepareForInvasion ()
		{
				tileFromWhichInvaderAttacks = gameObject.GetComponent<DesertExplorer> ().currentTile;

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
