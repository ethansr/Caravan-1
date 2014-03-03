using UnityEngine;
using System.Collections;

public class BarterPlacement : DropLocation {

	public Barter barter;

	void Start () {
		
	}
	

	protected override bool  CanOccupy (GameObject potentialOccupant)
	{
		GoodToken token = potentialOccupant.GetComponent<GoodToken> ();
		if (token) {
						return barter.ShouldAllowPlacement (token, this);
				} else {
			return false;
				}
	}

	public override void SetOccupant (GameObject o)
	{
		occupant = o;

		if (o) {
			barter.AttemptToCompleteBarter();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
