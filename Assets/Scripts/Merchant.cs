using UnityEngine;
using System.Collections;

public class Merchant : Event
{

		string foundMerchantMessage = "A merchant appears, and asks" + System.Environment.NewLine + "that you satisfy his demand...";
		string partTwo = "Do you wish to take this merchant?";
		bool showingButtons = false;

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
				showingButtons = true;
		}

		void Update ()
		{       


				if (effectOccurring) {
						displayResultOfTwoCaseEvent (true, foundMerchantMessage, partTwo, "");
				} else if (!showingButtons) { 
						if (inControlOfTextBox) {
								disableEventTextBox ();
								inControlOfTextBox = false;
								anEventIsHappeningInGeneral = false;
								//tellPlayerToFinishEndTurn ();
						}
			
				}
			
		}

		void OnGUI ()
		{
				if (showingButtons) {
			
						float buttonWidth = 80;
						float buttonHeight = 30;
						float buttonStartX = 500;
						float buttonY = 450;
			
						if (showingButtons) {
				
								if (GUI.Button (new Rect (buttonStartX, buttonY, buttonWidth, buttonHeight), "YES")) {
					
										showingButtons = false;
										givePlayerNewMerchantCard ();
				
					
								}
				
								if (GUI.Button (new Rect (buttonStartX + buttonWidth * 2, buttonY, buttonWidth, buttonHeight), "NO")) {
										//do nothing
										showingButtons = false;
					
					
								}
				
						} 
			
		 
				}
		
		}

		void givePlayerNewMerchantCard ()
		{
				GameController controller = GameObject.Find ("GameController").GetComponent<GameController> ();
				controller.AssignCardToPlayer (explorer.GetComponent<Meeple> ().player);
		}
	
		

}
