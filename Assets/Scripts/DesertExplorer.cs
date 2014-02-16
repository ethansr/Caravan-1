﻿using UnityEngine;
using System.Collections;

public class DesertExplorer : MonoBehaviour
{
		public GameObject currentTile;
		float[] alphas = {0.0f, 255.0f};
		int alphindex = 0;
		GameObject lastGoodAcquired;
		GameObject lastEventExperienced;
		public Vector3 bazaarPosition;
		Vector3 currentPos;
		GameObject desert;
	
		void Start ()
		{
				desert = GameObject.Find ("Desert");
		}
	
		//assume that move was successful
		public void updateLocation (GameObject newLocation)
		{       //the other one is being made to move to the new location.
				//which is based on the move called relative to the (original) meeples position
				currentTile.GetComponent<DesertTile> ().leaveTile (gameObject, gameObject.GetComponent<Transform> ().position);
				if (returnedToBazaar (newLocation))
						returnToMeepleSource ();
				else
						moveToNewDesertTile (newLocation);
		}
	
		bool returnedToBazaar (GameObject newLocation)
		{
				return (newLocation.GetComponent<Bazaar> () != null);
		
		}
	
		void returnToMeepleSource ()
		{
		
				stopFlashing ();
				removeSelfFromDesertIfMoving ();
				gameObject.GetComponent<Meeple> ().endExploration ();
		
		}
		//assume that move was successful
		void moveToNewDesertTile (GameObject newLocation)
		{  
				currentTile = newLocation;
				currentPos = currentTile.GetComponent<DesertTile> ().enterTile (gameObject);
				currentPos.z = 1;
				GetComponent<Transform> ().position = currentPos;
		
		}
	
		void OnMouseUpAsButton ()
		{  
				if (isMyPlayersTurn () && currentTile) {
						if (isMover ()) { 
								stopFlashing ();
								removeSelfFromDesertIfMoving ();
								experienceEventIfHaventAlready ();
						} else {
								makeMover ();
						}     
				}

		}
	
		bool isMyPlayersTurn ()
		{
				return gameObject.GetComponent<Meeple> ().player.GetComponent<Player> ().isPlayersTurn ();
		}
	
		void removeSelfFromDesertIfMoving ()
		{
				if (isMover ()) 
						desert.GetComponent<DesertState> ().movingObject = null;
		}
	
		void Update ()
		{   
				if (moving ()) {
						flashToIndicateMoveState ();
						GameObject newLocation = getNewLocationGivenKeyInput ();
						if (moveSuccessful (newLocation)) 
								handleSuccessfulMove (newLocation);
				} else 
						stopFlashing ();
		
				maintainPosition ();
	
		
		}
		//I don't really understand why this is neccessary tbh, but it seems to be.
		void maintainPosition ()
		{
				if (currentTile && currentTile.GetComponent<Bazaar> () != null)
						GetComponent<Transform> ().position = bazaarPosition;
				else if (currentTile)
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
				if (!waterAvailable ())
						reactToMovementEnding ();
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
				Color c = GetComponent<SpriteRenderer> ().color;
				c.a = alphas [alphindex];
				GetComponent<SpriteRenderer> ().color = c;
				alphindex += (alphindex < 1 ? 1 : -1);
		}

		public void decreaseAvailableWater ()
		{
				gameObject.GetComponent<Meeple> ().player.GetComponent<PlayerInventory> ().decreaseAvailableWater ();

		}
	
		void stopFlashing ()
		{
				Color c = GetComponent<SpriteRenderer> ().color;
				c.a = alphas [1];
				GetComponent<SpriteRenderer> ().color = c;
		}
	
		bool isMover ()
		{
				return desert.GetComponent<DesertState> ().movingObject == gameObject;
		}
	
		void makeMover ()
		{
				//desert.GetComponent<DesertState> ().movingObject = gameObject;
				desert.GetComponent<DesertState> ().changeMover (gameObject);
		}
	
		public bool acceptInvader (GameObject invader)
		{     
				return (invader.GetComponent<Meeple> ().player == GetComponent<Meeple> ().player);
		}
	
		public void handleInvader (GameObject invader)
		{
				Debug.Log ("handle invader");
		}
	
	
	
	
	
}
