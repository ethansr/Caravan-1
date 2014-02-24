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

		void SpawnMeeple ()
		{
				if (numSpawned < numMeeples) {
						GameObject inst = (GameObject)Instantiate (meeple, transform.position, Quaternion.identity);
						setColor (inst);
						setPlayer (inst);
						numSpawned++;
						inst.GetComponent<Draggable> ().MoveLocations (null, gameObject);

				}
		}

		void setColor (GameObject meeple)
		{
				Color colour = meeple.GetComponent<SpriteRenderer> ().color;
				colour = GetComponent<SpriteRenderer> ().color;
				meeple.GetComponent<SpriteRenderer> ().color = colour;
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

		bool CanOccupy (GameObject potentialOccupant)
		{
				GameObject occupantPlayer = potentialOccupant.GetComponent<Meeple> ().player;
				return (occupantPlayer == player);
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
