using UnityEngine;
using System.Collections;

public class Meeple : MonoBehaviour
{
		public GameObject player;

		void Start ()
		{      
			
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
			
		}

		public void makeExplorer (GameObject startingLocation)
		{   
				Vector3 explorerSize = new Vector3 (1 / 1.5f, 1 / 1.5f, 1);

				gameObject.GetComponent<Draggable> ().enabled = false;
				gameObject.GetComponent<DesertExplorer> ().enabled = true;
				gameObject.tag = "explorer";
				gameObject.GetComponent<Transform> ().localScale = explorerSize;
				gameObject.GetComponent<DesertExplorer> ().moveToNewDesertTile (startingLocation);
		        
				if (player)
						player.GetComponent<Player> ().moveableDesertExplorers++;
	
		}

		public void endExploration ()
		{
				Vector3 defaultSize = new Vector3 (1, 1, 1);
				gameObject.GetComponent<Draggable> ().enabled = true;
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
				gameObject.GetComponent<Transform> ().localScale = defaultSize;
				gameObject.GetComponent<Draggable> ().MoveLocations (null, player.GetComponent<Player> ().meepleSource);
				if (player)
						player.GetComponent<Player> ().moveableDesertExplorers--;
		}

	    


	
}
