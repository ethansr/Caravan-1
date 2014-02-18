using UnityEngine;
using System.Collections;

public class Bucket : MonoBehaviour {
	public Player player;
	private PlayerInventory inventory;
	private Vector3 original_position;
	// Use this for initialization
	void Start () {
		inventory = player.GetComponent<PlayerInventory> ();
		original_position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position =  original_position + Vector3.down * inventory.wellDepth * 7;
	}
}
