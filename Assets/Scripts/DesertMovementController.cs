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

		void endDesertMovementPhase ()
		{
				gameObject.GetComponent<WorkerPlacementController> ().enabled = true;
		}

		public void updatePlayer ()
		{
				GameObject nextPlayer = gameObject.GetComponent<GameController> ().getNextPlayer ();
				GameObject.Find ("Desert").GetComponent<DesertState> ().changePlayerWhoseTurnItIs (nextPlayer);

		}
}
