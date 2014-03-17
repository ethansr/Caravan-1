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
				if (movingObjectIsTile (movingObject)) {
						GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("End rotation " + getRotatingTileInformation ());

				}
				if (movingObjectIsExplorer (newMover)) {
						movingExplorer = newMover;

						movingObject = newMover;
						GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("Set Explorer To Move " + newMover.GetComponent<Meeple> ().id);
				}


				
				
		}

		public void makeATileRotate (GameObject rotatingDesertTile)
		{
				if (movingObjectIsTile (rotatingDesertTile)) {

						movingObject = rotatingDesertTile;

						GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("Set tile to rotate " + getRotatingTileInformation ());


				}
			
		       
				
		}

		string getRotatingTileInformation ()
		{
				int x = movingObject.GetComponent<DesertTile> ().rp.x;
				int y = movingObject.GetComponent<DesertTile> ().rp.y;
				int n = movingObject.GetComponent<DesertTile> ().vp.n;
				int s = movingObject.GetComponent<DesertTile> ().vp.s;
				int e = movingObject.GetComponent<DesertTile> ().hp.e;
				int w = movingObject.GetComponent<DesertTile> ().hp.w;
				return "x: " + x + " y: " + y + " paths " + n + "" + s + "" + e + "" + w;

		}




		//when the player whose turn it is changes any meeples belonging to the player who are moving 
		//must terinate their movement
		public void changePlayerWhoseTurnItIs (GameObject newPlayer)
		{

				movingObject = null;
	

				playerWhoseTurnItIs = newPlayer;
				GameObject.Find ("GameController").GetComponent<GameController> ().LogEvent ("Update Player " + newPlayer.GetComponent<Player> ().id);
	
		}

		public bool movingObjectIsExplorer (GameObject newMover)
		{
				return newMover.GetComponent<DesertExplorer> () != null;
		}

		public bool movingObjectIsTile (GameObject newMover)
		{
				return (newMover&&newMover.GetComponent<DesertTile> () != null);
		}


}
