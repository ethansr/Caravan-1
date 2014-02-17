using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Mercenary : Event
{
		public GameObject mercenaryPrefab;
		GameObject mercenary = null;
		DesertGenerator.GoodType desiredGood;
		int requiredNumberOfGood = 1;
		bool mercenaryCanBeHired = false;
		string price;
		string foundMercenaryMessage = "You have stumbled on the mercenaries' basecamp...";
		string canHireMessage;
		string insufficientFundsMessage;
		string noRoomMessage = "there is no room for me here.";
		string cannotHireMessage;
		bool showHireSelectionButtons = false;
		bool eventTileAlreadyActivated = false;

		void Start ()
		{
				
				pickDesiredGood ();

		}

		void initializeMercenary ()
		{
				mercenary = (GameObject)GameObject.Instantiate (mercenaryPrefab);
				
				mercenary.GetComponent<Draggable> ().enabled = false;
				
				mercenary.GetComponent<SpriteRenderer> ().enabled = false;

				mercenary.GetComponent<MercenaryExplorer> ().sourceEvent = gameObject;
				
		}
		
		public override void activateEvent (GameObject desertExplorer)
		{
				if (!eventTileAlreadyActivated) {
						initializeMercenary ();
						reActivateEvent (desertExplorer);
						eventTileAlreadyActivated = true;
				} 
			
		}

		void Update ()
		{       
				price = requiredNumberOfGood + " " + desiredGood.ToString ();
				canHireMessage = "You may hire the mercenary for " + price + ". Do you wish to?";
				insufficientFundsMessage = "you cannot pay my price of " + price;
				cannotHireMessage = "Either " + insufficientFundsMessage + " or " + noRoomMessage;
				

		if (effectOccurring) {
						displayResultOfTwoCaseEvent (mercenaryCanBeHired, foundMercenaryMessage, canHireMessage, cannotHireMessage);
				}
		               
			
		}
	
		bool roomAtTileWhereLocated ()
		{
				return true;
				//return desertTileWhereLocated.GetComponent<DesertTile> ().roomForMoreOccupants ();
		}

		bool checkIfPlayerHasSufficientFunds (GameObject player)
		{
				return true;
				//return player.GetComponent<PlayerInventory> ().hasGoods (desiredGood, requiredNumberOfGood);
		}

		protected override void takeEffect ()
		{
				showHireSelectionButtons = true;
		        
		}

		void OnGUI ()
		{       
				if (showHireSelectionButtons) {
						float buttonWidth = 80;
						float buttonHeight = 30;
						if (GUI.Button (new Rect (500, 500, buttonWidth, buttonHeight), "YES")) {
							
								hireMercenary ();
								showHireSelectionButtons = false;
						}
			
						if (GUI.Button (new Rect (500 + buttonWidth * 2, 500, buttonWidth, buttonHeight), "NO")) {
								//do nothing
								showHireSelectionButtons = false;
					
						}
			
				}
		
		}

		//requires that current explorer is the one who stumbled on the mercenary.
		void hireMercenary ()
		{
				GameObject newPlayer = explorer.GetComponent<Meeple> ().player;
				newPlayer.GetComponent<PlayerInventory> ().removeGoods (desiredGood, requiredNumberOfGood);
				if (firstTimeHired ())
						setupMercenaryForExploration ();
				assignToNewPlayer (newPlayer);

		}

		void setupMercenaryForExploration ()
		{
				mercenary.GetComponent<SpriteRenderer> ().enabled = true;
				
				mercenary.GetComponent<Meeple> ().makeExplorer (desertTileWhereLocated);
		
		}

		void assignToNewPlayer (GameObject newPlayer)
		{
				mercenary.GetComponent<Meeple> ().player = newPlayer;
				Color playersColor = newPlayer.GetComponent<Player> ().meepleSource.GetComponent<SpriteRenderer> ().color;
				playersColor = darken (playersColor);
				mercenary.GetComponent<SpriteRenderer> ().color = playersColor;

		}

		Color darken (Color color)
		{
				color.r /= 1.5f;
				color.g /= 1.5f;
				color.b /= 1.5f;
				return color;
		}

		bool firstTimeHired ()
		{
				return mercenary.GetComponent<DesertExplorer> ().enabled == false;
		}
	
		void pickDesiredGood ()
		{
				int randomGood = (int)UnityEngine.Random.Range (0, DesertGenerator.numGoods);
				desiredGood = (DesertGenerator.GoodType)Enum.ToObject (typeof(DesertGenerator.GoodType), randomGood);
				
		}

		public void reActivateEvent (GameObject desertExplorer)
		{
				mercenaryCanBeHired = roomAtTileWhereLocated () && checkIfPlayerHasSufficientFunds (desertExplorer.GetComponent<Meeple> ().player);
				effectOccurring = true;
				tookEffect = false;
				eventStartTime = Time.time;
				explorer = desertExplorer;
		
		}
	
	
}
