using UnityEngine;
using System.Collections;

public class MerchantCard : MonoBehaviour {
	public DesertGenerator.GoodItem first_good;
	public DesertGenerator.GoodItem second_good;
	public DesertGenerator.GoodItem third_good;
	public GameObject first_position;
	public GameObject second_position;
	public GameObject third_position;
	public GameObject player;

	
	
	float doubleClickStart = 0;
	// Use this for initialization

	// Use this for initialization
	void Start () {
		SetGoods (first_good, second_good, third_good);
	}

	public void SetGoods(DesertGenerator.GoodItem first, DesertGenerator.GoodItem second, DesertGenerator.GoodItem third) {
		first_good = first;
		second_good = second;
		third_good = third;

		first_position.GetComponent<TradePlacement>().setGood(first_good);
		second_position.GetComponent<TradePlacement>().setGood(second_good);
		if (third_good == (DesertGenerator.GoodItem)(-1)) {
			Destroy (third_position);
		} else {
			third_position.GetComponent<TradePlacement>().setGood(third_good); 
		}

	}
	// Update is called once per frame
	void Update () {
	
	}



	
	void OnMouseUp ()
	{
		if ((Time.time - doubleClickStart) < 0.3f) {
			this.OnDoubleClick ();
			doubleClickStart = -1;
		} else {
			doubleClickStart = Time.time;
		}
	}
	
	//in general double click is how a player ends his turn early (ie before running out of wter)
	void OnDoubleClick ()
	{      
		GameObject player = GameObject.Find ("GameController").GetComponent<GameController> ().currentPlayer ();
		PlayerInventory inventory = player.GetComponent<PlayerInventory> ();

		//if (third_good == (DesertGenerator.GoodItem)(-1)) {

		if (inventory.amountOfEachGoodItem[first_good] >= 1 && inventory.amountOfEachGoodItem[second_good] >= 1) {
		inventory.AddVictoryPoint ();
		}
		
		//	}else {
		//	if (inventory.hasNumberOfGivenGoodItem(first_good,1) && inventory.hasNumberOfGivenGoodItem(second_good, 1) && inventory.hasNumberOfGivenGoodItem(third_good,1) )  {
		//			inventory.AddVictoryPoint ();
		//		}

		//	}
	}


}
