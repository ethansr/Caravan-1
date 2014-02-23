using UnityEngine;
using System.Collections;

public class DesertMovementController : MonoBehaviour
{


		public void beginDesertMovementPhase ()
		{
				//disable workerPlacementController
				gameObject.GetComponent<WorkerPlacementController> ().enabled = false;
				
				//tell the desert who is the first player
				updatePlayer ();
				
		}

		bool noPlayersCanMakeMove ()
		{
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				bool result = true;
				foreach (GameObject player in players) {
						bool thisPlayerCanMoveAgain = player.GetComponent<Player> ().canMoveAgainThisRound;
						if (thisPlayerCanMoveAgain)
								return false;

				}
				return result;
			

		}

		void endDesertMovementPhase ()
		{
				Debug.Log ("End movement phase");
				//gameObject.GetComponent<WorkerPlacementController> ().enabled = true;
		}

		public void updatePlayer ()
	{      Debug.Log ("called");
				if (noPlayersCanMakeMove ())
						endDesertMovementPhase ();
				else { 
						int safety = 0;
						GameObject nextPlayer;
						do {
								nextPlayer = gameObject.GetComponent<GameController> ().getNextPlayer ();
								safety++;
								if (safety == 20) {
										Debug.Log (safety);
										return;
								}
						} while (!nextPlayer.GetComponent<Player>().canMoveAgainThisRound);

						GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (nextPlayer);
				}
		}


	   
}
