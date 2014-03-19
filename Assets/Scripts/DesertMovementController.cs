using UnityEngine;
using System;
using System.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;

public class DesertMovementController : Event
{
		
		string movementEndedMessage = "We have finished exploring for now...";
		string movementEndPartTwoMessage = "Explorers in the bazaar have" + System.Environment.NewLine + " returned to their duties... Those in the desert" + System.Environment.NewLine + "are checking their supplies.";
		string needToTradeGoodsForMeepleMessage = " you must give up goods for an Explorer.";
		string partTwo = "Your explorers have chosen who to send," + System.Environment.NewLine + "and which good to sacrifice to the desert.";
		public static bool inMovementPhase = false;
		bool showingEndOfMovePhaseScreen = false;
		bool showingPlayerMustTradeGoodsForExplorerScreen = false;
		Collection<GameObject> playersWhoMustTradeGoodsForExplorer = new Collection<GameObject> ();
		float buttonWidth = 100;
		float buttonHeight = 30;
		float buttonStartX = 1200;
		float buttonY = 50;
		float sendToSourceDelayStart;
		bool waitingOnExplorerReturn = false;
		bool updatePlayerWaitingOnEvent = false;

		public void beginDesertMovementPhase ()
		{
				if (!inMovementPhase) {
						inMovementPhase = true;
						showingPlayerMustTradeGoodsForExplorerScreen = false;
						waitingOnExplorerReturn = false;
						showingEndOfMovePhaseScreen = false;
						updateExplorerAndPlayerMovementVariablesForThisTurn ();
						getFirstPlayer ();
			            
				}

				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("Begin Movement Phase");
				

		}

		void updateExplorerAndPlayerMovementVariablesForThisTurn ()
		{
				GameObject[] explorers = GameObject.FindGameObjectsWithTag ("explorer");
				foreach (GameObject explorer in explorers) {
						if (explorer.GetComponent<DesertExplorer> ().mustMissThisTurn ()) {
								explorer.GetComponent<DesertExplorer> ().makeExplorerMissThisTurn ();
						} else {
								explorer.GetComponent<DesertExplorer> ().hasMovedThisRound = false;
					
								explorer.GetComponent<Meeple> ().player.GetComponent<Player> ().moveableDesertExplorers++;
						}
						logExplorerData (explorer);

				}

				resetPlayerMovementWithRegardToWaterAndMoveableExplorer ();
			
		}

		void logExplorerData (GameObject explorer)
		{
				DesertExplorer ex = explorer.GetComponent<DesertExplorer> ();
				Meeple m = explorer.GetComponent<Meeple> ();
				string explorerName = m.id;
				string explorerPlayer = m.player.GetComponent<Player> ().id;
				string position = ex.currentTile.GetComponent<DesertTile> ().rp.x + "," + ex.currentTile.GetComponent<DesertTile> ().rp.y;
				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent (explorerName + "," + explorerPlayer + "," + position);

		}

		void logPlayerData (GameObject player)
		{
				Player pl = player.GetComponent<Player> ();
				PlayerInventory pi = player.GetComponent<PlayerInventory> ();
				string name = pl.id;
				string availableWater = "" + pi.availableWater;
				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent (name + "," + availableWater);
		}

