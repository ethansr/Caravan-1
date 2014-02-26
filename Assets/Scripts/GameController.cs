using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public GameObject[] players;
	public int indexOfNextPlayer;
	public static int numPlayers = 4;
	public static int numMeeplesPerPlayer=5;
	public Stack<GameObject> merchantCards = new Stack<GameObject>();

	public GameObject merchant_card;

	// Use this for initialization
	void Start () {
		BeginMovementPhase ();

		BuildDeck ();
	




	}

	void BeginMovementPhase(){
		gameObject.GetComponent<DesertMovementController> ().beginDesertMovementPhase ();

	}

	void BeginPlacementPhase() {
		//Do Nothing
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
			card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45;


			merchantCards.Push (card);
			
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
			card.transform.position = card.transform.position + Vector3.left * i * 8 + Vector3.right * 45 ;
			card.transform.position = card.transform.position + Vector3.down * 16;
			merchantCards.Push (card);
			
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignCardToPlayer(GameObject player) {
		// do nothing
	}

	public GameObject getNextPlayer(){
		GameObject result= players[indexOfNextPlayer];
		indexOfNextPlayer+=(indexOfNextPlayer==numPlayers-1?-(numPlayers-1):1);
		return result;
	}
}
