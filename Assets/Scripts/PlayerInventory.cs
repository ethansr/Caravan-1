using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{

		//for testing
		public int availableWater;
		public GUIText waterText;
		public GUIText goodsText;
		int[] amountOfEachGoodType = {0,0,0,0};
		public Dictionary<DesertGenerator.GoodItem, int> amountOfEachGoodItem;
		public int wellDepth = 0;
		public int victory_points = 0;
		public List<GameObject> merchantCards;
		public List<Vector3> merchantCardLocations = new List<Vector3> ();


		//set this to true as the effect of the "invasion" worker placement tile;
		//I handle setting it back to false after it takes effedct during movement phase
		public bool canInvade;
		public GameController controller;

	
		// Use this for initialization
		void Start ()
		{

				controller = GameObject.Find ("GameController").GetComponent<GameController> ();
				canInvade = false;

				amountOfEachGoodItem = new Dictionary<DesertGenerator.GoodItem,int> ();

				foreach (int value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {
						DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
						amountOfEachGoodItem.Add (goodItem, 0);
						
				}

				
				merchantCardLocations.Add (transform.position + Vector3.right * 35 + Vector3.up * 1.1f);
				merchantCardLocations.Add (transform.position + Vector3.right * 45 + Vector3.up * 1.1f);

		}
	  
		// Update is called once per frame
		void Update ()
		{
				waterText.transform.position = Camera.main.WorldToViewportPoint (transform.position);
				if (!Event.anEventIsHappeningInGeneral) {
						waterText.text = (availableWater).ToString () + "\n VP:" + victory_points.ToString () + "\n";
						bool every_other = false;
						foreach (int value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {

								DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
								string name = Enum.GetName (typeof(DesertGenerator.GoodItem), value);

								if (every_other) {
										waterText.text += "\n";
								}
								;
								every_other = !every_other;
								waterText.text += name + ": " + amountOfEachGoodItem [(DesertGenerator.GoodItem)value].ToString () + "\t";
						}
				}
		}

		public void AddCard (GameObject newCard)
		{

				merchantCards.Add (newCard);

				foreach (GameObject merchCard in merchantCards) {
						if (merchantCards.IndexOf (merchCard) < merchantCardLocations.Count) {
								iTween.MoveTo (merchCard, merchantCardLocations [merchantCards.IndexOf (merchCard)], 1.0f);
						}
				}

		}

		public void AddVictoryPoint ()
		{
				victory_points += 1;

		}

		public bool waterAvailable ()
		{
				return availableWater > 0;
		}

		public int howMuchWaterAvailable ()
		{
				return availableWater;
		}

		public void increaseGood (DesertGenerator.GoodItem good)
		{
				int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
				amountOfEachGoodType [(int)typeOfGoodItem]++;
				amountOfEachGoodItem [good] = amountOfEachGoodItem [good] + 1;
				Debug.Log (amountOfEachGoodType [typeOfGoodItem].ToString () + " " + DesertGenerator.typeOfGoodItem (good).ToString ());
				Debug.Log (amountOfEachGoodItem [good].ToString () + " " + good.ToString ());
		}

		public void increaseGood (DesertGenerator.GoodItem good, int numOf)
		{
				int newAmount = amountOfEachGoodItem [good] + numOf;
				amountOfEachGoodItem [good] = newAmount;
				//also decrement num type of each good
				int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
				newAmount = amountOfEachGoodType [(int)DesertGenerator.typeOfGoodItem (good)] + numOf;
				amountOfEachGoodType [typeOfGoodItem] = newAmount;
				Debug.Log (amountOfEachGoodItem [good].ToString () + " " + good.ToString ());

		}

		public bool hasGoodsOfGivenType (DesertGenerator.GoodType goodType, int numOf)
		{
				return amountOfEachGoodType [(int)goodType] >= numOf;
		}

		public bool hasGoods ()
		{
				foreach (int i in amountOfEachGoodType)
						if (i > 0)
								return true;
				return false;

		}

		public bool hasNumberOfGivenGoodItem (DesertGenerator.GoodItem good, int numOf)
		{

				return amountOfEachGoodItem [good] >= numOf;

		}

		public void removeGoods (DesertGenerator.GoodItem good, int numOf)
		{       
				int newAmount = amountOfEachGoodItem [good] - numOf;
				if (newAmount > -1) {
						amountOfEachGoodItem [good] = newAmount;
						//also decrement num type of each good
						int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
						newAmount = amountOfEachGoodType [(int)DesertGenerator.typeOfGoodItem (good)] - numOf;
						amountOfEachGoodType [typeOfGoodItem] = newAmount;
						Debug.Log (amountOfEachGoodItem [good].ToString () + " " + good.ToString ());
				}
		}

		public Collection<DesertGenerator.GoodItem> getAllGoodItemsOfType (DesertGenerator.GoodType goodType)
		{
				Collection<DesertGenerator.GoodItem> result = new Collection<DesertGenerator.GoodItem> ();
				foreach (var value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {
						DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
				

						if (DesertGenerator.typeOfGoodItem (goodItem) == goodType && amountOfEachGoodItem [goodItem] > 0)
							
								result.Add (goodItem);
				}
				return result;
		                                                                    

		}

		public void drainWater ()
		{
				availableWater = 0;

		}

		public void changeAvailableWaterDuringMovement (int change)
		{
				changeAvailableWaterDuringPlacementPhase (change);
				if (!waterAvailable ())
						gameObject.GetComponent<Player> ().closeExplorerMovementAndEndTurn ();
			
		}

		public void changeAvailableWaterDuringPlacementPhase (int change)
		{
				//check new water
				int newWaterCount = availableWater + change;
		
				if (newWaterCount > -1)
						availableWater = newWaterCount;
				else 
						newWaterCount = 0;


		}

		public void removeRandomGood ()
		{
				foreach (DesertGenerator.GoodItem goodItem in amountOfEachGoodItem.Keys) {
						if (amountOfEachGoodItem [goodItem] > 0) {
								removeGoods (goodItem, 1);
								return;
						}


				}

		}




}