		void resetPlayerMovementWithRegardToWaterAndMoveableExplorer ()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject player in players) {
						player.GetComponent<Player> ().hasMovedAnExplorerThisTurn = false;


						//!!this condition requires that player has at least one meeple on the board;
						//else it won't recognize that the player can't take a turn.
						//this should work but for safety I'm going to just make it the weaker condition,
						//safe from infinite loop since now players can stop move phase.

	
						player.GetComponent<Player> ().updateWhetherCanMoveAgainThisRound ();
						logPlayerData (player);
			          
						//weaker condition; based strictly on available water
						//player.GetComponent<Player> ().canMoveAgainThisRound = player.GetComponent<PlayerInventory> ().waterAvailable ();
			          
						//strictly for testing; just easier to go thru various turns.
						//player.GetComponent<PlayerInventory> ().changeAvailableWaterDuringPlacementPhase (100);
				}
		
		}
	
		bool noPlayersCanMakeMove ()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				bool result = true;
				foreach (GameObject player in players) {
						bool thisPlayerCanMoveAgain = player.GetComponent<Player> ().canMoveAgainThisRound;
						if (thisPlayerCanMoveAgain)
								return false;

				}
				return result;
			

		}

		//get the current player from game controller;
		//if that player can't make a move then first player is the first player returned from updatePlayer()
		void getFirstPlayer ()
		{
				GameObject firstPlayer = gameObject.GetComponent<GameController> ().currentPlayer ();
				if (firstPlayer.GetComponent<Player> ().canMoveAgainThisRound)
						GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (firstPlayer);
				else
						updatePlayer ();

		}


		//this should only by called by the players during the movement phase
		public void updatePlayer ()
		{
				if (Event.anEventIsHappeningInGeneral) {
						updatePlayerWaitingOnEvent = true;
				} else {
						updatePlayerWaitingOnEvent = false;

						if (noPlayersCanMakeMove ()) {
								endDesertMovementPhase ();

						} else { 
					
								GameObject nextPlayer;
								do {
										nextPlayer = gameObject.GetComponent<GameController> ().getNextPlayer ();
								} while (!nextPlayer.GetComponent<Player>().canMoveAgainThisRound);

								GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (nextPlayer);
						}
				}
		}

		void endDesertMovementPhase ()
		{
				sendExplorersOnBazaarBackToSource ();
				resetAllPlayerMovementVariables ();
		   
				eventStartTime = Time.time;
				waitingOnExplorerReturn = true;
				

		}

		void announceEndOfMovePhaseAfterDelay ()
		{      
				if ((Time.time - eventStartTime) >= Draggable.iTweenTime) {
			
						announceEndOfMovePhase ();
			
				}
		}

		void announceEndOfMovePhase ()
		{
				
				showingEndOfMovePhaseScreen = true;
				initializeEvent ();
				waitingOnExplorerReturn = false;
			
		
		}

		public override void activateEvent (GameObject aNullValueUseTheCollection)
		{       
				showingEndOfMovePhaseScreen = false;
				showingPlayerMustTradeGoodsForExplorerScreen = true;
				initializeEvent ();
			
				string playerIds = "";
				foreach (GameObject player in playersWhoMustTradeGoodsForExplorer)
						playerIds = playerIds + player.GetComponent<Player> ().id + ", ";
				needToTradeGoodsForMeepleMessage = playerIds + needToTradeGoodsForMeepleMessage;
			
		}

		public override void activateEvent ()
		{
		}

		void Update ()
		{
				if (updatePlayerWaitingOnEvent)
						updatePlayer ();

				if (waitingOnExplorerReturn) {
						announceEndOfMovePhaseAfterDelay ();
				}
				if (effectOccurring) {
						if (showingEndOfMovePhaseScreen) {
								displayResultOfTwoCaseEvent (true, movementEndedMessage, movementEndPartTwoMessage, "");
						} else if (showingPlayerMustTradeGoodsForExplorerScreen) {
								displayResultOfTwoCaseEvent (true, needToTradeGoodsForMeepleMessage, partTwo, "");
						} 

				} else if (inControlOfTextBox) {
						closeEvent ();
						
				
						closeMovementPhase ();
				}
		      
				
				
		}
		
		protected override void takeEffect ()
		{
			
				if (showingEndOfMovePhaseScreen) {
						//resetAllPlayerMovementVariables ();
						checkForPlayersWhoNeedToReturnMeepleToSourceForGood ();
				
					
				}
				if (showingPlayerMustTradeGoodsForExplorerScreen) {
						takeRandomGoodFromPlayersAndMoveRandomMeepleToSource ();
						
				}
				
		}

		void resetAllPlayerMovementVariables ()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject player in players) {
						player.GetComponent<PlayerInventory> ().drainWater ();
						player.GetComponent<Player> ().reactToEndOfMovePhase ();
						player.GetComponent<Player> ().moveableDesertExplorers = 0;
						player.GetComponent<Player> ().canMoveAgainThisRound = false;
				}

		}

		void takeRandomGoodFromPlayersAndMoveRandomMeepleToSource ()
		{
				foreach (GameObject player in playersWhoMustTradeGoodsForExplorer) {
						player.GetComponent<Player> ().returnRandomExplorerToSource ();
						player.GetComponent<PlayerInventory> ().removeRandomGood ();
				}

		}

		void checkForPlayersWhoNeedToReturnMeepleToSourceForGood ()
		{
				
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject player in players) {
			            
						if (allPlayersMeeplesAreExploringAndWellIsEmpty (player))
								handlePlayerWithoutSourceMeeples (player);
				}

				//if some player had no water activate event
				if (somePlayersNeedToTradeGoodsForExplorer ())
						activateEvent (null);
			
			
		}

		bool allPlayersMeeplesAreExploringAndWellIsEmpty (GameObject player)
		{       

				return (noMeeplesAvailableToMineWater (player) && cannotHarvestMoreWaterFromWell (player));
			 
	
		}

		bool noMeeplesAvailableToMineWater (GameObject player)
		{
				return player.GetComponent<Player> ().exploringMeeples.Count == GameController.numMeeplesPerPlayer;
		}

		bool cannotHarvestMoreWaterFromWell (GameObject player)
		{
				return player.GetComponent<PlayerInventory> ().wellDepth == 0;
		}

		void handlePlayerWithoutSourceMeeples (GameObject player)
		{      
			
				playersWhoMustTradeGoodsForExplorer.Add (player);
				
		
		}

		bool somePlayersNeedToTradeGoodsForExplorer ()
		{
				return playersWhoMustTradeGoodsForExplorer.Count > 0;
		}

		void closeMovementPhase ()
		{
				
				resetDesertState ();
				playersWhoMustTradeGoodsForExplorer.Clear ();

				inMovementPhase = false;

				//initiate worker placement phase

				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("End Movement Phase");
	
				gameObject.GetComponent<GameController> ().BeginPlacementPhase ();
				
				//only for testing carry-over events
				//beginDesertMovementPhase ();
				
		}

		void resetDesertState ()
		{
				GameObject desert = GameObject.Find ("Desert");
				desert.GetComponent<DesertState> ().movingObject = null;
				desert.GetComponent<DesertState> ().playerWhoseTurnItIs = null;

		}

		void sendExplorersOnBazaarBackToSource ()
		{
				GameObject[] explorers = GameObject.FindGameObjectsWithTag ("explorer");
				foreach (GameObject explorer in explorers) {
						if (explorer.GetComponent<DesertExplorer> ().onBazaar ()) {
								explorer.GetComponent<Meeple> ().endExploration ();
						}

				}


		}

		void OnGUI ()
		{
				
				if (inMovementPhase) {
						if (GUI.Button (new Rect (buttonStartX, buttonY, buttonWidth, buttonHeight), "End Movement")) {
								endDesertMovementPhase ();
				
						}
			           
						



				}

		}


	  
		


}
