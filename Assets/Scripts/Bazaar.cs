using UnityEngine;
using System.Collections;

public class Bazaar : DropLocation
{
	
	
	
		void Start ()
		{
				GetComponent<DesertTile> ().vp.n = 1;
				GetComponent<DesertTile> ().vp.s = 1;
				GetComponent<DesertTile> ().hp.w = 1;
				GetComponent<DesertTile> ().hp.e = 1;
				GetComponent<DesertTile> ().flipped = true;
		}
	
		public override void SetOccupant (GameObject o)
		{ 
				if (o && GetComponent<DesertTile> ().roomForMoreOccupants ()) {
						o.GetComponent<Meeple> ().makeExplorer (gameObject);

				}
		
		}
	
	
	
	
	
	
	
	
	
	
}
