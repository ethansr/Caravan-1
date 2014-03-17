using UnityEngine;
using System.Collections;

public class MagicCarpet : Event
{

		float buttonWidth = 100;
		float buttonHeight = 30;
		float buttonStartX = 1200;
		float buttonY = 50;

		//magic carpet controller variables
		static GameObject playerWithMagicCarpet;
		//requires:
		//! bazaaar
		//otherwise this movement should behave the same as all other movements
		// i.e., if player has invasion power then they can move into a tile occupied by other meeples.
	
		public static GameObject tileToMoveTo;
		public static  GameObject explorerToMove;
		bool showingMagicCarpetScreen;
		string magicCarpetMessage = "You unfurl your magical carpet...";
		string partTwoMagicCarpet = "Click on an explorer; click on a tile," + System.Environment.NewLine + " and you will travel there.";
		public static bool waitingForPlayersMagicCarpetSelection;
		// Use this for initialization



		// Update is called once per frame
		void Update ()
		{
				if (effectOccurring) {
			
						displayResultOfTwoCaseEvent (true, magicCarpetMessage, partTwoMagicCarpet, "");
			
				} else if (inControlOfTextBox) {
						closeEvent ();
				}

				handleMagicCarpetPlayerSelection ();

	
		}

		void handleMagicCarpetPlayerSelection ()
		{
				if (waitingForPlayersMagicCarpetSelection) {
						if (playerHasMadeValidSelectionOfTileAndExplorer ()) {
								moveChosenExplorerToChosenTile ();
								closeMagicCarpetEvent ();
						}
				}
		}
	
		public static void setTilePlayerHasChosen (GameObject chosenTile)
		{
				if (explorerToMove && validTile (chosenTile))
						tileToMoveTo = chosenTile;
		
		}
	
		public static void setExplorerPlayerHasChosen (GameObject explorer)
		{     
				if (validExplorer (explorer))
						explorerToMove = explorer;
	
				Debug.Log (explorerToMove);

		}
	
		bool playerHasMadeValidSelectionOfTileAndExplorer ()
		{
		
				return (explorerToMove && tileToMoveTo);
				
		}

		static bool validExplorer (GameObject explorer)
		{
			

				return explorer.tag.Equals ("explorer") && explorer.GetComponent<Meeple> ().player == playerWithMagicCarpet && !explorer.GetComponent<DesertExplorer> ().hasMovedThisRound;
				
		}

		static bool validTile (GameObject tile)
		{
				return !tile.GetComponent<DesertTile> ().isBazaar () && tile.GetComponent<DesertTile> ().roomForMoreOccupants () && explorerCanMoveToTile (tile);
		}

		static bool explorerCanMoveToTile (GameObject tile)
		{
				if (tile.GetComponent<DesertTile> ().getNumOccupants () > 0) 
						return tile.GetComponent<DesertTile> ().handleOccupiedTile (explorerToMove, tile);
		
				return true;

		}
	
		void moveChosenExplorerToChosenTile ()
		{
				explorerToMove.GetComponent<DesertExplorer> ().updateLocation (tileToMoveTo);
				explorerToMove.GetComponent<DesertExplorer> ().decreaseAvailableWater ();
				//explorerToMove.GetComponent<DesertExplorer> ().reactToMovementEndingStayInDesert ();

		}
	
		void closeMagicCarpetEvent ()
		{
		
				tileToMoveTo = null;
				explorerToMove = null;
				playerWithMagicCarpet.GetComponent<PlayerInventory> ().hasMagicCarpetPower = false;
				Object.Destroy (playerWithMagicCarpet.GetComponent<PlayerInventory> ().playersMagicCarpetToken);
				playerWithMagicCarpet = null;
				showingMagicCarpetScreen = false;
				waitingForPlayersMagicCarpetSelection = false;
		
		}

		protected override void takeEffect ()
		{
				if (showingMagicCarpetScreen) {
						waitingForPlayersMagicCarpetSelection = true;
				}
		}

		void OnGUI ()
		{
				if (DesertMovementController.inMovementPhase && playerWhoseTurnItIsHasMagicCarpetPower () && playerHasntMovedMeeple ()) {
						if (GUI.Button (new Rect (buttonStartX - buttonWidth * 2, buttonY, buttonWidth * 1.5f, buttonHeight), "Magic Carpet Ride")) {
					
								activateEvent (GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs);
			
		
						}
				}
		}

		bool playerHasntMovedMeeple ()
		{
				return !GameObject.Find ("Desert").GetComponent<DesertState> ().movingObject;
		}

		bool playerWhoseTurnItIsHasMagicCarpetPower ()
		{
				GameObject player = GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs;
				return (player && player.GetComponent<PlayerInventory> ().hasMagicCarpetPower);
		}
		/*
		public override void activateEvent (GameObject player)
		{
	
				playerWithMagicCarpet = player;
				initializeEvent ();
				showingMagicCarpetScreen = true;
				
		}

		public override void activateEvent ()
		{
		}
		*/

		public override void activateEvent (GameObject player)
		{
		
				playerWithMagicCarpet = player;
				EventManager.addEventToQueue (gameObject.GetComponent<Event> ());

		
		}
	
		public override void activateEvent ()
	{           name = "magic carpet ride";
				initializeEvent ();
				showingMagicCarpetScreen = true;
		}



}
