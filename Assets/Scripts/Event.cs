using UnityEngine;
using System.Collections;

public abstract class Event : MonoBehaviour
{   
	
		public const int numbersInBagOfChance = 6;
		protected float secondsWaitForEventResult;
		protected float secondsWaitForEventResultClear;
		public static GameObject eventText;
		protected float eventStartTime;
		protected bool effectOccurring;
		protected bool tookEffect;
		protected GameObject explorer;
		protected GameObject desertTileWhereLocated;
		protected float delayStartTime;
		protected float eventTextBoxDisplayDelay = 2.0f;
		protected bool inControlOfTextBox;
	
		public abstract void activateEvent (GameObject desertExplorer);

		public void setTileWhereLocated (GameObject tile)
		{
				desertTileWhereLocated = tile;
		}
	
		void Start ()
		{
				eventText = GameObject.Find ("EventText");
				secondsWaitForEventResult = 5.0f;
				secondsWaitForEventResultClear = secondsWaitForEventResult * 2;
				disableEventTextBox ();
				clearEventText ();
				effectOccurring = false;
				tookEffect = false;
				inControlOfTextBox = false;
		       
			
		}
	
		public static bool drawFromBagOfChance (int numbersChosen)
		{
				int numberDrawn = (int)Random.Range (0, numbersInBagOfChance);
				return (numberDrawn < numbersChosen);
		}
	
		public static void writeToEventText (string message)
		{
				renderTextBox ();
				eventText.GetComponent<GUIText> ().text = message;

		}

		protected static void renderTextBox ()
		{
				
				eventText.GetComponent<SpriteRenderer> ().enabled = true;
		
		}

		protected static void disableEventTextBox ()
		{
				eventText.GetComponent<SpriteRenderer> ().enabled = false;
				
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

		protected void tellPlayerToFinishEndTurn ()
		{
				if (explorer && explorer.GetComponent<Meeple> ().player) {
						explorer.GetComponent<Meeple> ().player.GetComponent<Player> ().finishEndTurn ();
				}
		}
	
	
}
