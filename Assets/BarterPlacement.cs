using UnityEngine;
using System.Collections;

public class BarterPlacement : DropLocation {

	void Start () {
		
	}
	

	protected override bool  CanOccupy (GameObject potentialOccupant)
	{
		print ("Tried");
		return potentialOccupant.GetComponent<GoodToken>() ? true : false ;
	}

	public virtual void SetOccupant (GameObject o)
	{
		occupant = o;

		if (o) {
			GameObject parent = gameObject.transform.parent.gameObject;
			parent.GetComponent<Barter>().AttemptToCompleteBarter();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
