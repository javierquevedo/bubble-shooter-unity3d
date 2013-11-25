using UnityEngine;
using System.Collections;

namespace com.javierquevedo{
	
	public class SplashScreenController : MonoBehaviour {
	
		private GameObject _camera;
		private SplashScreenGUI _gui;
		private const string _bubbleShooterPrefabName = "Prefabs/BubbleShooterPrefab";
		
		void Start () {
			_camera = GameObject.Find("Camera");	
			this._gui = _camera.AddComponent<SplashScreenGUI>();
			this._gui.StartGameDelegate = this.startGame;
		}

		
		void startGame(){
			this._gui.StartGameDelegate = null;
			GameObject game = new GameObject("Game");
			game.AddComponent<GameController>();
			Destroy(this._gui);
			Destroy (this.gameObject);
			
		}
	}
}