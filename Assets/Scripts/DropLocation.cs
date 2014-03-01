using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropLocation : MonoBehaviour
{

		protected GameObject occupant;
		public bool allowMultipleOccupants;


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
				occupant = o;
		}

}
