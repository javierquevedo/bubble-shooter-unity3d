using UnityEngine;
using System;
using System.Collections;

namespace com.javierquevedo{
	public class HUD : MonoBehaviour {
		
		public Game game;
		
		private float _timeOffset;
		void Start () {
			_timeOffset = Time.timeSinceLevelLoad;	
		}
		
		void Update () {
		}
		
		void OnGUI(){
			GUI.Label(new Rect(20,20,200,30), "Javier Quevedo-Fernandez");
			GUI.Label(new Rect(20,40,200,30), "http://github.com/senc01a");
			
			TimeSpan timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad - _timeOffset);
			string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			
			GUI.Label(new Rect(20,100,200,30), "Score: " + game.score);
			GUI.Label(new Rect(20,120,200,30), "Bubbles destroyed: " + game.bubblesDestroyed);
			GUI.Label(new Rect(20,140,200,30), "Time played: " + timeText);
			
		}
	}
}
