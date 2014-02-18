using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {

	//for testing
	public int availableWater=10;
	public GUIText waterText;
	public GUIText goodsText;
	int[] amountOfEachGood = {0,0,0,0, 0,0,0,0, 0,0,0,0, 0,0,0,0};
	public int wellDepth = 0;

	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		waterText.transform.position = Camera.main.WorldToViewportPoint (transform.position);
		waterText.text = (availableWater).ToString ();

	}

	public void decreaseAvailableWater (){
		availableWater--;
	}

	public bool waterAvailable(){
				return availableWater > 0;
		}

	public void increaseGood(DesertGenerator.GoodType good){
		amountOfEachGood [(int)good]++;
		Debug.Log (amountOfEachGood [(int)good].ToString ());
	}

	public bool hasGoods(DesertGenerator.GoodType good, int numOf){
		return amountOfEachGood [(int)good] >= numOf;
		}

	public void removeGoods(DesertGenerator.GoodType good, int numOf){
		int newAmount = amountOfEachGood [(int)good] - numOf;
		amountOfEachGood [(int)good] = newAmount;
		}

	public void changeAvailableWater(int change){
				availableWater += change;
		}
}
