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
		public Dictionary<DesertGenerator.GoodItem, List<GameObject>> goodTokens = new Dictionary<DesertGenerator.GoodItem, List<GameObject>>();

		public int wellDepth = 0;
		public int victory_points = 0;
		public List<GameObject> merchantCards;
		public List<Vector3> merchantCardLocations = new List<Vector3> ();
		GameObject draggableGoodPrefab;


		//set this to true as the effect of the "invasion" worker placement tile;
		//I handle setting it back to false after it takes effedct during movement phase
		public bool canInvade;
		public bool hasMagicCarpetPower;
		public GameObject playersMagicCarpetToken;
		public GameObject playersInvaderToken;
		public GameController controller;
		Vector3 goodOrigin;



	
		// Use this for initialization
		void Start ()
		{

				controller = GameObject.Find ("GameController").GetComponent<GameController> ();
				canInvade = false;
				hasMagicCarpetPower = false;

				draggableGoodPrefab = GameObject.Find ("DraggableGood");

				amountOfEachGoodItem = new Dictionary<DesertGenerator.GoodItem,int> ();

				foreach (int value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {
						DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)value;
						amountOfEachGoodItem.Add (goodItem, 0);	
						goodTokens.Add (goodItem, new List<GameObject>());
			increaseGood(goodItem,1);
				}
				draggableGoodPrefab = GameObject.Find ("DraggableGood");
				//Vector3 goodOrigin = gameObject.GetComponent<Player> ().transform.position + Vector3.down * 6.8f + Vector3.right * 5.5f;

				//GoodTokens ();
				
				merchantCardLocations.Add (transform.position + Vector3.right * 35 + Vector3.up * 1.1f);
				merchantCardLocations.Add (transform.position + Vector3.right * 45 + Vector3.up * 1.1f);


			
		}
	  
		void GoodTokens ()
		{

				Vector3 goodOrigin = gameObject.GetComponent<Player> ().transform.position + Vector3.down * 6.8f + Vector3.right * 5.5f;
				for (int type = 0; type < 4; type++) {
						for (int offset = 0; offset < 4; offset++) {
								DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)(type * 4 + offset);
								addGoodToken (goodItem);
						}

				}
		}

		public void addGoodToken (DesertGenerator.GoodItem goodItem)
		{
				addGoodToken (goodItem, gameObject);
		}

		public void addGoodToken (DesertGenerator.GoodItem goodItem, GameObject source)
		{
				Vector3 goodOrigin = gameObject.GetComponent<Player> ().transform.position + Vector3.down * 6.8f + Vector3.right * 5.5f;

				int goodOffset = (int)goodItem % 4;
				int goodType = (int)goodItem / 4;
				//DesertGenerator.GoodItem goodItem = (DesertGenerator.GoodItem)(type * 3 + offset);
				GameObject desert = GameObject.Find ("Desert");
			
				GameObject tokenObject = (GameObject)Instantiate (draggableGoodPrefab);
				goodTokens [goodItem].Add (tokenObject);
				GoodToken token = tokenObject.GetComponent<GoodToken> ();
				// SpriteRenderer tokenBackground = tokenObject.transform.FindChild ("TokenBackground").GetComponentInChildren (SpriteRenderer); 
				token.player = gameObject.GetComponent<Player> ();
				colorTokenByPlayer (tokenObject);
				token.good = goodItem;
				tokenObject.transform.position = source.transform.position;

				iTween.MoveTo (tokenObject, goodOrigin + Vector3.right * 3 * goodOffset + Vector3.down * 3 * goodType, 2.0f);
				//tokenObject.transform.position = goodOrigin + Vector3.right * 3 * goodOffset + Vector3.down * 3 * goodType;

				tokenObject.GetComponent<SpriteRenderer> ().sprite = desert.GetComponent<DesertTileIndex> ().goodTileSprites [(int)token.good];
		}
		//foreach (DesertGenerator.GoodItem value in Enum.GetValues(typeof(DesertGenerator.GoodItem))) {

		//			GameObject tokenObject = (GameObject)Instantiate(draggableGoodPrefab);
		//			GoodToken token = tokenObject.GetComponent<GoodToken>();
		//
		//			if (hasNumberOfGivenGoodItem(value, 1)) {
						
		//		GameObject tokenObject = (GameObject)Instantiate(draggableGoodPrefab);
		//		GoodToken token = tokenObject.GetComponent<GoodToken>();
		//	}			
		//		}
		

		void colorTokenByPlayer (GameObject tokenObject)
		{
				SpriteRenderer tokenBackground = tokenObject.GetComponentsInChildren <SpriteRenderer> ()[1]; 
				tokenBackground.color = tokenObject.GetComponent<GoodToken> ().player.GetComponent<Player> ().col;
				
		}

		// Update is called once per frame
		void Update ()
		{
				waterText.transform.position = Camera.main.WorldToViewportPoint (transform.position);
				waterText.text = (availableWater).ToString () + "\n VP:" + victory_points.ToString () + "\n";
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

			if (victory_points == 4) {
	
			controller.PlayerWon(gameObject.GetComponent<Player>());
				}

		}

		public bool waterAvailable ()
		{
				return availableWater > 0;
		}

		public int howMuchWaterAvailable ()
		{
				return availableWater;
		}

		public void increaseGood (DesertGenerator.GoodItem good, GameObject source)
		{
				int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
				amountOfEachGoodType [(int)typeOfGoodItem]++;
				amountOfEachGoodItem [good] = amountOfEachGoodItem [good] + 1;
				Debug.Log (amountOfEachGoodType [typeOfGoodItem].ToString () + " " + DesertGenerator.typeOfGoodItem (good).ToString ());
				Debug.Log (amountOfEachGoodItem [good].ToString () + " " + good.ToString ());
				addGoodToken (good, source);

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
				
				for (int i = 0; i < numOf; i++) {
						addGoodToken (good);
				}

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
		for (int i = 0; i < numOf; i++) {
						removeGoods (good, numOf, goodTokens [good].First ());
				}
		}

		public void removeGoods (DesertGenerator.GoodItem good, int numOf, GameObject token)
		{       
				int newAmount = amountOfEachGoodItem [good] - numOf;
				if (newAmount > -1) {
						amountOfEachGoodItem [good] = newAmount;
						//also decrement num type of each good
						int typeOfGoodItem = (int)DesertGenerator.typeOfGoodItem (good);
						newAmount = amountOfEachGoodType [(int)DesertGenerator.typeOfGoodItem (good)] - numOf;
						amountOfEachGoodType [typeOfGoodItem] = newAmount;

						goodTokens[good].Remove(token);
						UnityEngine.Object.Destroy(token);

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
