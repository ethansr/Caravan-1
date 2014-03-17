using UnityEngine;
using System.Collections;

public class Robbery : Event
{

		public const int numbersThatLoseWater = 3;
		public const int waterStolen = -1;
		const string foundRobberMessage = "Robbers attack! Will they triumph???";
		string wasRobbedMessage;
		string tookWaterMessage = "Robbers steal " + (-1) * waterStolen + " water!!!";
		string missNextTurnMessage = "We broke this explorer's legs. He cannot move next turn.";
		const string escapedRobbersMessage = "You fight them off... this time.";
		bool wasRobbed = false;
		bool hadEnoughWaterToTake;

		/*
		public override void activateEvent (GameObject desertExplorer)
		{
				
		        initializeEvent ();
				wasRobbed = drawFromBagOfChance (numbersThatLoseWater);
	
				explorer = desertExplorer;
				hadEnoughWaterToTake = (explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().howMuchWaterAvailable () + waterStolen > -1);
				wasRobbedMessage = (hadEnoughWaterToTake ? tookWaterMessage : missNextTurnMessage);


		
		}

	public override void activateEvent ()
	{
	}
	*/

		public override void activateEvent (GameObject desertExplorer)
		{
				name = "robber";
				explorer = desertExplorer;
				EventManager.addEventToQueue (gameObject.GetComponent<Event> ());
	
		
		}
	
		public override void activateEvent ()
		{
				initializeEvent ();
				wasRobbed = drawFromBagOfChance (numbersThatLoseWater);
		
		
				hadEnoughWaterToTake = (explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().howMuchWaterAvailable () + waterStolen > -1);
				wasRobbedMessage = (hadEnoughWaterToTake ? tookWaterMessage : missNextTurnMessage);
				eventMessage = name + (wasRobbed ? " rob successful " : " escaped ") + " updated water " + (explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().availableWater + (wasRobbed ? waterStolen : 0));

		}
	
		void Update ()
		{      
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (wasRobbed, foundRobberMessage, wasRobbedMessage, escapedRobbersMessage);

				} else if (inControlOfTextBox) {
						recordEventToLog ();
						closeEvent ();
			           
					
				}
		
		
		}
	
		protected override void takeEffect ()
		{
				if (hadEnoughWaterToTake)
						explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWaterDuringMovement (waterStolen);
				else
						explorer.GetComponent<DesertExplorer> ().makeMissNextTurn ();
				
		}
}
