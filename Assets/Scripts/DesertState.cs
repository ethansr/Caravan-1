using UnityEngine;
using System.Collections;

public class DesertState : MonoBehaviour
{

		//either a deserttile (being rotated), or a desert explorer.
		//public GameObject movingObject;

		public GameObject movingObject;
		public GameObject movingExplorer;
		//public GameObject rotatingTile;
		public GameObject playerWhoseTurnItIs;

	    


		//when movement ends, if the mover was a desert explorer
		//force him to |end| movement on a tile.
		//(similar to the effect of double clicking)
		public void setMovingExplorer (GameObject newMover)
		{
				//closeMovementOfPreviousMovingExplorer ();
				if (movingObjectIsExplorer (newMover)) {
						movingExplorer = newMover;

						movingObject = newMover;
				}

				
				
		}

	   
		/*
		void closeMovementOfPreviousMovingExplorer ()
		{
				if (movingObject && movingObjectIsExplorer ()) 
						movingObject.GetComponent<DesertExplorer> ().reactToMovementEndingStayInDesert ();
		}
		*/

		public void makeATileRotate (GameObject rotatingDesertTile)
		{
				if (movingObjectIsTile (rotatingDesertTile))
				//if (rotatingDesertTile) {
				//rotatingTile = rotatingDesertTile;
						movingObject = rotatingDesertTile;
				//} 
		       
				
		}
		/*
		public void stopTileRotation ()
		{
				rotatingTile = null;
				if (movingExplorer)
						movingObject = movingExplorer;
		}
*/


		//when the player whose turn it is changes any meeples belonging to the player who are moving 
		//must terinate their movement
		public void changePlayerWhoseTurnItIs (GameObject newPlayer)
		{

				movingObject = null;
	

				playerWhoseTurnItIs = newPlayer;
	
		}
		/*
		public bool movingObjectIsExplorer ()
		{
				return movingObject.GetComponent<DesertExplorer> () != null;

		}
*/
		public bool movingObjectIsExplorer (GameObject newMover)
		{
				return newMover.GetComponent<DesertExplorer> () != null;
		}

		public bool movingObjectIsTile (GameObject newMover)
		{
				return newMover.GetComponent<DesertTile> () != null;
		}


}
