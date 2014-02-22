﻿using UnityEngine;
using System.Collections;

public class MercenaryExplorer : MonoBehaviour {

	public GameObject sourceEvent;


	public void activateEvent(GameObject newExplorer){
		sourceEvent.GetComponent<Mercenary> ().pickDesiredGoodTypeGivenCurrentLocation (GetComponent<DesertExplorer>().currentTile);
		sourceEvent.GetComponent<Mercenary> ().reActivateEvent(newExplorer);
	}


}
