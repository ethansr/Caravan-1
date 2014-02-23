using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Bazaar : DropLocation
{
		public Dictionary<GameObject,Vector3> playersToPositions;
	
		void Start ()
		{
				GetComponent<DesertTile> ().vp.n = 1;
				GetComponent<DesertTile> ().vp.s = 1;
				GetComponent<DesertTile> ().hp.w = 1;
				GetComponent<DesertTile> ().hp.e = 1;
				GetComponent<DesertTile> ().flipped = true;
				GetComponent<DesertTile> ().maxAllowedOccupants = 20;

				playersToPositions = null;

		}
	
		public override void SetOccupant (GameObject o)
		{
				setupPlayerPositionDictionaryIfNecessary ();
				if (o && GetComponent<DesertTile> ().roomForMoreOccupants ()) {
						o.GetComponent<Meeple> ().makeExplorer (gameObject);

				}
		
		}

		void setupPlayerPositionDictionaryIfNecessary ()
		{
				if (playersToPositions == null) {
						playersToPositions = new Dictionary<GameObject,Vector3> ();
						GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
						int playerIndex = 0;
						foreach (Vector3 pos in GetComponent<DesertTile>().availablePositions) {
								GameObject player = players [playerIndex];
								playersToPositions.Add (player, pos);
								playerIndex++;
						}
				}
		}

		public Vector3 getPositionForPlayer (GameObject player)
		{
				return playersToPositions [player];




		}
	
	
	
	
	
	
	
	
	
	
}
