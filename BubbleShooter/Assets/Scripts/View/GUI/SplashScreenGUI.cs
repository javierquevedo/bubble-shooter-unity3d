using UnityEngine;
using System.Collections;
namespace com.javierquevedo{
	
	public class SplashScreenGUI : MonoBehaviour {

		StartGameSelectionDelegate startGameDelegate;
			public delegate void StartGameSelectionDelegate();
			
			public StartGameSelectionDelegate StartGameDelegate{
				set{
					this.startGameDelegate = value;
				}
			}
			
			private const float _menuWidth = 200;
			private const float _menuHeight = 100;
			private const float _buttonWidth = 150;
			private const float _buttonHeight = 50;
			// Use this for initialization
			void Start () {
			
			}
			
			// Update is called once per frame
			void Update () {
			
			}
			
			void OnGUI(){
				GUI.Box(new Rect(Screen.width/2 - _menuWidth /2.0f, Screen.height/2 - _menuHeight /2.0f, _menuWidth, _menuHeight), "Bubble Shooter");
				GUI.Label(new Rect(20,20,200,30), "Javier Quevedo-Fernandez");
				GUI.Label(new Rect(20,40,200,30), "http://github.com/senc01a");
				
				if (GUI.Button(new Rect(Screen.width/2 - _buttonWidth /2.0f, (Screen.height/2 - _buttonHeight /2.0f) + 5, _buttonWidth, _buttonHeight), "Start game")){
					this.startGame();
				}
				
			}
			
			private void startGame(){
				if (this.startGameDelegate != null)
					this.startGameDelegate();
			}
		}
}

