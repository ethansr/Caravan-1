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
				Mercenary=3
	
	}
		; 
		//the indexes of the quantities of each event are matched to the integer values of the events in the enumeration
		int[] numbersOfEachEvent = {4, 4, 2, 4};

		//is the number of different goods
		public static int numGoods;
		public static int numGoodsPerType = 4;

		public enum GoodType
		{
				Spice=0,
				Animal=1,
				Gem=2,
				Textile=3
	}
		;

		int[] goodCounters = {0,0,0,0};
	    
		public enum GoodItem
		{
				Nutmeg=0,
				Cinnamon=1,
				Saffron=2,
				Clove=3,

				Lion=4,
				Camel=5,
				Horse=6,
				Elephant=7,

				Turqouise=8,
				Amethyst=9,
				Ruby=10,
				Topaz=11,

				AnimalSkin=12,
				PaintedCloth=13,
				Wool=14,
				Silk=15
		}
		;

		public Color firstGoodColor;
		public Color secondGoodColor;
		public Color thirdGoodColor;
		public Color fourthGoodColor;
		Dictionary<int,Color> typesToColors;
		GoodItem[] goods;
		


		// Use this for initialization
		void Start ()
		{
				numGoods = Enum.GetValues (typeof(GoodItem)).Length;
				typesToColors = new Dictionary<int,Color> ();

				typesToColors.Add ((int)GoodType.Spice, firstGoodColor);
				typesToColors.Add ((int)GoodType.Animal, secondGoodColor);
				typesToColors.Add ((int)GoodType.Gem, thirdGoodColor);
				typesToColors.Add ((int)GoodType.Textile, fourthGoodColor);
	
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
				goods = (GoodItem[])Enum.GetValues (typeof(GoodItem));
		      


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

		void assignAdjacentGoods (int y, int x, GameObject tile)
		{
				Vector3 goodTilePos = new Vector3 (0, 0, 0);

			
				int goodTypeAsInt = getAdjGoodTypeFromCoordinates (y, x);
				if (goodTypeAsInt == INTERIOR_TILE_INDEX) { //no adjacent good tile
						tile.GetComponent<DesertTile> ().adjGoodLocation = INTERIOR_TILE_INDEX;
						tile.GetComponent<DesertTile> ().adjGood = null;
						return;
				}

				switch (goodTypeAsInt) {
				case((int)GoodType.Spice):
						tile.GetComponent<DesertTile> ().adjGoodLocation = NORTH_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.y += tileSide;
						break;
				case((int)GoodType.Animal):
						tile.GetComponent<DesertTile> ().adjGoodLocation = SOUTH_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.y -= tileSide;
						break;
				case((int)GoodType.Gem):
						tile.GetComponent<DesertTile> ().adjGoodLocation = WEST_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.x -= tileSide;
						break;
				case((int)GoodType.Textile):
						tile.GetComponent<DesertTile> ().adjGoodLocation = EAST_INDEX;
						goodTilePos = tile.GetComponent<Transform> ().position;
						goodTilePos.x += tileSide;
						break;
			
				}
				makeAndAssignGood (tile, goodTilePos, goodTypeAsInt);
		}

		int getAdjGoodTypeFromCoordinates (int y, int x)
		{
				if (firstGoodType (y, x))
						return (int)GoodType.Spice;
				if (secondGoodType (y, x))
						return (int)GoodType.Animal;
				if (thirdGoodType (y, x))
						return (int)GoodType.Gem;
				if (fourthGoodType (y, x))
						return (int)GoodType.Textile;
				return INTERIOR_TILE_INDEX;
		}

		bool firstGoodType (int y, int x)
		{
				return y == 0 && x % 2 == 1;
		}

		bool secondGoodType (int y, int x)
		{
				return y == numTilesHeight - 1 && x % 2 == 1;
		}

		bool thirdGoodType (int y, int x)
		{
				return x == 0 && y % 2 == 1;
		}
	
		bool fourthGoodType (int y, int x)
		{
				return x == numTilesWidth - 1 && y % 2 == 1;
		}

		void makeAndAssignGood (GameObject tile, Vector3 goodTilePos, int goodTypeAsInt)
		{
				
				GameObject goodTile = (GameObject)Instantiate (good);
				int indexOfGoodItem = (numGoodsPerType * goodTypeAsInt) + goodCounters [goodTypeAsInt];
				goodTile.GetComponent<Good> ().good = goods [indexOfGoodItem];
	
				Color goodSpriteColor = typesToColors [goodTypeAsInt];
				goodTile.GetComponent<SpriteRenderer> ().sprite = GetComponent<DesertTileIndex> ().goodTileSprites [indexOfGoodItem];
				goodSpriteColor.a = 255.0f;
				goodTile.GetComponent<SpriteRenderer> ().color = goodSpriteColor;
				goodTile.GetComponent<Transform> ().position = goodTilePos;
				tile.GetComponent<DesertTile> ().adjGood = goodTile;
				
				goodCounters [goodTypeAsInt]++;

		}
	
		bool center (int y, int x)
		{
				return (x == numTilesWidth / 2 && y == numTilesHeight / 2);

		}

		void setDesertTileParameters (GameObject tile)
		{   
				setPaths (tile);
				
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
								} while(candidateTile.GetComponent<DesertTile>().hasEvent||candidateTile.GetComponent<DesertTile>().isBazaar());
				
								//have to instantiate an event
								GameObject newDesertEvent = (GameObject)Instantiate (events [indexOfNumberOf]);
								candidateTile.GetComponent<DesertTile> ().setEvent (newDesertEvent);
				
						}




				}
		//plus an aditional one for testing
		/*
		GameObject testAdjBazaar = GameObject.FindGameObjectWithTag ("41");
		GameObject newDesertEvent2 = (GameObject)Instantiate (events [0]);
		testAdjBazaar.GetComponent<DesertTile> ().setEvent (newDesertEvent2);
		*/

		}

		public static DesertGenerator.GoodType getGoodTypeGivenLocation (int x, int y)
		{     //two lines define 4 triangular regions
				int a = x;
				int b = (numTilesHeight - 1) - x;
				GoodType result = GoodType.Spice;
				if (y > a && y <= b)
						result = GoodType.Gem;
				else if (y < a && y >= b)
						result = GoodType.Textile;
				else if (y >= a && y > b)
						result = GoodType.Animal;
				return result;

		}

		public static GoodType typeOfGoodItem (GoodItem goodItem)
		{
				int intValofType = (int)goodItem / numGoodsPerType;
				return (GoodType)GoodType.ToObject (typeof(GoodType), intValofType);

		}

		
}
