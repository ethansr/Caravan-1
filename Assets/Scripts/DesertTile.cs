using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class DesertTile : MonoBehaviour
{       
	
		public bool hasEvent;
		public GameObject desertEvent;
		public bool flipped = false;
		GameObject playerWhoIsRotatingTile = null;
		int tileSideLength = DesertGenerator.tileSide;
		int eventMarkerSideLength = DesertGenerator.eventMarkerSide;
	
		//-1 if has no adjacent good.
		public int adjGoodLocation;
		public GameObject adjGood;
		public const int maxAllowedOccupants = 4;
		Collection<Vector3> availablePositions;
		public GameObject headOccupant;
		Collection<GameObject> occupants;
		GameObject desert;
	
	
		//factor this out into a custom sprite renderer 
	
		float[] alphas = {0.0f, 255.0f};
		int alphindex = 0;
		Color defaultCol;
	
	
	
		public struct RelativePosition
		{
				public int y;
				public int x;
		}
	
		public struct VerticalPaths
		{
				public int n;
				public int s;
		}
	
		public struct HorizontalPaths
		{
				public int e;
				public int w;
		}
	
		public RelativePosition rp;
		public VerticalPaths vp;
		public HorizontalPaths hp;
	
	
		// Use this for initialization
		void Start ()
		{           
				saturate ();
		
		
				int posJitter = DesertGenerator.tileSide / 4;
				Vector3 pos1 = GetComponent<Transform> ().position;
				pos1.x += posJitter;
				pos1.y += posJitter;
				Vector3 pos2 = pos1;
				pos2.y -= posJitter * 2;
				Vector3 pos3 = pos1;
				pos3.x -= posJitter * 2;
				Vector3 pos4 = pos2;
				pos4.x -= posJitter * 2;
		
		
				availablePositions = new Collection<Vector3> ();
				availablePositions.Add (pos1);
				availablePositions.Add (pos2);
				availablePositions.Add (pos3);
				availablePositions.Add (pos4);
		
		
		
				occupants = new Collection<GameObject> ();
		
				desert = GameObject.Find ("Desert");
		
		
		}
	
		void saturate ()
		{
				defaultCol = GetComponent<SpriteRenderer> ().color;
				defaultCol.a = 255.0f;
				GetComponent<SpriteRenderer> ().color = defaultCol;
		}
	
		void Update ()
		{       //....presently it does allow tiles to be rotated when they are occupied;
				//ed to think whether we want this.
		
				if (!roomForMoreOccupants ()) {
						GetComponent<BoxCollider2D> ().enabled = false;
			
				} else {
						GetComponent<BoxCollider2D> ().enabled = true;
				}
		
		
				if (isInRotationState ()) {
						rotateByKey ();
			
						flashToIndicateMoveState ();
			
				} else
			
						GetComponent<SpriteRenderer> ().color = defaultCol; 
		
		
		}
	
		bool isInRotationState ()
		{
				return desert.GetComponent<DesertState> ().movingObject == gameObject;
		}
	
		public bool occupied ()
		{
				return occupants.Count () > 0;
		}
	
		public bool roomForMoreOccupants ()
		{
				return occupants.Count () < maxAllowedOccupants;
		}
	
		public int getNumOccupants ()
		{
				return occupants.Count ();
		}
	
		public void setRelativePosition (int relativeX, int relativeY)
		{
				rp = new RelativePosition ();
				rp.x = relativeX;
				rp.y = relativeY;
		}
	
		public void setVerticalPaths (int n, int s)
		{
				vp = new VerticalPaths ();
				vp.n = n;
				vp.s = s;
		
		}
	
		public void setHorizontalPaths (int w, int e)
		{
				hp = new HorizontalPaths ();
				hp.w = w;
				hp.e = e;
		
		}
	
		public void flip ()
		{
				flipped = true;
				updateSprite ();
				showEvent ();
		}
	
		public void updateSprite ()
		{
				Sprite newSprite = desert.GetComponent<DesertTileIndex> ().getDesertTile (vp.n, vp.s, hp.e, hp.w);
				GetComponent<SpriteRenderer> ().sprite = newSprite;
		}
	
		public void showEvent ()
		{
				if (desertEvent) {
						desertEvent.GetComponent<SpriteRenderer> ().enabled = true;
				}
		}
	
		public GameObject MoveVertical (int direction)
		{        //- n + s
		
				if (hasPathToMove (vp.n, vp.s, direction)) {
			
						if (moveToGoodTile (direction > 0 ? DesertGenerator.SOUTH_INDEX : DesertGenerator.NORTH_INDEX))
								return adjGood;
						int targetX = rp.x;
						int targetY = rp.y + direction;
						return moveIfPossible (targetX, targetY, direction);
				} else
						return null;
		
		}
	
		public GameObject MoveHorizontal (int direction)
		{       //- = left (w) + = right (e)
		
				if (hasPathToMove (hp.w, hp.e, direction)) {
			
						if (moveToGoodTile (direction > 0 ? DesertGenerator.EAST_INDEX : DesertGenerator.WEST_INDEX))
								return adjGood;
			
						int targetX = rp.x + direction;
						int targetY = rp.y;
						return moveIfPossible (targetX, targetY, direction);
				} else
						return null;
		}
	
		bool hasPathToMove (int pathPositive, int pathNegative, int moveDir)
		{         
		
				return (moveDir < 0 && pathPositive == 1 || moveDir > 0 && pathNegative == 1);
		
		}
	
		bool moveToGoodTile (int direction)
		{      
				return  (adjGood != null && direction == adjGoodLocation);
		}
	
		public bool isBazaar ()
		{
				return (gameObject.GetComponent<Bazaar> () != null);
		
		}
	
		//assume that the target is not a "good" tile.
		GameObject moveIfPossible (int targetX, int targetY, int direction)
		{      
				GameObject targetTile = desert.GetComponent<DesertGenerator> ().getTileAtIndex (targetX, targetY);
		
				if (targetTile) {  
						bool tilePathsConnect = pathsConnect (targetTile, direction);
						bool targetIsFlipped = targetTile.GetComponent<DesertTile> ().flipped;
						if (targetIsFlipped) {
								bool hasRoomForMoreOccupants = targetTile.GetComponent<DesertTile> ().roomForMoreOccupants ();
								if (tilePathsConnect && hasRoomForMoreOccupants) {
										bool hasAnOccupant = targetTile.GetComponent<DesertTile> ().occupied ();
										if (hasAnOccupant)
												return handleOccupiedTile (targetTile);
										else
												return targetTile;
								}
								return null;
						}
						//target is not flipped.
						if (!tilePathsConnect)
								rotatePath (targetTile);
						return targetTile;
				}
				return targetTile;
		}
	
		GameObject handleOccupiedTile (GameObject targetTile)
		{
				if (!targetTile.GetComponent<DesertTile> ().isBazaar ()) {
						GameObject headOccupantOfTargetTile = targetTile.GetComponent<DesertTile> ().getHeadOccupant ();
						GameObject invader = desert.GetComponent<DesertState> ().movingObject;
						if (headOccupantOfTargetTile.GetComponent<DesertExplorer> ().acceptInvader (invader)) {
								headOccupantOfTargetTile.GetComponent<DesertExplorer> ().handleInvader (headOccupantOfTargetTile);
						} else
								targetTile = null;
				}
				return targetTile;
		}

		//... the mercenary is only considered the "head" occupant in case he's the only occupant on the tile.
		// otherwise if first is the mercenary then if there are more then get the next one
		GameObject getHeadOccupant ()
		{
				GameObject first = occupants.First<GameObject> ();
				if (first.GetComponent<DesertExplorer> ().isMercenary () && getNumOccupants () > 1) 
						first = occupants.ElementAt (1);
				
				return first;
				
		
		}
	
		bool pathsConnect (GameObject target, int direction)
		{     
				if (target.GetComponent<DesertTile> ().rp.x == rp.x)
						return (target.GetComponent<DesertTile> ().vp.n == vp.s && direction > 0 || target.GetComponent<DesertTile> ().vp.s == vp.n && direction < 0);
				if (target.GetComponent<DesertTile> ().rp.y == rp.y)
						return (target.GetComponent<DesertTile> ().hp.e == hp.w && direction < 0 || target.GetComponent<DesertTile> ().hp.w == hp.e && direction > 0);
		
				return false;
		
		
		}
	
		//swaps the values of vertical paths for horizontal paths. Always creates a connector.
		void rotatePath (GameObject target)
		{
				int rand = (int)Random.Range (0, 2);
				if (rand == 0)
						target.GetComponent<DesertTile> ().rotateRight ();
				else 
						target.GetComponent<DesertTile> ().rotateLeft ();
		
		
		}
	
		void OnMouseUpAsButton ()
		{
				if (flipped) {
						if (!occupied () || isBazaar ()) {
								GameObject playerWhoseTurnItIs = desert.GetComponent<DesertState> ().playerWhoseTurnItIs;
								if (playerWhoseTurnItIs) {//if new player clicks on tile
										if (playerWhoseTurnItIs != playerWhoIsRotatingTile) {
												playerWhoIsRotatingTile = playerWhoseTurnItIs;
												makeTileRotatable ();
										} else { //same user double clicks on tile again to stop rotating
												playerWhoIsRotatingTile = null;
												leaveRotationStateIfNecessary ();
										}
								}
						}
				}
		}
	
		void makeTileRotatable ()
		{
				desert.GetComponent<DesertState> ().changeMover (gameObject);
		}
	
		void leaveRotationStateIfNecessary ()
		{
				if (isInRotationState ())
						desert.GetComponent<DesertState> ().movingObject = null;
		}
	
		void rotateByKey ()
		{
				if (isInRotationState () && playerWhoIsRotatingTile.GetComponent<PlayerInventory> ().waterAvailable ()) {
			
						if (Input.GetKeyDown (KeyCode.LeftArrow)) {
								rotateLeft ();
								changeSpriteAndDecrementRotatingPlayerWater ();
						} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
								rotateRight ();
								changeSpriteAndDecrementRotatingPlayerWater ();
						} 
				} 
		}
	
		void changeSpriteAndDecrementRotatingPlayerWater ()
		{
				playerWhoIsRotatingTile.GetComponent<PlayerInventory> ().decreaseAvailableWater ();
				updateSprite ();
		}
	
		void rotateLeft ()
		{
		
				int newNorth = hp.e;
				int newWest = vp.n;
				int newEast = vp.s;
				int newSouth = hp.w;
		
				hp.e = newEast;
				hp.w = newWest;
				vp.n = newNorth;
				vp.s = newSouth;
		}
	
		void rotateRight ()
		{
		
				int newNorth = hp.w;
				int newWest = vp.s;
				int newEast = vp.n;
				int newSouth = hp.e;
		
				hp.e = newEast;
				hp.w = newWest;
				vp.n = newNorth;
				vp.s = newSouth;
		}
	
		public void setEvent (GameObject desertEvent_)
		{
				desertEvent = desertEvent_;
				Vector3 newPos = GetComponent<Transform> ().position;
				newPos.z = 1.0f;
				newPos.x -= (tileSideLength / 2 - eventMarkerSideLength / 2);
				newPos.y -= (tileSideLength / 2 - eventMarkerSideLength / 2);
				desertEvent.GetComponent<Transform> ().position = newPos;
		
				desertEvent.GetComponent<SpriteRenderer> ().enabled = false;
				hasEvent = true;
				desertEvent.GetComponent<Event> ().setTileWhereLocated (gameObject);
		}

		public bool hasDesertEvent ()
		{
				return hasEvent;
		}

		public GameObject getEvent ()
		{
				return desertEvent;
		}
	
		void flashToIndicateMoveState ()
		{
				Color c = GetComponent<SpriteRenderer> ().color;
				c.a = alphas [alphindex];
				GetComponent<SpriteRenderer> ().color = c;
				alphindex += (alphindex < 1 ? 1 : -1);
		}
	
		public void leaveTile (GameObject explorer, Vector3 explorersPosition)
		{      
				occupants.Remove (explorer);
				availablePositions.Add (explorersPosition);
		
		}
	
		//assume that move was successful
		public Vector3 enterTile (GameObject explorer)
		{      
		
				if (!flipped)
						flip ();
		
				Vector3 explorersPosition = availablePositions.First<Vector3> ();
				availablePositions.Remove (explorersPosition);
				occupants.Add (explorer);
		
				return explorersPosition;
		
		}
	
	
	
	
	
	
	
	
	
	
}
