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
			movingObject.GetComponent<DesertExplorer> ().reactToMovementEnding ();

		movingObject = newMover;
	}

	//when the player whose turn it is changes any meeples belonging to the player who are moving 
	//must terinate their movement
	public void changePlayerWhoseTurnItIs(GameObject newPlayer){
		if (movingObject&&movingObjectIsExplorer()){
			 if(movingObject.GetComponent<Meeple>().player==playerWhoseTurnItIs)
				movingObject.GetComponent<DesertExplorer> ().reactToMovementEnding ();
		}
		//this is new; beware if there are bugs...
		 movingObject = null;

		if(playerWhoseTurnItIs) playerWhoseTurnItIs.GetComponent<Player> ().reactToTurnEnding ();

		playerWhoseTurnItIs = newPlayer;
	
	}

	bool movingObjectIsExplorer(){
		return movingObject.GetComponent<DesertExplorer> () != null;

	}
}
