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
								endTurn ();
						else
								GameObject.Find ("GameController").GetComponent<GameController> ().getNextPlayer ();
	
				}
				

		}

		bool movementPhase ()
		{
				return (GameObject.Find ("GameController").GetComponent<DesertMovementController> ().inMovementPhase);
		}

		public void endTurn ()
		{
				GameObject desert = GameObject.Find ("Desert");
				GameObject currentMovingObject = desert.GetComponent<DesertState> ().movingObject;
		        
				if (wasMovingAnExplorer (currentMovingObject, desert))
						makeMovingExplorerReactToMovementEnding (currentMovingObject);
				else
						finishEndTurn ();

		}

		//cases: this player took his turn, but didn't actually set one of his guys to be moving
		//(so the over is null, another meeple, a tile). in that case this guy must finish end turn.
		//otherwise the turn ends with a meeple that was moving
		//so we need to experience the event and then when the evvent is finished then finishmovement
		//note that if the explorer returns to the bazaar then the mover is set tonull and so the reacttomoveneding wont be called
		void makeMovingExplorerReactToMovementEnding (GameObject explorer)
		{

				explorer.GetComponent<DesertExplorer> ().reactToMovementEnding ();
					
		}

		bool wasMovingAnExplorer (GameObject currentMovingObject, GameObject desert)
		{

				if (currentMovingObject && desert.GetComponent<DesertState> ().movingObjectIsExplorer ()) 
						return (currentMovingObject.GetComponent<Meeple> ().player == gameObject);
				return false;

		}

		public void finishEndTurn ()
		{
				eventsExperiencedThisTurn.Clear ();

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

				foreach (int good in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {

				}
			
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
				//return GameObject.Find ("GameController").GetComponent<GameController> ().currentPlayer () == gameObject;

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

		
	    
	  
		


}
