using UnityEngine;
using System.Collections;

public class TradePlacement : DropLocation {
	public DesertGenerator.GoodItem good;

	// Use this for initialization
	void Start () {
		GameObject desert = GameObject.Find("Desert");

		GetComponent<SpriteRenderer> ().sprite = desert.GetComponent<DesertTileIndex> ().goodTileSprites [(int)good];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
