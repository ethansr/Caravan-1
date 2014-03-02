using UnityEngine;
using System.Collections;

public class MagicCarpet : Event
{

		float buttonWidth = 100;
		float buttonHeight = 30;
		float buttonStartX = 1200;
		float buttonY = 50;

		//magic carpet controller variables
		public static GameObject playerWithMagicCarpet;
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
						disableEventTextBox ();
						inControlOfTextBox = false;
						anEventIsHappeningInGeneral = false;

						//tellPlayerToFinishEndTurn ();
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
	
		public void setTilePlayerHasChosen (GameObject chosenTile)
		{
				if (!chosenTile.GetComponent<DesertTile> ().isBazaar () && chosenTile.GetComponent<DesertTile> ().roomForMoreOccupants ())
						tileToMoveTo = chosenTile;
		
		}
	
		public void setExplorerPlayerHasChosen (GameObject explorer)
		{
				if (explorer.tag.Equals ("explorer") && explorer.GetComponent<Meeple> ().player == playerWithMagicCarpet)
						explorerToMove = explorer;
		
		}
	
		bool playerHasMadeValidSelectionOfTileAndExplorer ()
		{
		
				if (explorerToMove && tileToMoveTo) {
						if (tileToMoveTo.GetComponent<DesertTile> ().getNumOccupants () > 0) 
								return tileToMoveTo.GetComponent<DesertTile> ().handleOccupiedTile (explorerToMove, tileToMoveTo);
			
						return true;
				}
				return false;
		}
	
		void moveChosenExplorerToChosenTile ()
		{
				explorerToMove.GetComponent<DesertExplorer> ().updateLocation (tileToMoveTo);
		}
	
		void closeMagicCarpetEvent ()
		{
		
				tileToMoveTo = null;
				explorerToMove = null;
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
				if (DesertMovementController.inMovementPhase && playerWithMagicCarpet) {
						if (GUI.Button (new Rect (buttonStartX - buttonWidth * 2, buttonY, buttonWidth * 1.5f, buttonHeight), "Magic Carpet Ride")) {
								if (playerWhoseTurnItIsHasMagicCarpetPower ())
										activateEvent (null);
			
		
						}
				}
		}
			
		bool playerWhoseTurnItIsHasMagicCarpetPower ()
		{
				return GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs == playerWithMagicCarpet;
		}

		public override void activateEvent (GameObject dummy)
		{
				initializeEvent ();
				showingMagicCarpetScreen = true;
				
		}
			
		void initializeEvent ()
		{
				anEventIsHappeningInGeneral = true;
				effectOccurring = true;
				inControlOfTextBox = true;
				tookEffect = false;
				eventStartTime = Time.time;
		}

}
