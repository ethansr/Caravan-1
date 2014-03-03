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
				
				initializeEvent ();
				getMoreWater = drawFromBagOfChance (numbersThatWinMoreWater);
		  
				explorer = desertExplorer;
				

		}

		void Update ()
		{      
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (getMoreWater, foundOasisMessage, isOasisMessage, isMirageMessage);
						
				} else if (inControlOfTextBox) {
						closeEvent ();
						
				}
				

		}

		protected override void takeEffect ()
		{      
				explorer.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWaterDuringMovement (waterGranted);

	
		}
		
	
	
	
	
}
