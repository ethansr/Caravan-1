using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DesertTileIndex : MonoBehaviour
{
	 
		public Sprite[] desertTileSprites = new Sprite[7];
	    public Sprite[] goodTileSprites = new Sprite[16];
	    
		public Dictionary<int,Sprite> sprites;

		void Start ()
		{
				sprites = new Dictionary<int,Sprite> ();
				sprites.Add (1 * 8 + 1 * 4 + 1 * 2 + 1, desertTileSprites [0]);
				sprites.Add (1 * 8 + 1 * 4 + 0 * 2 + 0, desertTileSprites [1]);
				sprites.Add (0 * 8 + 0 * 4 + 1 * 2 + 1, desertTileSprites [2]);
				sprites.Add (0 * 8 + 1 * 4 + 1 * 2 + 1, desertTileSprites [3]);
				sprites.Add (1 * 8 + 0 * 4 + 1 * 2 + 1, desertTileSprites [4]);
				sprites.Add (1 * 8 + 1 * 4 + 0 * 2 + 1, desertTileSprites [5]);
				sprites.Add (1 * 8 + 1 * 4 + 1 * 2 + 0, desertTileSprites [6]);

		}

		public Sprite getDesertTile (int n, int s, int e, int w)
		{      
				int spritekey = (n * 8) + (s * 4) + (e * 2) + w;
				
				return sprites [spritekey];
		}

}
