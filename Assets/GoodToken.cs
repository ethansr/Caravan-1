using UnityEngine;
using System.Collections;

public class GoodToken : MonoBehaviour {

	public GameController controller;
	public Player player;
	public DesertGenerator.GoodItem good;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
