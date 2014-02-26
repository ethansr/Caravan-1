using UnityEngine;
using System.Collections;

public class DesertExplorer : MonoBehaviour
{
		public GameObject currentTile;
		public Color defaultColor;
		public Color fColor;
		public int flash = 1;
		GameObject lastGoodAcquired;
		GameObject lastEventExperienced;
		public Vector3 bazaarPosition;
		Vector3 currentPos;
		GameObject desert;
		public string id;
		public bool hasMovedThisRound;
		public bool missNextTurn = false;
	
		void Start ()
		{
				desert = GameObject.Find ("Desert");
				hasMovedThisRound = false;
				defaultColor = GetComponent<SpriteRenderer> ().color;
				fColor.a = 255.0f;
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
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().endTurn ();
				gameObject.GetComponent<Meeple> ().endExploration ();
		
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
		
		}
	
		void OnMouseUpAsButton ()
		{  
				if (isMyPlayersTurn () && currentTile && firstExplorerMovedThisTurn ()) {

						makeMover ();
				}     
		}
	
		bool isMyPlayersTurn ()
		{
				return gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().isPlayersTurn ();
		}


		//return true if this explorer's player has NOT moved an exploer this turn already
		bool firstExplorerMovedThisTurn ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				return !myPlayer.GetComponent<Player> ().hasMovedAnExplorerThisTurn;
		}

		bool playerHasntMovedTileThisTurn ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				return !myPlayer.GetComponent<Player> ().hasRotatedATileThisTurn;
		}
	
		void Update ()
		{         
				

				if (!Event.anEventIsHappeningInGeneral && moving ()) {
						flashToIndicateMoveState ();
						GameObject newLocation = getNewLocationGivenKeyInput ();
						if (moveSuccessful (newLocation)) 
								handleSuccessfulMove (newLocation);
				} else {
						stopFlashing ();
					
				}
		
				
				maintainPosition ();
		}
		//I don't really understand why this is neccessary tbh, but it seems to be.
		void maintainPosition ()
		{
				GetComponent<Transform> ().position = currentPos;
		}
	
		bool moving ()
		{
				return (isMyPlayersTurn () && isMover () && waterAvailable ());
		}

		//called when we run out of water,
		//and by the desert generator.


		public void reactToMovementEnding ()
		{
				preventThisExplorerFromMovingAgainThisRound ();
				decrementPlayersMoveableExplorers ();
				
				if (isAnEventToExperienceOnCurrentLocation ()) {
						tellEventToEndTurnWhenFinished ();
						experienceEvent ();
				} else
						endPlayersTurn ();
				
		}
	    
		bool isAnEventToExperienceOnCurrentLocation ()
		{
				return (finishedOnEventTile () && haventAlreadyExperienceEvent ());
		}

		void tellEventToEndTurnWhenFinished ()
		{
				GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
				newEvent.GetComponent<Event> ().endPlayersTurn = true;
		}
	
		bool finishedOnEventTile ()
		{
				return currentTile.GetComponent<DesertTile> ().hasDesertEvent ();
		}

		bool haventAlreadyExperienceEvent ()
		{
				GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
				return (newEvent != lastEventExperienced && !myPlayerHasExperiencedEvent (newEvent));
			
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
				lastEventExperienced = newEvent;
				updatePlayersEvents (newEvent);
				newEvent.GetComponent<Event> ().activateEvent (gameObject);

		}
		/*
		void experienceEventIfHaventAlready ()
		{
				if (currentTile.GetComponent<DesertTile> ().hasDesertEvent ()) {
						GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
						if (newEvent != lastEventExperienced && !myPlayerHasExperiencedEvent (newEvent)) {
								lastEventExperienced = newEvent;
								updatePlayersEvents (newEvent);
								newEvent.GetComponent<Event> ().activateEvent (gameObject);
						} else
								gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().finishEndTurn ();
				} else 
						gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().finishEndTurn ();
		}
		*/

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
						if (newLocation == lastGoodAcquired)
								return; 
						addGoodToPlayerInventory (newLocation);
						lastGoodAcquired = newLocation;
			
				} else 
						updateLocation (newLocation);
		
				decreaseAvailableWater ();
			
		}

		void addGoodToPlayerInventory (GameObject goodTile)
		{
				goodTile.GetComponent<Good> ().addGoodToPlayerInventory (GetComponent<Meeple> ().player);
		}
	
		bool waterAvailable ()
		{ 
				return gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().waterAvailable ();
		}
	
		void flashToIndicateMoveState ()
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
						desert.GetComponent<DesertState> ().changeMover (gameObject);
						recordMovementOfExplorerToPlayer ();
				}
		       
				
		}

		void recordMovementOfExplorerToPlayer ()
		{
				GameObject myPlayer = gameObject.GetComponent<Meeple> ().player;
				myPlayer.GetComponent<Player> ().hasMovedAnExplorerThisTurn = true;
		}



		//if the mercenary is the sole occupant of this tile, he will accept the invader no matter what
		public bool acceptInvader (GameObject invader)
		{      
				if (!isMercenary ()) {
						if (playerHasInvaderPower (invader)) {
								invader.GetComponent<Invader> ().prepareForInvasion ();
								return true;
						} else 
								return (!isForeign (invader));
				}
                
				return true;
		}
	
		public void handleInvader (GameObject invader)
		{
				

				if (isMercenary () && isForeign (invader)) 
						GetComponent<MercenaryExplorer> ().activateEvent (invader);
				else
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

		public void missThisTurn ()
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
