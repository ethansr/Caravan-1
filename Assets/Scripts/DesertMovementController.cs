using UnityEngine;
using System.Collections;

public class DesertMovementController : Event
{
		GameObject playerWhoMustTradeMeepleForGoods;
		const string needToTradeGoodsForMeepleMessage = "You must give up goods for an Explorer.";
		string partTwo = "You cannot choose which good; you can choose which Explorer.";

		public void beginDesertMovementPhase ()
		{
				//disable workerPlacementController
				gameObject.GetComponent<WorkerPlacementController> ().enabled = false;
			


				//tell the desert who is the first player
				updatePlayer ();
				
		}

		void resetPlayerMoveVariablesAndCheckForNoWater ()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				foreach (GameObject player in players) {
						player.GetComponent<Player> ().hasMovedAnExplorerThisTurn = false;
						if (player.GetComponent<PlayerInventory> ().waterAvailable ())
								player.GetComponent<Player> ().canMoveAgainThisRound = true;
						else {
								handlePlayerWithoutWater (player);
						}
						
			
				}

		}

		void handlePlayerWithoutWater (GameObject player)
		{      
				int playersMeeplesOnSource = player.GetComponent<Player> ().meepleSource.GetComponent<MeepleSource> ().meeplesOnSource;
				if (playersMeeplesOnSource == 0) {
						activateEvent (player);

				}


		}

		void allExplorersCanMove ()
		{
				GameObject[] explorers = GameObject.FindGameObjectsWithTag ("explorer");
				foreach (GameObject explorer in explorers) {
						explorer.GetComponent<DesertExplorer> ().hasMovedThisRound = false;

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

		void endDesertMovementPhase ()
		{
				Debug.Log ("End movement phase");
				resetDesertState ();
				resetPlayerMoveVariablesAndCheckForNoWater ();
				allExplorersCanMove ();

				//for testing
				//beginDesertMovementPhase ();

		       



		        
				//gameObject.GetComponent<WorkerPlacementController> ().enabled = true;
		}
	   
		void resetDesertState ()
		{
				GameObject desert = GameObject.Find ("Desert");
				desert.GetComponent<DesertState> ().movingObject = null;
				desert.GetComponent<DesertState> ().playerWhoseTurnItIs = null;

		}

		public void updatePlayer ()
		{
				
				if (noPlayersCanMakeMove ()) {
						Debug.Log ("called no players can move");
						endDesertMovementPhase ();
				} else { 
						int safety = 0;
						GameObject nextPlayer;
						do {
								nextPlayer = gameObject.GetComponent<GameController> ().getNextPlayer ();
								safety++;
								if (safety == 20) {
										Debug.Log (safety);
										return;
								}
						} while (!nextPlayer.GetComponent<Player>().canMoveAgainThisRound);

						GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (nextPlayer);
				}
		}

		protected override void takeEffect ()
		{


		}

		public override void activateEvent (GameObject player)
		{
				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
				playerWhoMustTradeMeepleForGoods = player;

		}

		void Update ()
		{

	
				if (effectOccurring) {
						displayResultOfTwoCaseEvent (true, needToTradeGoodsForMeepleMessage, partTwo, "");

				} else if (inControlOfTextBox) {
						disableEventTextBox ();
						inControlOfTextBox = false;
				}

		}

	   
}
