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
	
		public override void activateEvent (GameObject desertExplorer)
		{
				anEventIsHappeningInGeneral = true;
		
				wasRobbed = drawFromBagOfChance (numbersThatLoseWater);

				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				explorer = desertExplorer;
				hadEnoughWaterToTake = (explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().howMuchWaterAvailable () + waterStolen > -1);
				wasRobbedMessage = (hadEnoughWaterToTake ? tookWaterMessage : missNextTurnMessage);


		
		}
	
		void Update ()
		{      
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (wasRobbed, foundRobberMessage, wasRobbedMessage, escapedRobbersMessage);

				} else if (inControlOfTextBox) {
						disableEventTextBox ();
						inControlOfTextBox = false;
						anEventIsHappeningInGeneral = false;
						//tellPlayerToFinishEndTurn ();
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
