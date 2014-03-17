using UnityEngine;
using System.Collections;

public class Merchant : Event
{

		string foundMerchantMessage = "A merchant appears, and asks" + System.Environment.NewLine + "that you satisfy his demand...";
		string partTwo = "Do you wish to take this merchant?";
		bool showingButtons = false;
	   
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
		
				EventManager.addEventToQueue (gameObject.GetComponent<Event> ());
				explorer = desertExplorer;
		
		
		}
	
		public override void activateEvent ()
		{
				name = "merchant";
				initializeEvent ();
				eventMessage = name + ",";
		
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
								closeEvent ();
								
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
										eventMessage = eventMessage + "no";
										recordEventToLog ();
					
					
								}
				
						} 
			
		 
				}
		
		}

		void givePlayerNewMerchantCard ()
		{
				eventMessage = eventMessage + "yes,";
				GameController controller = GameObject.Find ("GameController").GetComponent<GameController> ();
				controller.AssignCardToPlayer (explorer.GetComponent<Meeple> ().player, eventMessage);

		}
	
		

}
