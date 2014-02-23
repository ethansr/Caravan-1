using UnityEngine;
using System.Collections;

public class DesertMovementController : MonoBehaviour
{

		public void beginDesertMovementPhase ()
		{
				//disable workerPlacementController
				gameObject.GetComponent<WorkerPlacementController> ().enabled = false;
				//tell the desert who is the first player
				GameObject.Find ("Desert").GetComponent<DesertState> ().playerWhoseTurnItIs = gameObject.GetComponent<GameController> ().getNextPlayer ();
		}

		void endDesertMovementPhase ()
		{


				gameObject.GetComponent<WorkerPlacementController> ().enabled = true;
		}
}
