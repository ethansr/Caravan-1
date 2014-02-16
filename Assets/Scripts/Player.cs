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

		void OnMouseUp ()
		{
				if ((Time.time - doubleClickStart) < 0.3f) {
						this.OnDoubleClick ();
						doubleClickStart = -1;
				} else {
						doubleClickStart = Time.time;
				}
		}
	
		void OnDoubleClick ()
		{      
				
				GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (gameObject);
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
		}

		public bool alreadyExperiencedThisEventThisTurn (GameObject candidateEvent)
		{
				return eventsExperiencedThisTurn.Contains (candidateEvent);
		}


}
