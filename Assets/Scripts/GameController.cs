using UnityEngine;
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

		deck = new Stack<GameObject> (list);
		
	}


		void BuildDeck() {
	int numberOfTwoGoodCards = 24;
		int numberOfThreeGoodCards = 24;
		
		for (int i = 0; i < numberOfTwoGoodCards; i++) {
			int first_type = UnityEngine.Random.Range(0,3);
			int second_type;
			
			do {
				second_type = UnityEngine.Random.Range(0,3);
			} while(first_type == second_type);
			
			int first_good = first_type * 4 + UnityEngine.Random.Range(0,3);
			int second_good = second_type * 4 + UnityEngine.Random.Range(0,3);
			GameObject card = (GameObject)Instantiate(merchant_card);
			card.GetComponent<MerchantCard>().SetGoods((DesertGenerator.GoodItem)first_good,(DesertGenerator.GoodItem)second_good, (DesertGenerator.GoodItem)(-1));
			//card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45;


			deck.Push (card);
			
		}

		for (int i = 0; i < numberOfThreeGoodCards; i++) {
			int first_type = UnityEngine.Random.Range(0,3);
			int second_type;
			int third_type;
			
			do {
				second_type = UnityEngine.Random.Range(0,3);
			} while(first_type == second_type);
			do {
				third_type = UnityEngine.Random.Range(0,3);
			} while(first_type == second_type || second_type == third_type || first_type == third_type );
			
			int first_good = first_type * 4 + UnityEngine.Random.Range(0,3);
			int second_good = second_type * 4 + UnityEngine.Random.Range(0,3);
			int third_good = third_type * 4 + UnityEngine.Random.Range (0,3);
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

	public void AssignCardToPlayer(GameObject player) {

		PlayerInventory inventory = player.transform.GetComponent<PlayerInventory> ();

		GameObject card = deck.Pop ();
		card.GetComponent<MerchantCard>().player = player;

		inventory.AddCard (card);
		
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
			indexOfNextPlayer = 0 ;
		}
		else {

			indexOfNextPlayer += 1;
		}
		GameObject result= players[indexOfNextPlayer];
		LogEvent("start_turn");
		return result;
	}

	//thanks http://stackoverflow.com/questions/5057567/how-to-do-logging-in-c
	public void LogEvent(string message) {
		// Write the string to a file.append mode is enabled so that the log
		// lines get appended to  test.txt than wiping content and writing the log
		/*
		string fileName = gameStartTime.ToString("yyyyMMdd_hh_mm_ss") + ".csv";
		string logFileDirectory = "logs";
		string filePath = System.IO.Path.Combine(logFileDirectory,fileName);
		System.IO.StreamWriter file = new System.IO.StreamWriter( filePath,true);

		//thanks http://stackoverflow.com/questions/18757097/writing-data-into-csv-file

		var csv = string.Format ("{0},{1},{2},{3}", DateTime.Now.ToString ("u"), DateTime.Now.Ticks, currentPlayer().name, message);
		print (csv);
		file.WriteLine(csv);

		file.Close();
		*/
		}
		
}
