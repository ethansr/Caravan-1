using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropLocation : MonoBehaviour
{

		protected GameObject occupant;
		public bool allowMultipleOccupants;
	public string label = "placement";
		


		//returns true if this droplocation doesn't have an occupant already or if it allows multiple occupants
		protected virtual bool CanOccupy (GameObject potentialOccupant)
		{
				return (!occupant || allowMultipleOccupants);
		}
	
		void OnTriggerEnter2D (Collider2D other)
		{
				if (CanOccupy (other.gameObject)) {
						if (other.GetComponent<Draggable> ().enabled)
								other.gameObject.GetComponent<Draggable> ().SetDropLocation (gameObject);
				}
		}

		void OnTriggerExit2D (Collider2D other)
		{
				if (CanOccupy (other.gameObject)) {
						if (other.GetComponent<Draggable> ().enabled)
								other.gameObject.GetComponent<Draggable> ().SetDropLocation (null);

				}
		}

		public virtual void SetOccupant (GameObject o)
		{
		GameObject.Find ("GameController").GetComponent<GameController>().LogEvent(label);
				occupant = o;
		}

		public virtual bool Occupied(){
		return occupant ? true : false;
	}

	protected bool isMeepleOrNull (GameObject potentialOccupant)
	{
		return (potentialOccupant && potentialOccupant.GetComponent<Meeple> () || !potentialOccupant);
	}
	
	public virtual GameObject Occupant() {
		return occupant;
		}

}
