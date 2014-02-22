using UnityEngine;
using System.Collections;

public class Good : MonoBehaviour {

	public DesertGenerator.GoodItem good;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addGoodToPlayerInventory(GameObject player){
		player.GetComponent<PlayerInventory>().increaseGood(good);

	}
}
