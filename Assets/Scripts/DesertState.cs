using UnityEngine;
using System.Collections;

public class DesertState : MonoBehaviour {

	//either a deserttile (being rotated), or a desert explorer.
	public GameObject movingObject;
	public GameObject playerWhoseTurnItIs;


	//when movement ends, if the mover was a desert explorer
	//force him to |end| movement on a tile.
	//(similar to the effect of double clicking)
   public void changeMover(GameObject newMover){
		//close movement of the movingObject.
		//atm is an unfortunate if else
		if (movingObject&&movingObjectIsExplorer())
			movingObject.GetComponent<DesertExplorer> ().reactToMovementEndingStayInDesert ();

		movingObject = newMover;
	}

	//when the player whose turn it is changes any meeples belonging to the player who are moving 
	//must terinate their movement
	public void changePlayerWhoseTurnItIs(GameObject newPlayer){

		 movingObject = null;
	

		playerWhoseTurnItIs = newPlayer;
	
	}

	public bool movingObjectIsExplorer(){
		return movingObject.GetComponent<DesertExplorer> () != null;

	}
}
