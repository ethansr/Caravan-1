using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
		public static int maxQueuedEvents = 3;
		static Event[] events;
		static int numQueuedEvents;


		// Use this for initialization
		void Start ()
		{       
				
				events = new Event[maxQueuedEvents];
	
		}

		public static void alertEventManagerThatEventIsFinished ()
		{
				

				decrementNumberOfEventsInQueue ();
				advanceQueue ();
				if (eventInQueue ())
						activateNextEventInQueue ();
		}

		static void decrementNumberOfEventsInQueue ()
		{
				numQueuedEvents -= (numQueuedEvents == 0 ? 0 : 1);
		}

		static void activateNextEventInQueue ()
		{
				currentEvent ().activateEvent ();
			

		}
	
		static void advanceQueue ()
		{
				for (int i=0; i<maxQueuedEvents-1; i++) {
						events [i] = events [i + 1];
				}
				
			
		
		}

		static Event currentEvent ()
		{
				return events [0];
		}

		static bool eventInQueue ()
		{
				return numQueuedEvents > 0;

		}

		public static void addEventToQueue (Event nextEvent)
		{       
				if (numQueuedEvents < maxQueuedEvents) {
						events [numQueuedEvents] = nextEvent;
						
						if (!eventInQueue ())
								activateNextEventInQueue ();
						numQueuedEvents++;
				}
			
	
		}







		



}
