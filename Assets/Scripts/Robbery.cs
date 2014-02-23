using UnityEngine;
using System.Collections;

public class Robbery : Event
{

		public const int numbersThatLoseWater = 3;
		public const int waterStolen = -1;
		const string foundRobberMessage = "Robbers attack! Will they triumph???";
		string wasRobbedMessage = "Robbers steal " + waterStolen + " water!!!";
		const string escapedRobbersMessage = "You fight them off... this time.";
		bool wasRobbed = false;
	
		public override void activateEvent (GameObject desertExplorer)
		{
		
				wasRobbed = drawFromBagOfChance (numbersThatLoseWater);
				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				explorer = desertExplorer;
		
		}
	
		void Update ()
		{      
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (wasRobbed, foundRobberMessage, wasRobbedMessage, escapedRobbersMessage);

				} else if (inControlOfTextBox)
						disableEventTextBox ();
		inControlOfTextBox=false;
		
		
		}
	
		protected override void takeEffect ()
		{
				explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWater (waterStolen);
		}
}
