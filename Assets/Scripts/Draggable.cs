using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
	protected GameObject dropTarget;
	public GameObject currentLocation;
	protected int numActiveTargets = 0;
	public static float iTweenTime = 1.0f;
	
	//
	public void MoveLocations(GameObject source, GameObject target) {
		//if actually moving, erase ourselves from old location
		if (source != null) {
			source.GetComponent<DropLocation>().SetOccupant(null);
		}
		//always put ourselves in new location
		target.GetComponent<DropLocation>().SetOccupant(gameObject);
		currentLocation = target;
		iTween.MoveTo(gameObject, target.transform.position, iTweenTime);
		GameController controller = GameObject.Find ("GameController").GetComponent<GameController> ();
		if ( GameObject.Find ("GameController").GetComponent<GameController> ().currentPhase == "Placement" && controller.currentPlayer() == gameObject.GetComponent<Meeple>().player ) {
				controller.getNextPlayer ();
				}
	}


	//don't delete this; necessary for the send message method in dragmanager to function
	public void StartDrag(){
	}
	
	public void SetDropLocation(GameObject target) {
		//only set if valid new target or we are erasing the only target
		if (numActiveTargets == 1 || target) {
			dropTarget = target;
		}
		//need to keep track of how many active drop locations we're currently over
		//so that we don't erase valid new ones when leaving others
		if (target) {
			numActiveTargets++;
		} else {
			numActiveTargets--;
		}
	}

	public void StopDrag() {
		//move between targets, or move back to last location
		if (dropTarget) {
			MoveLocations(currentLocation, dropTarget);
		} else if (currentLocation != null) {
			MoveLocations(null, currentLocation);
		}
		numActiveTargets = 0;
	}
}
