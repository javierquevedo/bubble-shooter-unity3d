using UnityEngine;
using System.Collections;


namespace com.javierquevedo.gui{
	
	public class GameFinishedGUI : MenuGUI {
	
		GameFinishGUIDelegate startNewGameSelectedDelegate;
		public delegate void GameFinishGUIDelegate();
			
		public GameFinishGUIDelegate StartNewGameSelectedDelegate{
			set{
				this.startNewGameSelectedDelegate = value;
			}
		}
		
		public Game game;
		void Start () {
				base.Start();;
			}
			
			// Update is called once per frame
			void Update () {
				base.Update ();
			
			}
			
			protected override void  OnGUI(){
				GUI.Box(new Rect(Screen.width/2 - _menuWidth /2.0f, Screen.height/2 - _menuHeight /2.0f, _menuWidth, _menuHeight), "Bubble Shooter");
				GUI.Label(new Rect(Screen.width/2 - _buttonWidth /2.0f, (Screen.height/2 - _buttonHeight /2.0f) - 14, _buttonWidth, _buttonHeight), 
				"YOU " + (game.state == GameState.Win ? "WON!" : "LOST!"));	
				if (GUI.Button(new Rect(Screen.width/2 - _buttonWidth /2.0f, (Screen.height/2 - _buttonHeight /2.0f) + 15, _buttonWidth, _buttonHeight), "BACK TO MENU")){
					this.newGameWasSelected();
				}
			}
		
		private void newGameWasSelected(){
			if (startNewGameSelectedDelegate != null)
				startNewGameSelectedDelegate();
			}
		}
			


}
