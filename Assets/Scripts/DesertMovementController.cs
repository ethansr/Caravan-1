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
		
		public void beginDesertMovementPhase ()
		{
				if (!inMovementPhase) {
						inMovementPhase = true;
						updateExplorerAndPlayerMovementVariablesForThisTurn ();
						getFirstPlayer ();
			            
				}
				
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

				}

				resetPlayerMovementWithRegardToWaterAndMoveableExplorer ();
			
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
			          
						//weaker condition; based strictly on available water
						//player.GetComponent<Player> ().canMoveAgainThisRound = player.GetComponent<PlayerInventory> ().waterAvailable ();
			
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

		void endDesertMovementPhase ()
		{
				sendExplorersOnBazaarBackToSource ();
		   
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
				effectOccurring = true;

				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				waitingOnExplorerReturn = false;
			
		
		}

		public override void activateEvent (GameObject aNullValueUseTheCollection)
		{       
				effectOccurring = true;
				showingEndOfMovePhaseScreen = false;
				showingPlayerMustTradeGoodsForExplorerScreen = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
			
				string playerIds = "";
				foreach (GameObject player in playersWhoMustTradeGoodsForExplorer)
						playerIds = playerIds + player.GetComponent<Player> ().id + ", ";
				needToTradeGoodsForMeepleMessage = playerIds + needToTradeGoodsForMeepleMessage;
			
		}
	
		void Update ()
		{       
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
								disableEventTextBox ();
								inControlOfTextBox = false;
								closeMovementPhase ();
						}
				
		}

		protected override void takeEffect ()
		{
			
				if (showingEndOfMovePhaseScreen) {
						resetAllPlayerMovementVariables ();
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
