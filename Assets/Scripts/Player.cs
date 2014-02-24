using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Player : MonoBehaviour
{

		Color col;
		public Color fColor;
		public int flash = 1;
		float doubleClickStart = 0;
		public GameObject meepleSource;
		Collection<GameObject> eventsExperiencedThisTurn;
		public string id;
		int moveableDesertExplorers;
		public bool hasMovedAnExplorerThisTurn = false;
		public bool canMoveAgainThisRound = true;
		public Collection<GameObject> exploringMeeples;

		void OnMouseUp ()
		{
				if ((Time.time - doubleClickStart) < 0.3f) {
						this.OnDoubleClick ();
						doubleClickStart = -1;
				} else {
						doubleClickStart = Time.time;
				}
		}

		//in general double click is how a player ends his turn early (ie before running out of wter)
		void OnDoubleClick ()
		{      
				if (isPlayersTurn ()) {
						endTurn ();
				} else {
						//this part will eventually be removed, since the controller will handle this all. 
						//we leabe it here now just for test purposes

						//GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (gameObject);
				}
		}

		public void endTurn ()
		{      //step 1: make meeple experience event
				makeMovingExplorerReactToMovementEnding ();
				//continue if there was no event; or when the event is finished
				

				
				

		}

		//cases: this player took his turn, but didn't actually set one of his guys to be moving
		//(so the over is null, another meeple, a tile). in that case this guy must finish end turn.
		//otherwise the turn ends with a meeple that was moving
		//so we need to experience the event and then when the evvent is finished then finishmovement
		//note that if the explorer returns to the bazaar then the mover is set tonull and so the reacttomoveneding wont be called
		void makeMovingExplorerReactToMovementEnding ()
		{
				GameObject desert = GameObject.Find ("Desert");
				GameObject currentMovingObject = desert.GetComponent<DesertState> ().movingObject;
				if (currentMovingObject && desert.GetComponent<DesertState> ().movingObjectIsExplorer ()) {
						if (currentMovingObject.GetComponent<Meeple> ().player == gameObject)
								currentMovingObject.GetComponent<DesertExplorer> ().reactToMovementEnding ();
						else 
								finishEndTurn ();
				} else
						finishEndTurn ();

		}

		public void finishEndTurn ()
		{
				eventsExperiencedThisTurn.Clear ();
				hasMovedAnExplorerThisTurn = false;
		
				updateWhetherCanMoveAgainThisRound ();
		
				GameObject.Find ("GameController").GetComponent<DesertMovementController> ().updatePlayer ();

		}

		void Start ()
		{
				col = GetComponent<SpriteRenderer> ().color;
				fColor.a = 255.0f;
				eventsExperiencedThisTurn = new Collection<GameObject> ();
				exploringMeeples = new Collection<GameObject> ();
		}

		void Update ()
		{
				if (isPlayersTurn ())
						flashColor ();
				else
						GetComponent<SpriteRenderer> ().color = col;
				//this should wok when people are placing thei explorers during the wp phase,
				//so that when the movement phase actually starts this will generally be true.
				//but for now I'll have the mercenary set it to true...
				//canMoveAgainThisRound = (moveableDesertExplorers > 0);
	
				Debug.Log (id + " " + moveableDesertExplorers + " " + canMoveAgainThisRound + " " + exploringMeeples.Count ());

				

		}

		public bool isPlayersTurn ()
		{
				return GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs == gameObject;

		}

		void flashColor ()
		{
				if (flash == 1)
						GetComponent<SpriteRenderer> ().color = fColor;
				else 
						GetComponent<SpriteRenderer> ().color = col;
				flash *= -1;
		}

		public void addEvent (GameObject eventExperienced)
		{
				eventsExperiencedThisTurn.Add (eventExperienced);
		}

		public bool alreadyExperiencedThisEventThisTurn (GameObject candidateEvent)
		{
				return eventsExperiencedThisTurn.Contains (candidateEvent);
		}
	   
		public void updateWhetherCanMoveAgainThisRound ()
		{
				canMoveAgainThisRound = (gameObject.GetComponent<PlayerInventory> ().waterAvailable () && moveableDesertExplorers > 0);

		}

		public void changeMovebleDesertExplorers (int change)
		{
				int newNumMoveables = moveableDesertExplorers + change;
				if (newNumMoveables > -1)
						moveableDesertExplorers = newNumMoveables;
				else
						moveableDesertExplorers = 0;
			
				updateWhetherCanMoveAgainThisRound ();
				
		}

		public void setMoveablesToZero ()
		{
				moveableDesertExplorers = 0;
				updateWhetherCanMoveAgainThisRound ();
			
		}

		void tellDesertControllerToGetNextPlayer ()
		{
				GameObject.Find ("GameController").GetComponent<DesertMovementController> ().updatePlayer ();
		}

		
	    
	  
		


}
