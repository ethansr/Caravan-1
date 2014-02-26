using UnityEngine;
using System.Collections;

public class Merchant : Event {


	public override void activateEvent (GameObject desertExplorer){
		Debug.Log ("merchant");
		explorer = desertExplorer;
		tellPlayerToFinishEndTurn();
		
	}

	protected override void takeEffect(){
		GameController controller = GameObject.Find ("GameController").GetComponent<GameController>();
		controller.AssignCardToPlayer(explorer.GetComponent<Meeple>().player);
		/*
		 * game controller get card 
		 * give card to player private stash
		 *
		 * 
		 * 
		*/
	}
}
