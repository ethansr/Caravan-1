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
						currentTile.GetComponent<DesertTile> ().leaveTile (gameObject, gameObject.GetComponent<Transform> ().position);

				//...not actually equivalent to if(returned to bazaar and not mercenary, note).
				if (returnedToBazaar (newLocation)) {  
						if (!isMercenary ())
								returnToSource ();
						//(else, if the mercenary tries to move to bazaar, don't move).
				} else
						moveToNewDesertTile (newLocation);

		}
	
		bool returnedToBazaar (GameObject newLocation)
		{
				return (newLocation.GetComponent<Bazaar> () != null);
		
		}
	
		void returnToSource ()
		{
				stopFlashing ();
				
				tellDesertControllerToGetNextPlayer ();
				gameObject.GetComponent<Meeple> ().endExploration ();
		
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
	
		void Update ()
		{          
				if (moving ()) {
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
				experienceEventIfHaventAlready ();
				preventThisExplorerFromMovingAgainThisRound ();
				decrementPlayersMoveableExplorers ();
				
		}

		void preventThisExplorerFromMovingAgainThisRound ()
		{
				hasMovedThisRound = true;
		}

		void decrementPlayersMoveableExplorers ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().changeMovebleDesertExplorers (-1);
		
		}

		void experienceEventIfHaventAlready ()
		{
				if (currentTile.GetComponent<DesertTile> ().hasDesertEvent ()) {
						GameObject newEvent = currentTile.GetComponent<DesertTile> ().getEvent ();
						if (newEvent != lastEventExperienced && !myPlayerHasExperiencedEvent (newEvent)) {
								newEvent.GetComponent<Event> ().activateEvent (gameObject);
								lastEventExperienced = newEvent;
								updatePlayersEvents (newEvent);
				      
						}
				}
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
						if (newLocation == lastGoodAcquired)
								return; 
						addGoodToPlayerInventory (newLocation);
						lastGoodAcquired = newLocation;
			
				} else 
						updateLocation (newLocation);
		
				decreaseAvailableWater ();
				//end movement if out of water.

				if (!waterAvailable ()) {
						tellDesertControllerToGetNextPlayer ();

				}
		}

		void tellDesertControllerToGetNextPlayer ()
		{
				GameObject.Find ("GameController").GetComponent<DesertMovementController> ().updatePlayer ();
		}

		void setThisPlayersMoveableMeeplesToZero ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().setMoveablesToZero ();
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
				gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().changeAvailableWater (-1);

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
				if (!isMercenary ())
						return (!isForeign (invader));
                
				return true;
		}
	
		public void handleInvader (GameObject invader)
		{      

				if (isMercenary () && isForeign (invader)) 
						GetComponent<MercenaryExplorer> ().activateEvent (invader);
			

		}

		public bool isMercenary ()
		{
				return GetComponent<MercenaryExplorer> () != null;

		}

		bool isForeign (GameObject invader)
		{
				
				return !(invader.GetComponent<Meeple> ().player.Equals (GetComponent<Meeple> ().player));
		}
	
	
	
	
	
}
