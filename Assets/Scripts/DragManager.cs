using UnityEngine;
using System.Collections;

public class DragManager : MonoBehaviour
{
		Camera mainCamera;
		GameObject beingDragged;

		// Use this for initialization
		void Start ()
		{
				mainCamera = Camera.main;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (!Input.GetMouseButtonDown (0))
						return;

				int layer = LayerMask.NameToLayer ("Draggable");
				int layerMask = 1 << layer;

				RaycastHit2D hit = Physics2D.Raycast (mainCamera.ScreenToWorldPoint (Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);
				if (hit) {
						beingDragged = hit.collider.gameObject;

						if (beingDragged.GetComponent<Draggable> ().enabled) {
								if (isThisPlayersTurnToMove (beingDragged)) {

										if (beingDragged) {
												beingDragged.SendMessage ("StartDrag");
												StartCoroutine (Drag (hit.fraction));
										}
								}
						}
				}
		}

	//at present, this method only cares when we're dragging meeples. but we could change it depending on what else
	//we plan to be dragged
		bool isThisPlayersTurnToMove (GameObject beingDragged)
		{ if (isMeeple (beingDragged)) {
			return beingDragged.GetComponent<Meeple>().player.GetComponent<Player>().isPlayersTurn();

				} else
						return true;
		}

	bool isMeeple(GameObject beingDragged){
		return (beingDragged.GetComponent<Meeple> () != null);
		}

		IEnumerator Drag (float distance)
		{

				while (Input.GetMouseButton(0)) {
						Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
						Vector3 p = ray.GetPoint (distance);
						p.z = 0.0f;
						iTween.MoveUpdate (beingDragged, p, 0.5f);
						yield return null;
				}
				beingDragged.SendMessage ("StopDrag");
				beingDragged = null;
	
		}
}
