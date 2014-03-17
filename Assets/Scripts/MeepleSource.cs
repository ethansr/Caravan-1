using UnityEngine;
using System.Collections;

//a meeple source is a drop location that can spawn meeples
public class MeepleSource : DropLocation
{

		public GameObject player;
		public int numMeeples;
		protected int numSpawned = 0;
		public GameObject meeple;
		public GUIText countText;
		public int meeplesOnSource;
		bool ready = false;

		void SpawnMeeple ()
		{
				if (ready && numSpawned < numMeeples) {
						GameObject inst = (GameObject)Instantiate (meeple, transform.position, Quaternion.identity);
						setColorAndID (inst);
						setPlayer (inst);
						inst.GetComponent<Meeple> ().id = "Meeple:" + numSpawned;
						numSpawned++;
						inst.GetComponent<Draggable> ().MoveLocations (null, gameObject);

				}
		}

		public void Ready ()
		{
				ready = true;
		}

		void setColorAndID (GameObject meeple)
		{
				Color colour = meeple.GetComponent<SpriteRenderer> ().color;
				colour = GetComponent<SpriteRenderer> ().color;
				meeple.GetComponent<SpriteRenderer> ().color = colour;

				meeple.GetComponent<Meeple> ().id = "Meeple_"+colour+"_"+ numSpawned;
		}

		void setPlayer (GameObject meeple)
		{
				meeple.GetComponent<Meeple> ().player = player;
		}

		void Start ()
		{      
				if (numMeeples > 1) {
						allowMultipleOccupants = true;
				}
				SpawnMeeple ();
				player.GetComponent<Player> ().meepleSource = gameObject;
		}
	
		void Update ()
		{
				countText.transform.position = Camera.main.WorldToViewportPoint (transform.position);
				
				meeplesOnSource = (occupant ? numMeeples - numSpawned + 1 : 0);
				countText.text = meeplesOnSource.ToString ();
				if (!occupant) {
						SpawnMeeple ();
				}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
				if (CanOccupy (other.gameObject)) {
						other.gameObject.GetComponent<Draggable> ().SetDropLocation (gameObject);
				}
		}

		protected override bool CanOccupy (GameObject potentialOccupant)
		{
				if (potentialOccupant.GetComponent<Meeple> ()) {
						GameObject occupantPlayer = potentialOccupant.GetComponent<Meeple> ().player;
						return (occupantPlayer == player);
				} else {
						return false;
				}
		}
	
		public override void SetOccupant (GameObject o)
		{
				if (o && occupant) {
						Object.Destroy (occupant);
						numSpawned--;
				}
				base.SetOccupant (o);
		}
}
