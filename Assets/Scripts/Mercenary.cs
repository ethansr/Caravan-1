using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Mercenary : Event
{
		public GameObject mercenaryPrefab;
		GameObject mercenary = null;
		DesertGenerator.GoodItem desiredGood;
		DesertGenerator.GoodType desiredGoodType;
		int requiredNumberOfGood = 1;
		bool mercenaryCanBeHired = false;
		string price;
		string foundMercenaryMessage = "You have stumbled on the mercenaries' basecamp...";
		string canHireMessage;
		string insufficientFundsMessage;
		string noRoomMessage = "there is no room for me here.";
		string cannotHireMessage;
		bool showHireSelectionButtons = false;
		bool showGoodSelectionButtons = false;
		bool mercenaryHasBeenInitialized = false;
		Collection<DesertGenerator.GoodItem> goodsPlayerCanPay;

		void initializeMercenary ()
		{
				mercenary = (GameObject)GameObject.Instantiate (mercenaryPrefab);
				
				mercenary.GetComponent<Draggable> ().enabled = false;
				
				mercenary.GetComponent<SpriteRenderer> ().enabled = false;

				mercenary.GetComponent<MercenaryExplorer> ().sourceEvent = gameObject;
		        
		        mercenary.GetComponent<Meeple>().id = "mercenary " + mercenary.GetHashCode ();
				
		}
		

		/*
	public override void activateEvent (GameObject desertExplorer)
	{
		if (!mercenaryHasBeenInitialized) {
			pickDesiredGoodTypeGivenCurrentLocation (desertTileWhereLocated);
			initializeMercenary ();
			reActivateEvent (desertExplorer);
			
		} 
		
	}
	
	public override void activateEvent ()
	{
	}
	*/

		public override void activateEvent (GameObject desertExplorer)
		{
				if (!mercenaryHasBeenInitialized) {
						pickDesiredGoodTypeGivenCurrentLocation (desertTileWhereLocated);
						initializeMercenary ();
						reActivateEvent (desertExplorer);
			
				} 
		
		}
	
		public override void activateEvent ()
		{
				initializeEvent ();
		}

		void Update ()
		{       
				price = requiredNumberOfGood + " " + desiredGoodType.ToString ();
				canHireMessage = "You may hire the mercenary for " + price + System.Environment.NewLine + ". Do you wish to?";
				insufficientFundsMessage = "you cannot pay my price of " + price;
				cannotHireMessage = "Either " + insufficientFundsMessage + System.Environment.NewLine + " or " + noRoomMessage;
				
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (mercenaryCanBeHired, foundMercenaryMessage, canHireMessage, cannotHireMessage);
				} else if (!showingButtons ()) { 
						if (inControlOfTextBox) {
								recordEventToLog ();
								closeEvent ();
							
						}
		
				}
		}

		bool showingButtons ()
		{
				return (showHireSelectionButtons || showGoodSelectionButtons);
		}
	
		bool roomAtTileWhereLocated ()
		{
				
				return desertTileWhereLocated.GetComponent<DesertTile> ().roomForMoreOccupants ();
		}

		bool checkIfPlayerHasSufficientFunds (GameObject player)
		{
				
				return player.GetComponent<PlayerInventory> ().hasGoodsOfGivenType (desiredGoodType, requiredNumberOfGood);
		}

		protected override void takeEffect ()
		{
				showHireSelectionButtons = true;
				
			
		}

		void OnGUI ()
		{
				if (showHireSelectionButtons || showGoodSelectionButtons) {
						
						float buttonWidth = 80;
						float buttonHeight = 30;
						float buttonStartX = 500;
						float buttonY = 450;

						if (showHireSelectionButtons) {
						
								if (GUI.Button (new Rect (buttonStartX, buttonY, buttonWidth, buttonHeight), "YES")) {
							
										showHireSelectionButtons = false;
										showGoodSelectionButtons = true;

								}
			
								if (GUI.Button (new Rect (buttonStartX + buttonWidth * 2, buttonY, buttonWidth, buttonHeight), "NO")) {
										//do nothing
										showHireSelectionButtons = false;
										eventMessage = name + "," + desiredGoodType + ",no";
									
					
								}
			
						} else if (showGoodSelectionButtons) {
								int xAdjFactor = 0;
								float xAdj = buttonWidth * 1.5f;
								foreach (DesertGenerator.GoodItem item in goodsPlayerCanPay) {
										string itemName = item.ToString ();
										if (GUI.Button (new Rect (buttonStartX + (xAdj * xAdjFactor), buttonY, buttonWidth, buttonHeight), itemName)) {
												hireMercenary (item);
												showGoodSelectionButtons = false;
						                        
												
										}
										xAdjFactor++;
								}
			       
						}

				} 
		
		
		}
	
	

		//requires that current explorer is the one who stumbled on the mercenary.
		void hireMercenary (DesertGenerator.GoodItem payment)
		{
				eventMessage = name + "," + desiredGoodType + "," + payment+",";
				GameObject newPlayer = explorer.GetComponent<Meeple> ().player;
				newPlayer.GetComponent<PlayerInventory> ().removeGoods (payment, requiredNumberOfGood);
				if (firstTimeHired ()) {
						setupMercenaryForExploration ();
						mercenaryHasBeenInitialized = true;
						eventMessage = eventMessage + "first_hire,";
				}
				assignToNewPlayer (newPlayer);

		}

		void setupMercenaryForExploration ()
		{
				mercenary.GetComponent<SpriteRenderer> ().enabled = true;
				
				mercenary.GetComponent<Meeple> ().makeExplorer (desertTileWhereLocated);
		
		}

		void assignToNewPlayer (GameObject newPlayer)
		{
				removeSelfFromOldPlayer ();
				assumeNewColor (newPlayer);
				mercenary.GetComponent<DesertExplorer> ().hasMovedThisRound = false;
				mercenary.GetComponent<DesertExplorer> ().clearGoodsAndEventsRecords ();
				newPlayer.GetComponent<Player> ().changeMovebleDesertExplorers (1);
		}

		void removeSelfFromOldPlayer ()
		{
				GameObject oldPlayer = mercenary.GetComponent<Meeple> ().player;
			
				bool hasMovedThisTurnForOldPlayer = mercenary.GetComponent<DesertExplorer> ().hasMovedThisRound;
				//if the mercenary has alreayd moved for this player, then num oveables was already decrementd		
				if (oldPlayer && !hasMovedThisTurnForOldPlayer) {
						oldPlayer.GetComponent<Player> ().changeMovebleDesertExplorers (-1);
						eventMessage = eventMessage + oldPlayer.GetComponent<Player> ().id + ",";
				}

		}

		void assumeNewColor (GameObject newPlayer)
		{
				mercenary.GetComponent<Meeple> ().player = newPlayer;
				Color playersColor = newPlayer.GetComponent<Player> ().meepleSource.GetComponent<SpriteRenderer> ().color;
				playersColor = darken (playersColor);
				mercenary.GetComponent<SpriteRenderer> ().color = playersColor;
				mercenary.GetComponent<DesertExplorer> ().defaultColor = playersColor;

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

		//the MercenaryExplorerScript calls this method subsequent times it is reaxctviated
		public void pickDesiredGoodTypeGivenCurrentLocation (GameObject currentLocation)
		{      
				DesertTile.RelativePosition mercPos = currentLocation.GetComponent<DesertTile> ().rp;
				desiredGoodType = DesertGenerator.getGoodTypeGivenLocation (mercPos.x, mercPos.y);
		     
		}
		//assume that whenever this event has been called the new desired good type has been updated
		/*
		public void reActivateEvent (GameObject desertExplorer)
		{
				initializeEvent ();
				mercenaryCanBeHired = roomAtTileWhereLocated () && checkIfPlayerHasSufficientFunds (desertExplorer.GetComponent<Meeple> ().player);
				if (mercenaryCanBeHired)
						getGoodItemsPlayerCanPay (desertExplorer.GetComponent<Meeple> ().player);		
			
				explorer = desertExplorer;
		
		}
		*/

		public void reActivateEvent (GameObject desertExplorer)
		{
				name = "mercenary";
				mercenaryCanBeHired = roomAtTileWhereLocated () && checkIfPlayerHasSufficientFunds (desertExplorer.GetComponent<Meeple> ().player);
				if (mercenaryCanBeHired)
						getGoodItemsPlayerCanPay (desertExplorer.GetComponent<Meeple> ().player);		
		
				explorer = desertExplorer;
				EventManager.addEventToQueue (gameObject.GetComponent<Event> ());
		
		}

		void getGoodItemsPlayerCanPay (GameObject player)
		{
				goodsPlayerCanPay = player.GetComponent<PlayerInventory> ().getAllGoodItemsOfType (desiredGoodType);

		}	
	
}
