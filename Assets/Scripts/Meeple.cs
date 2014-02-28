using UnityEngine;
using System.Collections;

public class Meeple : MonoBehaviour
{
		public GameObject player;
		Vector3 explorerSize;

		void Start ()
		{      
			
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
			
		}

		public void makeExplorer (GameObject startingLocation)
		{   
				explorerSize = new Vector3 (1 / 1.5f, 1 / 1.5f, 1);
				DesertExplorer.defaultSize = explorerSize;
				DesertExplorer.movingSize = DesertExplorer.defaultSize * 1.2f;
		       
	
				gameObject.GetComponent<Draggable> ().enabled = false;
				gameObject.GetComponent<DesertExplorer> ().enabled = true;
				gameObject.tag = "explorer";
				gameObject.GetComponent<Transform> ().localScale = explorerSize;
	
				gameObject.GetComponent<DesertExplorer> ().moveToNewDesertTile (startingLocation);
		        
				if (player && !gameObject.GetComponent<DesertExplorer> ().isMercenary ()) {
						//note: now the desert controller is responsible for at the start of the move phase
						//summing up all the meeples that were placed in the bazaar tile to the players available meeples to move
						//(these allso shouldnt be reset to 0 ever, given the mercenary sums them)
						
						player.GetComponent<Player> ().exploringMeeples.Add (gameObject);
			            
				}
	
		}

		public void endExploration ()
		{
				Vector3 defaultSize = new Vector3 (1, 1, 1);
				gameObject.GetComponent<Draggable> ().enabled = true;
				gameObject.GetComponent<DesertExplorer> ().hasMovedThisRound = false;
				gameObject.tag = "Untagged";
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
				gameObject.GetComponent<Transform> ().localScale = defaultSize;
				gameObject.GetComponent<Draggable> ().MoveLocations (null, player.GetComponent<Player> ().meepleSource);
				
				if (player && !gameObject.GetComponent<DesertExplorer> ().isMercenary ()) {
						player.GetComponent<Player> ().changeMovebleDesertExplorers (-1);
						player.GetComponent<Player> ().exploringMeeples.Remove (gameObject);
				}
		}

	    


	
}
