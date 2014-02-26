using UnityEngine;
using System.Collections;

public class Invader : Event
{

		const string invaderMessagePartOne = "A skirmish breaks out!!!";
		string invaderMessagePartTwo = "This is our land now....";
		GameObject tileFromWhichInvaderAttacks;

		public void prepareForInvasion ()
		{
				tileFromWhichInvaderAttacks = gameObject.GetComponent<DesertExplorer> ().currentTile;

		}

		public override void activateEvent (GameObject desertExplorer)
		{
				anEventIsHappeningInGeneral = true;
				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				explorer = desertExplorer;

		}

		protected override void takeEffect ()
		{
				swapLocationsWithVictim ();
				removePlayersInvaderAbility ();
			
		        

		}

		void swapLocationsWithVictim ()
		{
				GameObject tileOfVictim = explorer.GetComponent<DesertExplorer> ().currentTile;
			
		
				explorer.GetComponent<DesertExplorer> ().updateLocation (tileFromWhichInvaderAttacks);
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
						disableEventTextBox ();
						inControlOfTextBox = false;
						anEventIsHappeningInGeneral = false;
						tellPlayerToFinishEndTurn ();
				}



		}
}
