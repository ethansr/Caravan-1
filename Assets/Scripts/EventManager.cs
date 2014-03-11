using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
		public static int maxQueuedEvents = 3;
		static Event[] events;
		static int numQueuedEvents;
		static int indexOfNextEvent;
		Event currentEvent;
		

		// Use this for initialization
		void Start ()
		{       
				reset ();
				events = new Event[maxQueuedEvents];
	
		}
	
		// Update is called once per frame
		void Update ()
		{    
				if ((currentEvent && currentEvent.done) || (!currentEvent && eventInQueue ())) 
						activateNextEventInQueue ();
				else
						reset ();
	
		}

		bool eventInQueue ()
		{
				return numQueuedEvents > 0;

		}

		public static void addEventToQueue (Event nextEvent)
		{       
				if (numQueuedEvents < maxQueuedEvents) {
						Debug.Log (nextEvent.name);
						events [numQueuedEvents] = nextEvent;
						numQueuedEvents++;
				}
			
	
		}

		void getNextEventInQueue ()
		{
				if (++indexOfNextEvent < maxQueuedEvents) {
						currentEvent = events [indexOfNextEvent];
						events [indexOfNextEvent] = null;
				} else
						reset ();
				
		}

		void reset ()
		{
				currentEvent = null;
				numQueuedEvents = 0;
				indexOfNextEvent = numQueuedEvents - 1;
		}

		void activateNextEventInQueue ()
		{
				getNextEventInQueue ();
				if (currentEvent) 
						currentEvent.activateEvent ();
				
	
		}

		



}
