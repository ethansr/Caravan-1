using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DesertGenerator : MonoBehaviour
{       //constants that hold the raw dimensions of the desert and the tiles
		//these are derived from the pixel to unit field in the sprite assets, so if those are changed these need 
		//to be changed as well.
		public static float numUnitsDesertWidth = 99.8f;
		public static float numUnitsDesertHeight = 56.2f;
		public static float numUnitsDesertTileWidth = 13.6f;
		public static float numUnitsDesertTileHeight = 19.7f;

		//generate 9*9 grid of desert tiles
		static int  numTilesWidth = 9;
		static int numTilesHeight = 9;
		public int totalTiles;
		//this is the desired dimension of the desert. we want a desert of 80*80 units.
		public static int desertSideLength = 80;
		//desired side length in units for the tile, given we want 9 tiles per side of the desert.
		public static int tileSide = (int)desertSideLength / numTilesWidth;
		public static int eventMarkerSide = tileSide / 4;

		//these are the scale factors we need for the desert and the tiles to make them fit the required dimensions.
		float desertXScale = desertSideLength / numUnitsDesertWidth;
		float desertYScale = desertSideLength / numUnitsDesertHeight;
		float desertTileXScale = tileSide / numUnitsDesertTileWidth;
		float desertTileYScale = tileSide / numUnitsDesertTileHeight;


		//desert tile prefab
		public GameObject desertTile;
		public GameObject bazaar;
		public GameObject desertEvent;
		public GameObject good;

		
	    
		//use these to determine how many of each type of pathway to make.
		public int numPathTypes = 7;
		int[] pathTypeCounts = {40,13,13,6,6,6,6};
		//aligned to indexes of pathTypeCounts
		public static int NORTH_INDEX = 0;
		public static int SOUTH_INDEX = 1;
		public static int EAST_INDEX = 2;
		public static int WEST_INDEX = 3;
		public static int INTERIOR_TILE_INDEX = -1;
		int[][] pathTypes = {
				new int[] {1,1,1,1},
				new int[] {1,1,0,0},
				new int[] {
						0,
						0,
						1,
						1
				},
				new int[] {
						0,
						1,
						1,
						1
				},
				new int[] {
						1,
						0,
						1,
						1
				},
				new int[] {
						1,
						1,
						0,
						1
				},
				new int[] {
						1,
						1,
						1,
						0
				}
		};
		GameObject desert;
		float desertTopLeftX;
		float desertTopLeftY;
		float trueTileX;
		float trueTileY;
		int relativeX;
		int relativeY;
		


		//use these to subsequently assign events to tiles.
		//integers are the numbers of each type of event

		//the other classes typically treat events as integers; these must be unique.
		//if you want to add a new event you'll need to adjust the Evennt class as well which
		//contains a delegator method (takeEffect) that branches to the different specific event methods.
		
		public GameObject[] events = new GameObject[4];

		public enum EventType
		{
				Robbery=0,
				Merchant=1,
				Oasis=2,
				Mercenary=3}
		; 
		//the indexes of the quantities of each event are matched to the integer values of the events in the enumeration
		int[] numbersOfEachEvent = {4, 4, 2, 4};

		//use these to identify "goods'
		public static int numGoods = 16;
		public enum GoodType
		{
				Nutmeg=0,
				Clove=1,
				Cinnamon=2,
				Saffron=3,

				Lion=4,
				Camel=5,
				Horse=6,
				Elephant=7,

				Amethyst=8,
				Turquoise=9,
				Topaz=10,
				Ruby=11,

				AnimalSkin=12,
				Silk=13,
				Wool=14,
				PaintedCloth=15,
		};

		GoodType[] goods;
		int goodsCounter;


		// Use this for initialization
		void Start ()
		{
				totalTiles = numTilesWidth * numTilesHeight;
				initDesertParameters ();
				makeTilesAndPlaceInDesert ();
				assignEventsToTiles ();
		      
		}

		void initDesertParameters ()
		{
				desert = GameObject.Find ("DesertBoard");
		        
				desertTopLeftX = desert.GetComponent<Transform> ().position.x - desertSideLength / 2 + tileSide;
				desertTopLeftY = desert.GetComponent<Transform> ().position.y + desertSideLength / 2 - tileSide;
		        
				//rescale the desertboard sprite
				Vector3 desertLocalScale = new Vector3 (desertXScale, desertYScale, desert.GetComponent<Transform> ().localScale.z);
				desert.GetComponent<Transform> ().localScale = desertLocalScale;
	
		}

		void makeTilesAndPlaceInDesert ()
		{       //holder for the tile
				GameObject tile;
				//set up good variables
				goods = (GoodType[])Enum.GetValues (typeof(GoodType));
				goodsCounter = 0;
				//outer loop, go by columns
				for (int i=0; i<numTilesHeight; i++) {
						for (int j=0; j<numTilesWidth; j++) {
								//determine position and path type of desert tile
								//actual positions, for the transform component
								setTileCoordinates (i, j);
								if (center (i, j)) {
										tile = (GameObject)Instantiate (bazaar);
								} else {
										tile = (GameObject)Instantiate (desertTile);
										setDesertTileParameters (tile);
										
								}
								setHasEventToFalse (tile);
								scaleTile (tile);
								assignTileCoordinates (tile);
								tagTileWithOneDimensionalIndex (tile, i, j);
								assignAdjacentGoods (i, j, tile);
								
						}
				}
		}

		void setHasEventToFalse (GameObject tile)
		{
				tile.GetComponent<DesertTile> ().hasEvent = false;

		}

		void scaleTile (GameObject tile)
		{
				Vector3 desertTileLocalScale = new Vector3 (desertTileXScale, desertTileYScale, tile.GetComponent<Transform> ().localScale.z);
				tile.GetComponent<Transform> ().localScale = desertTileLocalScale;

		}

		void setTileCoordinates (int i, int j)
		{       
				trueTileX = desertTopLeftX + (j * tileSide);
				trueTileY = desertTopLeftY - (i * tileSide);
              
				relativeX = j;
				relativeY = i;

		}

		void assignAdjacentGoods (int y, int j, GameObject tile)
		{
				Vector3 goodTilePos;
				if (y == 0 && j % 2 == 1) {
						tile.GetComponent<DesertTile> ().adjGoodLocation = NORTH_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.y += tileSide;

				} else if (y == numTilesHeight - 1 && j % 2 == 1) {
						tile.GetComponent<DesertTile> ().adjGoodLocation = SOUTH_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.y -= tileSide;
		
				} else if (j == 0 && y % 2 == 1) {
						tile.GetComponent<DesertTile> ().adjGoodLocation = WEST_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.x -= tileSide;
			
				} else if (j == numTilesWidth - 1 && y % 2 == 1) {
						tile.GetComponent<DesertTile> ().adjGoodLocation = EAST_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.x += tileSide;
			
				} else { //no adjacent tiles
						tile.GetComponent<DesertTile> ().adjGoodLocation = INTERIOR_TILE_INDEX;
						tile.GetComponent<DesertTile> ().adjGood = null;
						return;
				}
				makeAndAssignGood (tile, goodTilePos);
	
		}

		void makeAndAssignGood (GameObject tile, Vector3 goodTilePos)
		{
				GameObject goodTile = (GameObject)Instantiate (good);
				goodTile.GetComponent<Good> ().good = goods [goodsCounter];
				goodTile.GetComponent<SpriteRenderer> ().sprite = GetComponent<DesertTileIndex> ().goodTileSprites [goodsCounter];
				
				goodTile.GetComponent<Transform> ().position = goodTilePos;
				tile.GetComponent<DesertTile> ().adjGood = goodTile;
				goodsCounter++;

		}
	
		bool center (int y, int x)
		{
				return (x == numTilesWidth / 2 && y == numTilesHeight / 2);

		}

		void setDesertTileParameters (GameObject tile)
		{   
				setPaths (tile);
				//setEvents (tile);
		}

		void setPaths (GameObject tile)
		{
				int rand;
				int typeCount;
				do {
						rand = (int)UnityEngine.Random.Range (0, pathTypeCounts.Length);
						typeCount = pathTypeCounts [rand];
				} while(typeCount==0);
				typeCount--;
				pathTypeCounts [rand] = typeCount;
				int[] pathValues = pathTypes [rand];
				tile.GetComponent<DesertTile> ().setVerticalPaths (pathValues [NORTH_INDEX], pathValues [SOUTH_INDEX]);
				tile.GetComponent<DesertTile> ().setHorizontalPaths (pathValues [EAST_INDEX], pathValues [WEST_INDEX]);
		}

		void assignTileCoordinates (GameObject tile)
		{      
				tile.GetComponent<DesertTile> ().setRelativePosition (relativeX, relativeY);
				tile.GetComponent<Transform> ().position = new Vector2 (trueTileX, trueTileY);
		}

		void tagTileWithOneDimensionalIndex (GameObject tile, int i, int j)
		{
				int tagId = ((i * numTilesWidth) + j);
				tile.tag = tagId.ToString ();
		}
	
		public GameObject getTileAtIndex (int x, int y)
		{
				if (x < numTilesWidth && x >= 0 && y < numTilesHeight && y >= 0)
						return GameObject.FindWithTag (((y * numTilesWidth) + x).ToString ());
				else
						return null;
						
		}
	/*
		void assignEventsToTiles ()
		//for each typpe of e
		{
				foreach (var value in Enum.GetValues(typeof(EventType))) {
						int indexOfNumberOf = (int)value;
						int numberOf = numbersOfEachEvent [indexOfNumberOf];
						for (int i=0; i<numberOf; i++) {
								GameObject candidateTile;
								do {
										int tagOfCandidateTileForEvent = (int)UnityEngine.Random.Range (0, totalTiles);
										candidateTile = GameObject.FindGameObjectWithTag (tagOfCandidateTileForEvent.ToString ());
								} while(candidateTile.GetComponent<DesertTile>().hasEvent);
			
								//have to instantiate an event
								GameObject newDesertEvent = (GameObject)Instantiate (desertEvent);
								newDesertEvent.GetComponent<Event> ().type = (int)value;
								candidateTile.GetComponent<DesertTile> ().setEvent (newDesertEvent);

						}
				}

		}
		*/

		void assignEventsToTiles ()
		{
				for (int i=0; i<events.Length; i++) {
						int indexOfNumberOf = i;
						int numberOf = numbersOfEachEvent [indexOfNumberOf];
						for (int j=0; j<numberOf; j++) {
								GameObject candidateTile;
								do {
										int tagOfCandidateTileForEvent = (int)UnityEngine.Random.Range (0, totalTiles);
										candidateTile = GameObject.FindGameObjectWithTag (tagOfCandidateTileForEvent.ToString ());
								} while(candidateTile.GetComponent<DesertTile>().hasEvent);
				
								//have to instantiate an event
								GameObject newDesertEvent = (GameObject)Instantiate (events [indexOfNumberOf]);
								candidateTile.GetComponent<DesertTile> ().setEvent (newDesertEvent);
				
						}



				}

		}

		
}
