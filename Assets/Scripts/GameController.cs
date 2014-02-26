using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject[] players;
	private int indexOfNextPlayer = 3;
	public static int numPlayers = 4;
	public static int numMeeplesPerPlayer=5;
	public Stack<GameObject> deck = new Stack<GameObject>();
	public GameObject merchant_card;
	public string currentPhase;





	// Use this for initialization
	void Start () {

		BuildDeck ();
		ShuffleDeck ();

		DealInitialCards ();
		BeginPlacementPhase ();
	
		//BeginMovementPhase ();

	}


	public void BeginMovementPhase(){
		gameObject.GetComponent<DesertMovementController> ().beginDesertMovementPhase ();

	}

	public void BeginPlacementPhase() {
		currentPhase = "Placement";
		indexOfNextPlayer = 0;

		//getNextPlayer ().GetComponent<Player> ().isPlayersTurn ();
	}

	public void EndPlacementPhase() {
		indexOfNextPlayer = 0;
		currentPhase = "Movement";
		BeginMovementPhase ();

	}
	void DealInitialCards () {
		for (int i =0; i < 4; i++) {

						iTween.MoveTo (deck.Pop (), transform.position +  Vector3.right * 10 + Vector3.left * i * 10 + Vector3.up * 10, 1.0f);
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
			int first_type = Random.Range(0,3);
			int second_type;
			
			do {
				second_type = Random.Range(0,3);
			} while(first_type == second_type);
			
			int first_good = first_type * 4 + Random.Range(0,3);
			int second_good = second_type * 4 + Random.Range(0,3);
			GameObject card = (GameObject)Instantiate(merchant_card);
			card.GetComponent<MerchantCard>().SetGoods((DesertGenerator.GoodItem)first_good,(DesertGenerator.GoodItem)second_good, (DesertGenerator.GoodItem)(-1));
			//card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45;


			deck.Push (card);
			
		}

		for (int i = 0; i < numberOfThreeGoodCards; i++) {
			int first_type = Random.Range(0,3);
			int second_type;
			int third_type;
			
			do {
				second_type = Random.Range(0,3);
			} while(first_type == second_type);
			do {
				third_type = Random.Range(0,3);
			} while(first_type == second_type || second_type == third_type || first_type == third_type );
			
			int first_good = first_type * 4 + Random.Range(0,3);
			int second_good = second_type * 4 + Random.Range(0,3);
			int third_good = third_type * 4 + Random.Range (0,3);
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

		iTween.MoveTo(card, player.transform.position + Vector3.right * 35, 1.0f);

		
		// do nothing
	}

	public GameObject currentPlayer() {
		return players[indexOfNextPlayer];
	}

	public GameObject getNextPlayer(){
		if (indexOfNextPlayer == 3) {
			indexOfNextPlayer = 0 ;
		}
		else {

			indexOfNextPlayer += 1;
		}
		GameObject result= players[indexOfNextPlayer];

		return result;
	}
}
