using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class Player : MonoBehaviour
{

		public Color col;
		public Color fColor;
		public int flash = 1;
		float doubleClickStart = 0;
		public GameObject meepleSource;
		Collection<GameObject> eventsExperiencedThisTurn;
		public string id;
		public int moveableDesertExplorers;
		public bool hasMovedAnExplorerThisTurn = false;
		public bool hasRotatedATileThisTurn = false;
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
						if (movementPhase ()) 
								closeExplorerMovementAndEndTurn ();
						else
								GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
	
				}
				

		}

		bool movementPhase ()
		{
				return DesertMovementController.inMovementPhase;
		}

		public void closeExplorerMovementAndEndTurn ()
		{
				GameObject explorerThatThisPlayerWasMoving = wasMovingAnExplorer ();
				if (explorerThatThisPlayerWasMoving)
						makeMovingExplorerReactToMovementEnding (explorerThatThisPlayerWasMoving);
				else
						finishEndTurn ();

		}

		GameObject wasMovingAnExplorer ()
		{
				GameObject desert = GameObject.Find ("Desert");
				GameObject currentMovingObject = desert.GetComponent<DesertState> ().movingObject;
				if (currentMovingObject && desert.GetComponent<DesertState> ().movingObjectIsExplorer ()) {
						if (currentMovingObject.GetComponent<Meeple> ().player == gameObject)
								return currentMovingObject;
				}
				return null;
		}

		void makeMovingExplorerReactToMovementEnding (GameObject explorer)
		{

				explorer.GetComponent<DesertExplorer> ().reactToMovementEndingStayInDesert ();
					
		}

		bool wasMovingAnExplorer (GameObject currentMovingObject, GameObject desert)
		{

				if (currentMovingObject && desert.GetComponent<DesertState> ().movingObjectIsExplorer ()) 
						return (currentMovingObject.GetComponent<Meeple> ().player == gameObject);
				return false;

		}

		public void finishEndTurn ()
		{
				

				hasMovedAnExplorerThisTurn = false;

				hasRotatedATileThisTurn = false;

				updateWhetherCanMoveAgainThisRound ();
		
				GameObject.Find ("GameController").GetComponent<DesertMovementController> ().updatePlayer ();

		}

		void Start ()
		{
				col = GetComponent<SpriteRenderer> ().color;
				fColor.a = 255.0f;
				eventsExperiencedThisTurn = new Collection<GameObject> ();
				exploringMeeples = new Collection<GameObject> ();

				meepleSource.transform.position = this.transform.position + Vector3.right * 12;
				meepleSource.GetComponent<MeepleSource> ().Ready ();
				gameObject.GetComponent<PlayerInventory> ().waterText.transform.position = this.transform.position;

			
		}

		void Update ()
		{
				if (isPlayersTurn ())
						flashColor ();
				else
						GetComponent<SpriteRenderer> ().color = col;

	
				Debug.Log (id + " " + moveableDesertExplorers + " " + canMoveAgainThisRound + " " + exploringMeeples.Count ());

			
		}

		public bool isPlayersTurn ()
		{
				return GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs == gameObject ||
						GameObject.Find ("GameController").GetComponent<GameController> ().currentPlayer () == gameObject;
				

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

		public void returnRandomExplorerToSource ()
		{
				int randExplorer = (int)UnityEngine.Random.Range (0, exploringMeeples.Count);
				GameObject explorerToReturn = exploringMeeples.ElementAt (randExplorer);
				explorerToReturn.GetComponent<DesertExplorer> ().leaveCurrentTile ();
				explorerToReturn.GetComponent<Meeple> ().endExploration ();
		}

	    public void reactToEndOfMovePhase(){
		eventsExperiencedThisTurn.Clear ();
	}



}
