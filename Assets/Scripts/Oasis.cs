using UnityEngine;
using System.Collections;

public class Oasis : Event
{
	
		public const int numbersThatWinMoreWater = 3;
		public const int waterGranted = 3;
		const string foundOasisMessage = "Is it an oasis, or a mirage?...";
		const string isOasisMessage = "It is an oasis! You earn more water.";
		const string isMirageMessage = "It is a mirage. You earn nothing...";
		bool getMoreWater = false;
	
		public override void activateEvent (GameObject desertExplorer)
		{
				//anEventIsHappeningInGeneral = true;
		initializeEvent ();
				getMoreWater = drawFromBagOfChance (numbersThatWinMoreWater);
		        /*
				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				*/
				explorer = desertExplorer;
				

		}

		void Update ()
		{      
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (getMoreWater, foundOasisMessage, isOasisMessage, isMirageMessage);
						
				} else if (inControlOfTextBox) {
						disableEventTextBox ();
						inControlOfTextBox = false;
						anEventIsHappeningInGeneral = false;
						//tellPlayerToFinishEndTurn();
				}
				

		}

		protected override void takeEffect ()
		{      
				explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWaterDuringMovement (waterGranted);

	
		}
		
	
	
	
	
}
