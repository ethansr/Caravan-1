﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour {

	public static bool testMeeplesSentBackToBazaarAfterFindingGood=true;

	public GameObject[] players;
	private int indexOfNextPlayer = 0;
	public static int numPlayers = 4;
	public static int numMeeplesPerPlayer=5;
	public Stack<GameObject> deck = new Stack<GameObject>();
	public GameObject merchant_card;
	public string currentPhase;

	public List<GameObject> public_cards = new List<GameObject>();

	public Stack<GameObject> disCards = new Stack<GameObject> ();

	public Vector3 deckLocation;

	public DateTime gameStartTime;
	public DateTime gameEndTime;

	public int firstPlayerIndex = 0;


	// Use this for initialization
	void Start () {
		gameStartTime = DateTime.Now;
		BuildDeck ();
		deckLocation = deck.Peek ().transform.position;
		ShuffleDeck ();

		DealPublicCards ();

	
		BeginPlacementPhase ();

	


	}


	public void BeginMovementPhase(){
		gameObject.GetComponent<DesertMovementController> ().beginDesertMovementPhase ();

	}

	public void BeginPlacementPhase() {
		currentPhase = "Placement";
		LogEvent("started!");
		indexOfNextPlayer = 0;

		foreach (Meeple meeple in GameObject.FindObjectsOfType<Meeple>()) {
						print (meeple);
						if (meeple.player) {
								Player player = meeple.player.GetComponent<Player> ();
								GameObject meeplesource = player.meepleSource;
								Draggable drag = meeple.gameObject.GetComponent<Draggable> ();
								if (drag.currentLocation && drag.currentLocation != meeplesource && !meeple.gameObject.CompareTag ("explorer")) {
										drag.MoveLocations (drag.currentLocation, meeplesource);
								}
						}
				}


	}

	public void EndPlacementPhase() {
		foreach (GameObject player in players) {
			PlayerInventory inv = player.GetComponent<PlayerInventory>();
			inv.availableWater += inv.wellDepth;
				}
		indexOfNextPlayer = 0;
		currentPhase = "Movement";
		BeginMovementPhase ();

	}
	public void DealPublicCards () {
		for (int i =0; i < 4; i++) {
			GameObject card = deck.Pop ();
			public_cards.Add(card);
			iTween.MoveTo (card, deckLocation +  Vector3.right * 27 + Vector3.left * i * 10 + Vector3.down * 43, 1.0f);
			string cardRecord="public card,"+card.GetComponent<MerchantCard> ().first_good + "," + card.GetComponent<MerchantCard> ().second_good + "," + card.GetComponent<MerchantCard> ().third_good + ",";
			LogEvent (cardRecord);
		
		}
	}
	public void ShuffleDeck() {
		//thanks http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp#answer-1262619
		System.Random rng = new System.Random(); 


		GameObject[] list = deck.ToArray ();
		int n = deck.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			GameObject value = list[k]; 
			list[k] = list[n];  
			list[n] = value;  
		}  

		n = deck.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			GameObject value = list[k]; 
			list[k] = list[n];  
			list[n] = value;  
		}  

		deck = new Stack<GameObject> (list);
		
	}


		void BuildDeck() {
	int numberOfTwoGoodCards = 24;
		int numberOfThreeGoodCards = 24;
		
		for (int i = 0; i < numberOfTwoGoodCards; i++) {
			int first_type = UnityEngine.Random.Range(0,4);
			int second_type;
			
			do {
				second_type = UnityEngine.Random.Range(0,4);
			} while(first_type == second_type);
			
			int first_good = first_type * 4 + UnityEngine.Random.Range(0,4);
			int second_good = second_type * 4 + UnityEngine.Random.Range(0,4);
			GameObject card = (GameObject)Instantiate(merchant_card);
			card.GetComponent<MerchantCard>().SetGoods((DesertGenerator.GoodItem)first_good,(DesertGenerator.GoodItem)second_good, (DesertGenerator.GoodItem)(-1));
			//card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45;


			deck.Push (card);
			
		}

		for (int i = 0; i < numberOfThreeGoodCards; i++) {
			int first_type = UnityEngine.Random.Range(0,4);
			int second_type;
			int third_type;
			
			do {
				second_type = UnityEngine.Random.Range(0,4);
			} while(first_type == second_type);
			do {
				third_type = UnityEngine.Random.Range(0,4);
			} while(first_type == second_type || second_type == third_type || first_type == third_type );
			
			int first_good = first_type * 4 + UnityEngine.Random.Range(0,4);
			int second_good = second_type * 4 + UnityEngine.Random.Range(0,4);
			int third_good = third_type * 4 + UnityEngine.Random.Range (0,4);
			GameObject card = (GameObject)Instantiate(merchant_card);
			card.GetComponent<MerchantCard>().SetGoods((DesertGenerator.GoodItem)first_good,(DesertGenerator.GoodItem)second_good, (DesertGenerator.GoodItem)third_good);
			//card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45 ;
			//card.transform.position = card.transform.position + Vector3.down * 16;
			deck.Push (card);
			
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignCardToPlayer(GameObject player, string message) {

		PlayerInventory inventory = player.transform.GetComponent<PlayerInventory> ();

		GameObject card = deck.Pop ();
		card.GetComponent<MerchantCard>().player = player;

		inventory.AddCard (card);
		message = message + card.GetComponent<MerchantCard> ().first_good + "," + card.GetComponent<MerchantCard> ().second_good + "," + card.GetComponent<MerchantCard> ().third_good + ",";
		LogEvent (message);
		
		// do nothing
	}

	public void ReturnPublicCardsToDeck() {
		foreach (GameObject card in public_cards) {
			deck.Push(card);
			iTween.MoveTo (card, deckLocation, 1.0f);
		}
		public_cards.Clear();

		}

	public void Discard (GameObject cardToRemove){
		if (public_cards.Contains(cardToRemove)) { public_cards.Remove(cardToRemove); }
		foreach (GameObject playerObject in players) {
			PlayerInventory inventory = playerObject.GetComponent<PlayerInventory>();
			if (inventory.merchantCards.Contains(cardToRemove)) { inventory.merchantCards.Remove(cardToRemove); }
		}
			disCards.Push(cardToRemove);
		}
	public GameObject currentPlayer() {
		return players[indexOfNextPlayer];
	}

	public GameObject getNextPlayer(){
		LogEvent ("end_turn");
		if (indexOfNextPlayer == 3) {
			indexOfNextPlayer = firstPlayerIndex;
		}
		else {

			indexOfNextPlayer += 1;
		}
		GameObject result= players[indexOfNextPlayer];
		LogEvent("start_turn");
		return result;
	}

	public void MakeFirstPlayer(GameObject player) {


		int index = 0;
		for (int i = 0; i < 4; i++) {
			if (players[i] == player) {
					index = i;
				break;
			}
		}

		indexOfNextPlayer = 0;

		switch(index) 
		{
		case 0:
				//do nothing
			break;
		case 1:
			players = new GameObject[] { players[1],players[2],players[3],players[0] };
			break;
		case 2:
			players = new GameObject[] { players[2],players[3],players[0],players[1] };
			break;
		case 3:
			players = new GameObject[] { players[3],players[0],players[1],players[2] };
			break;
		}

	}

	public void PlayerWon(Player player) {
		LogEvent ("winner");


	}

	//thanks http://stackoverflow.com/questions/5057567/how-to-do-logging-in-c
	public void LogEvent(string message) {
		// Write the string to a file.append mode is enabled so that the log
		// lines get appended to  test.txt than wiping content and writing the log

		PlayerInventory inv = currentPlayer ().GetComponent<PlayerInventory> ();

		if ((currentPhase == "Placement" || currentPhase == "Movement") && inv.ready) {
						string fileName = gameStartTime.ToString ("yyyyMMdd_hh_mm_ss") + ".csv";
						string logFileDirectory = "logs";
						string filePath = System.IO.Path.Combine (logFileDirectory, fileName);
						System.IO.StreamWriter file = new System.IO.StreamWriter (filePath, true);

						//thanks http://stackoverflow.com/questions/18757097/writing-data-into-csv-file
						List<string> elements = new List<string> ();
						elements.Add (DateTime.Now.ToString ("u"));
						elements.Add (DateTime.Now.Ticks.ToString ());
						elements.Add (message);



						//name
						elements.Add (currentPlayer ().name);

						//vp
						elements.Add (inv.victory_points.ToString ());

						//available_water
						elements.Add (inv.availableWater.ToString ());

						//well_depth
						elements.Add (inv.wellDepth.ToString ());

						//goods 0-15

						foreach (DesertGenerator.GoodItem goodItem in (DesertGenerator.GoodItem[])Enum.GetValues (typeof(DesertGenerator.GoodItem))) {
								elements.Add (inv.amountOfEachGoodItem [goodItem].ToString ());
						}


						//private_card_demand 1
						//pivate_card_demand 2
						//private_card_demand 3
						//pivate_card_demand 4
						//private_card_demand 5
						//pivate_card_demand 6
						//total_demand 0-15

						//magic carpet
						elements.Add (inv.hasMagicCarpetPower.ToString ());

						//invader
						elements.Add (inv.canInvade.ToString ());

						//meeple 1
						//meeple 2
						//meeple 3
						//meeple 4
						//meeple 5

						var csv = string.Join (",", elements.ToArray ());

						print (csv);
						file.WriteLine (csv);

						file.Close ();
				}
		}


		
}
