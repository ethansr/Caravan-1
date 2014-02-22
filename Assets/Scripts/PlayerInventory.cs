using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{

		//for testing
		public int availableWater = 10;
		public GUIText waterText;
		public GUIText goodsText;
		int[] amountOfEachGoodType = {0,0,0,0};
		Dictionary<DesertGenerator.GoodItem, int> amountOfEachGoodItem;
		public int wellDepth = 0;

	
		// Use this for initialization
		void Start ()
		{      
				amountOfEachGoodItem = new Dictionary<DesertGenerator.GoodItem,int> ();

				foreach (var value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {
						DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
						amountOfEachGoodItem.Add (goodItem, 0);
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
				waterText.transform.position = Camera.main.WorldToViewportPoint (transform.position);
				waterText.text = (availableWater).ToString ();

		}

		public bool waterAvailable ()
		{
				return availableWater > 0;
		}

		public void increaseGood (DesertGenerator.GoodItem good)
		{
				amountOfEachGoodType [(int)good]++;
				amountOfEachGoodItem [good] = amountOfEachGoodItem [good]++;

				Debug.Log (amountOfEachGoodItem [good].ToString () + " " + good.ToString ());
		}

		public bool hasGoods (DesertGenerator.GoodItem good, int numOf)
		{
				return amountOfEachGoodType [(int)good] >= numOf;
		}

		public void removeGoods (DesertGenerator.GoodItem good, int numOf)
		{       
				int newAmount = amountOfEachGoodItem [good] - numOf;//amountOfEachGoodType [(int)DesertGenerator.typeOfGoodItem(good)] - numOf;
				amountOfEachGoodItem [good] = newAmount;
				//also decrement num type of each good
				int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
				newAmount = amountOfEachGoodType [(int)DesertGenerator.typeOfGoodItem (good)] - numOf;
				amountOfEachGoodType [typeOfGoodItem] = newAmount;
		}

		public Collection<DesertGenerator.GoodItem> getAllGoodItemsOfType (DesertGenerator.GoodType goodType)
		{
				Collection<DesertGenerator.GoodItem> result = new Collection<DesertGenerator.GoodItem> ();
				foreach (var value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {
						DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
				

						if (DesertGenerator.typeOfGoodItem(goodItem) == goodType && amountOfEachGoodItem [goodItem] > 0)
							
								result.Add (goodItem);
				}
				return result;
		                                                                    

		}

		public void changeAvailableWater (int change)
		{
				//check new water
				int newWaterCount = availableWater + change;
				if (newWaterCount > -1)
						availableWater = newWaterCount;
		}
}
