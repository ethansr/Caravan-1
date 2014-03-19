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

		public bool prepareForInvasion ()
		{
				bool willInvade = entersFromInvadingTile ();
				if (willInvade)
						tileFromWhichInvaderAttacks = gameObject.GetComponent<DesertExplorer> ().currentTile;
				return willInvade;

		}
		/*
		public override void activateEvent (GameObject desertExplorer)
		{
				
				initializeEvent ();
				explorer = desertExplorer;

		}

	public override void activateEvent ()
	{
	}
	*/

		public override void activateEvent (GameObject desertExplorer)
		{
	
				explorer = desertExplorer;
				EventManager.addEventToQueue (gameObject.GetComponent<Event> ());
		
		}
	
		public override void activateEvent ()
		{
				name = "invade";
				eventMessage = name + ",";
				initializeEvent ();
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
				string player = explorer.GetComponent<Meeple> ().player.GetComponent<Player> ().id;
				eventMessage = eventMessage + player + ",";

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
						recordEventToLog ();
						closeEvent ();
						moveVictimToInvadersOldTile ();
						
				}
	

		}

		void moveVictimToInvadersOldTile ()
		{
				explorer.GetComponent<DesertExplorer> ().updateLocation (tileFromWhichInvaderAttacks);
		}
}
