using UnityEngine;
using System.Collections;

namespace com.javierquevedo.events{
	
public class GameEvents : MonoBehaviour {
	
		public delegate void BubblesRemovedHandler (int bubbleCount, bool exploded);
		public static event BubblesRemovedHandler OnBubblesRemoved;
		
		public static void BubblesRemoved(int bubbleCount, bool exploded){
			if (OnBubblesRemoved != null){
				OnBubblesRemoved(bubbleCount, exploded);
			}
		}
		
		public delegate void GameFinishedHandler (GameState state);
		public static event GameFinishedHandler OnGameFinished;
		
		public static void GameFinished(GameState state){
			if (OnGameFinished != null){
				OnGameFinished(state);
			}
		}
		
	}
	
}
