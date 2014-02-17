using UnityEngine;
using System.Collections;

public abstract class Event : MonoBehaviour
{   
	
		public const int numbersInBagOfChance = 6;
		public const float secondsWaitForEventResult = 5.0f;
		public const float secondsWaitForEventResultClear = secondsWaitForEventResult * 2;
		public static GameObject eventText;
		protected float eventStartTime;
		protected bool effectOccurring;
		protected bool tookEffect;
		protected GameObject explorer;
		protected GameObject desertTileWhereLocated;

		public abstract void activateEvent (GameObject desertExplorer);

		public void setTileWhereLocated (GameObject tile)
		{
				desertTileWhereLocated = tile;
		}
	
		void Start ()
		{
				eventText = GameObject.Find ("EventText");
				clearEventText ();
				effectOccurring = false;
				tookEffect = false;
			
		}
	
		public static bool drawFromBagOfChance (int numbersChosen)
		{
				int numberDrawn = (int)Random.Range (0, numbersInBagOfChance);
				return (numberDrawn < numbersChosen);
		}
	
		public static void writeToEventText (string message)
		{
				eventText.GetComponent<GUIText> ().text = message;

		}


	
		public static void clearEventText ()
		{
				eventText.GetComponent<GUIText> ().text = "";
		}

		protected void displayResultOfTwoCaseEvent (bool result, string waitingForResultMessage, string resultTrueMessage, string resultFalseMessage)
		{
				
				if ((Time.time - eventStartTime) < secondsWaitForEventResult) {
						writeToEventText (waitingForResultMessage);
				} else if ((Time.time - eventStartTime) < secondsWaitForEventResultClear) {
						if (result) {
								writeToEventText (resultTrueMessage);
								if (!tookEffect) {
										takeEffect ();
								}
						} else
								writeToEventText (resultFalseMessage);
						tookEffect = true;
				} else {
								
						clearEventText ();
						effectOccurring = false; 
				
				}
				
		}
	
		protected abstract void takeEffect ();
	
	
	
}
