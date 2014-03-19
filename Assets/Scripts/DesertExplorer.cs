using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class DesertExplorer : MonoBehaviour
{
		public GameObject currentTile;
		public Color defaultColor;
		public Color fColor;
		public int flash = 1;

		//both of these should be clear on start, and be cleared each time a meeple 
		//returns to the source (it should be the last call of "end exploration")
		Collection<GameObject> eventsExperiencedThisExploration;
		Dictionary<DesertGenerator.GoodItem, bool> hasCollectedGoodThisExploration;
		GameObject lastGoodAcquired;
		GameObject lastEventExperienced;
		public Vector3 bazaarPosition;
		Vector3 currentPos;
		GameObject desert;
		public string id;
		public bool hasMovedThisRound;
		public bool missNextTurn = false;
		public static Vector3 movingSize;
		public static Vector3 defaultSize;
	
		void Start ()
		{
				desert = GameObject.Find ("Desert");
				hasMovedThisRound = false;
				defaultColor = GetComponent<SpriteRenderer> ().color;
				fColor.a = 255.0f;

				eventsExperiencedThisExploration = new Collection<GameObject> ();
				initializeGoodItemRecord ();
		}

		void initializeGoodItemRecord ()
		{
				hasCollectedGoodThisExploration = new Dictionary<DesertGenerator.GoodItem, bool> ();
				foreach (DesertGenerator.GoodItem goodItem in (DesertGenerator.GoodItem[])Enum.GetValues (typeof(DesertGenerator.GoodItem))) {
						hasCollectedGoodThisExploration.Add (goodItem, false);
				}
		}
	
		//assume that move was successful
		public void updateLocation (GameObject newLocation)
		{      //the only case where we dont leave the tile is when we try moving back to bazaar when we are mercenary.
				if (!(returnedToBazaar (newLocation) && isMercenary ()))
						leaveCurrentTile ();
				//...not actually equivalent to if(returned to bazaar and not mercenary, note).
				if (returnedToBazaar (newLocation)) {  
						if (!isMercenary ())
								returnToSource ();
						//(else, if the mercenary tries to move to bazaar, don't move).
				} else {
						moveToNewDesertTile (newLocation);
						if (isAnEventToExperienceOnCurrentLocation ())
								experienceEvent ();
				}
		
		}

		public void clearGoodsAndEventsRecords ()
		{
				if (eventsExperiencedThisExploration != null) {
						eventsExperiencedThisExploration.Clear ();
						setHasCollectedGoodsToFalse ();
				}
		}
	    
		void setHasCollectedGoodsToFalse ()
		{
				foreach (DesertGenerator.GoodItem goodItem in (DesertGenerator.GoodItem[])Enum.GetValues (typeof(DesertGenerator.GoodItem))) {
						hasCollectedGoodThisExploration [goodItem] = false;
				}
		}

		public void leaveCurrentTile ()
		{
				currentTile.GetComponent<DesertTile> ().leaveTile (gameObject, gameObject.GetComponent<Transform> ().position);
		}
	
		bool returnedToBazaar (GameObject newLocation)
		{
				return (newLocation.GetComponent<Bazaar> () != null);
		
		}
	
		void returnToSource ()
		{
				stopFlashing ();
				removeSelfFromDesertState ();
				closeMovement ();

				//endPlayersTurn ();
				//reactToMovementEnding ();
				
				gameObject.GetComponent<Meeple> ().endExploration ();
				endPlayersTurn ();
		
		}

		//this method is here strictly so that in the case where there is an event on the tile
		//just before the bazaar, the meeple leaving doesnt experience the event as he does
		void removeSelfFromDesertState ()
		{
				GameObject.Find ("Desert").GetComponent<DesertState> ().movingObject = null;

		}
		//assume that move was successful
		public void moveToNewDesertTile (GameObject newLocation)
		{      
				currentTile = newLocation;
				currentPos = currentTile.GetComponent<DesertTile> ().enterTile (gameObject);
				currentPos.z = 1;
				GetComponent<Transform> ().position = currentPos;
				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("Enter tile," +getId ()+ getTileInformation ());
		
		}

		string getId ()
		{
				return gameObject.GetComponent<Meeple> ().id + ",";

		}

		string getTileInformation ()
		{
				int x = currentTile.GetComponent<DesertTile> ().rp.x;
				int y = currentTile.GetComponent<DesertTile> ().rp.y;
				return "" + x + "," + y;
		}
	
		void OnMouseUpAsButton ()
		{
				
				if (MagicCarpet.waitingForPlayersMagicCarpetSelection) {
			
						MagicCarpet.setExplorerPlayerHasChosen (gameObject);
						
			
				} else if (ableToMove ()) { //(DesertMovementController.inMovementPhase && isMyPlayersTurn () && currentTile && firstExplorerMovedThisTurn () && !hasMovedThisRound) {
					
						makeMover ();
				}

		}

		bool ableToMove ()
		{

				return DesertMovementController.inMovementPhase && isMyPlayersTurn () && currentTile && firstExplorerMovedThisTurn () && !hasMovedThisRound;
		}

		bool isMyPlayersTurn ()
		{
				return gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().isPlayersTurn ();
		}


		//return true if this explorer's player has NOT moved an exploer this turn already
		bool firstExplorerMovedThisTurn ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				return !myPlayer.GetComponent<Player> ().hasMovedAnExplorerThisTurn || lastMovingExplorerIsMe ();
		}

		bool lastMovingExplorerIsMe ()
		{
				return desert.GetComponent<DesertState> ().movingExplorer == gameObject;
		}

		bool playerHasntMovedTileThisTurn ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				return !myPlayer.GetComponent<Player> ().hasRotatedATileThisTurn;
		}
	
		void Update ()
		{         
				if (moving () || flyingMagicCarpet ())
						graphicallyIndicateMoveState ();
				else
						stopFlashing ();

				if (!Event.anEventIsHappeningInGeneral && moving ()) {
						GameObject newLocation = getNewLocationGivenKeyInput ();
						if (moveSuccessful (newLocation)) 
								handleSuccessfulMove (newLocation);
				}
				maintainPosition ();
		}
		
		void maintainPosition ()
		{
				
				GetComponent<Transform> ().position = currentPos;
		}
	
		bool moving ()
		{
				return (isMyPlayersTurn () && isMover () && waterAvailable ());
		}
	   
		bool flyingMagicCarpet ()
		{
				
				return MagicCarpet.explorerToMove == gameObject;
		}

		public void reactToMovementEndingStayInDesert ()
		{
				decrementPlayersMoveableExplorers ();
				closeMovement ();

				endPlayersTurn ();
		}

		void closeMovement ()
		{
				preventThisExplorerFromMovingAgainThisRound ();
				//decrementPlayersMoveableExplorers ();
				GetComponent<Transform> ().localScale = defaultSize;
		}
	    
		bool isAnEventToExperienceOnCurrentLocation ()
		{
				return (finishedOnEventTile () && haventAlreadyExperienceEvent ());
		}
	
		bool finishedOnEventTile ()
		{
				return currentTile.GetComponent<DesertTile> ().hasDesertEvent ();
		}

		bool haventAlreadyExperienceEvent ()
		{
				GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
				return !eventsExperiencedThisExploration.Contains (newEvent);

				//return (newEvent != lastEventExperienced && !myPlayerHasExperiencedEvent (newEvent));
			
		}

		void endPlayersTurn ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().finishEndTurn ();
		}
		
		void preventThisExplorerFromMovingAgainThisRound ()
		{
				hasMovedThisRound = true;
		}

		void decrementPlayersMoveableExplorers ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().changeMovebleDesertExplorers (-1);
		
		}

		void experienceEvent ()
		{
				GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
				eventsExperiencedThisExploration.Add (newEvent);
				//lastEventExperienced = newEvent;
				updatePlayersEvents (newEvent);
				newEvent.GetComponent<Event> ().activateEvent (gameObject);

		}

		bool myPlayerHasExperiencedEvent (GameObject newEvent)
		{
				return gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().alreadyExperiencedThisEventThisTurn (newEvent);
		}

		void updatePlayersEvents (GameObject newEvent)
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().addEvent (newEvent);
		}
	
		GameObject getNewLocationGivenKeyInput ()
		{       
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
						return currentTile.GetComponent<DesertTile> ().MoveVertical (-1);
				} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
						return currentTile.GetComponent<DesertTile> ().MoveVertical (1);
				} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
						return currentTile.GetComponent<DesertTile> ().MoveHorizontal (-1);
				} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
						return currentTile.GetComponent<DesertTile> ().MoveHorizontal (1);
				}
				return null;
		
		}
	
		bool moveSuccessful (GameObject newLocation)
		{
				return newLocation != null;
		}
	
		void handleSuccessfulMove (GameObject newLocation)
		{          
				if (newLocation.GetComponent<Good> ()) {
						if (haveAlreadyCollectedThisGood (newLocation))//(newLocation == lastGoodAcquired)
								return; 
						addGoodToPlayerInventory (newLocation);
						recordThatIHaveCollectedThisGood (newLocation);
				} else 
						updateLocation (newLocation);
		
				decreaseAvailableWater ();
			
		}

		bool haveAlreadyCollectedThisGood (GameObject goodTile)
		{
				return hasCollectedGoodThisExploration [goodTile.GetComponent<Good> ().good];
		}
	    
		void recordThatIHaveCollectedThisGood (GameObject goodTile)
		{
				hasCollectedGoodThisExploration [goodTile.GetComponent<Good> ().good] = true;
		}

		void addGoodToPlayerInventory (GameObject goodTile)
		{
				goodTile.GetComponent<Good> ().addGoodToPlayerInventory (GetComponent<Meeple> ().player);
		}
	
		bool waterAvailable ()
		{ 
				return gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().waterAvailable ();
		}
	
		void graphicallyIndicateMoveState ()
		{
				if (flash == 1)
						GetComponent<SpriteRenderer> ().color = fColor;
				else 
						GetComponent<SpriteRenderer> ().color = defaultColor;
				flash *= -1;
			

		}

		public void decreaseAvailableWater ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWaterDuringMovement (-1);

		}
	
		void stopFlashing ()
		{
				GetComponent<SpriteRenderer> ().color = defaultColor;
		}

		bool isMover ()
		{
				return desert.GetComponent<DesertState> ().movingObject == gameObject;
		}
	
		void makeMover ()
		{
				if (!hasMovedThisRound) {
						desert.GetComponent<DesertState> ().setMovingExplorer (gameObject);
						recordMovementOfExplorerToPlayer ();
						GetComponent<Transform> ().localScale = movingSize;
				}
		       
				
		}

		void recordMovementOfExplorerToPlayer ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				myPlayer.GetComponent<Player> ().hasMovedAnExplorerThisTurn = true;
		}



		//if the mercenary is the sole occupant of this tile, he will accept the invader no matter what
		//don't accept an invader on a tile that is already an event tile.
		public bool acceptInvader (GameObject invader)
		{       
				if (!isForeign (invader))
						return true;

				/*
				//mercenary doesn't have invasion power.
				if (invader.GetComponent<DesertExplorer> ().isMercenary ())
						return false;

				//is foreign; if current tile has event return false.

		/*
				if (currentTile.GetComponent<DesertTile> ().hasDesertEvent ())
						return false;
						*/


				//foreign and tile does not have event
				if (!isMercenary ()) {
						if (playerHasInvaderPower (invader)) {
								return invader.GetComponent<Invader> ().prepareForInvasion ();
								
								//return true;
						} else 
								return false;
				}
				//mercenary and tile does not have event
				return true;
		}
	
		public void handleInvader (GameObject invader)
		{
				

				if (isMercenary () && isForeign (invader)) 
						GetComponent<MercenaryExplorer> ().activateEvent (invader);
				else if (isForeign (invader) && playerHasInvaderPower (invader) && invader.GetComponent<Invader> ().entersFromInvadingTile ())
						invader.GetComponent<Invader> ().activateEvent (gameObject);
				
				
			

		}

		bool playerHasInvaderPower (GameObject invader)
		{

				return (invader.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().canInvade);

		}

		public bool isMercenary ()
		{
				return GetComponent<MercenaryExplorer> () != null;

		}

		bool isForeign (GameObject invader)
		{
				
				return !(invader.GetComponent<Meeple> ().player.Equals (GetComponent<Meeple> ().player));
		}

		public void makeMissNextTurn ()
		{
				missNextTurn = true;

		}

		public bool mustMissThisTurn ()
		{
				return missNextTurn;
		}

		public void makeExplorerMissThisTurn ()
		{
				preventThisExplorerFromMovingAgainThisRound ();
				
				missNextTurn = false;
		}

		//should we use this?
		public void resetEventAnddGoodVariablesForNextTurn ()
		{
				lastEventExperienced = null;
				lastGoodAcquired = null;
		}

		public bool onBazaar ()
		{
				return currentTile.GetComponent<DesertTile> ().isBazaar ();

		}
	
	
	
	
	
}
