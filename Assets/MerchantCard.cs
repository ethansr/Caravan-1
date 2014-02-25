using UnityEngine;
using System.Collections;

public class MerchantCard : MonoBehaviour {
	public DesertGenerator.GoodItem first_good;
	public DesertGenerator.GoodItem second_good;
	public DesertGenerator.GoodItem third_good;
	public GameObject first_position;
	public GameObject second_position;
	public GameObject third_position;
	// Use this for initialization
	void Start () {
		first_position.GetComponent<TradePlacement>().setGood(first_good);
		second_position.GetComponent<TradePlacement>().setGood(second_good);
		if (third_good == null) {
			Destroy (third_position);
		} else {
			third_position.GetComponent<TradePlacement>().setGood(third_good); 
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
