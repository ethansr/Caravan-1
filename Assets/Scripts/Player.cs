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
		//public int moveableDesertExplorers;
		int moveableDesertExplorers;
		public bool hasMovedAnExplorerThisTurn = false;
		public bool canMoveAgainThisRound = true;

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
						GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (gameObject);
				}
		}

		void endTurn ()
		{
				GameObject.Find ("GameController").GetComponent<DesertMovementController> ().updatePlayer ();

		}

		void Start ()
		{
				col = GetComponent<SpriteRenderer> ().color;
				eventsExperiencedThisTurn = new Collection<GameObject> ();
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
	
				Debug.Log (id + " " + moveableDesertExplorers + " " + canMoveAgainThisRound);
				

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

		public void reactToTurnEnding ()
		{
				eventsExperiencedThisTurn.Clear ();
				hasMovedAnExplorerThisTurn = false;
		}

		public bool alreadyExperiencedThisEventThisTurn (GameObject candidateEvent)
		{
				return eventsExperiencedThisTurn.Contains (candidateEvent);
		}

		public void changeMovebleDesertExplorers (int change)
		{
				int newNumMoveables = moveableDesertExplorers + change;
				if (newNumMoveables <= 0)
						setMoveablesToZero ();
				else { //its posible that we will dip to 0 and then go back to 1 again, because of the sequence of
			//events when players acquire a mercenary
						if (moveableDesertExplorers == 0)
								canMoveAgainThisRound = true;
						moveableDesertExplorers = newNumMoveables;
				}
				
		}

		public void setMoveablesToZero ()
		{
				moveableDesertExplorers = 0;
				preventFromTakingAnotherTurnThisRound ();
		}

		public void preventFromTakingAnotherTurnThisRound ()
		{

				canMoveAgainThisRound = false;

		}
	    
	  
		


}
