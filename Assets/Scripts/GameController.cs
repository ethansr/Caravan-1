﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject[] players;
	public int indexOfNextPlayer;
	public static int numPlayers = 4;
	public static int numMeeplesPerPlayer=5;

	// Use this for initialization
	void Start () {
	
		//for testing
		gameObject.GetComponent<DesertMovementController> ().beginDesertMovementPhase ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject getNextPlayer(){
		GameObject result= players[indexOfNextPlayer];
		indexOfNextPlayer+=(indexOfNextPlayer==numPlayers-1?-(numPlayers-1):1);
		return result;
	}
}
