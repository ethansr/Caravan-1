using UnityEngine;
using System.Collections;

public class Meeple : MonoBehaviour
{
		public GameObject player;
		Vector3 defaultSize;
		Vector3 explorerSize;

		void Start ()
		{
				defaultSize = gameObject.GetComponent<Transform> ().localScale;
				explorerSize = defaultSize;
				explorerSize.x /= 1.5f;
				explorerSize.y /= 1.5f;
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
			
		}
	
		public void makeExplorer (GameObject bazaar, Vector3 positionInBazaar)
		{
				gameObject.GetComponent<Draggable> ().enabled = false;
				gameObject.GetComponent<DesertExplorer> ().enabled = true;
				gameObject.tag = "explorer";
				gameObject.GetComponent<DesertExplorer> ().currentTile = bazaar;
				gameObject.GetComponent<DesertExplorer> ().bazaarPosition = positionInBazaar;
				gameObject.GetComponent<Transform> ().localScale = explorerSize;
		 
		}

		public void endExploration ()
		{
				gameObject.GetComponent<Draggable> ().enabled = true;
				gameObject.GetComponent<DesertExplorer> ().enabled = false;
				//gameObject.tag="bazaarMeeple";
				gameObject.GetComponent<Transform> ().localScale = defaultSize;
		        gameObject.GetComponent<Draggable>().MoveLocations(null,player.GetComponent<Player>().meepleSource);

		}


	
}
