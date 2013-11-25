using UnityEngine;
using System.Collections;

	
namespace com.javierquevedo{

	public enum GameState {Playing, Loose, Win};
	public class Game {
		/* Constants */
		
		public const int PointsPerExplosion = 20;
		public const int PointsPerFall = 25;
		
		
		/* Public properties */
		public GameState state;
		
		/* Private iVars */
		private int _points;
		private int _bubblesDestroyed;
		
		public Game(){
			this._points = 0;
			this._bubblesDestroyed = 0;
		}

		/* The score of the current game
		 * Read-only
		 */
		public int score{
			get{
				return this._points;
			}
		}
		
		/* The amount of bubbles that have been destroyed throughout the game
		 * Read-only
		 */
		public int bubblesDestroyed{
			get{
				return this._bubblesDestroyed;
			}
		}
		
		public void destroyBubbles(int bubbleCount, bool exploded){
			this._points +=  exploded ? bubbleCount * PointsPerExplosion : bubbleCount * PointsPerFall;
			this._bubblesDestroyed += bubbleCount;	
		}
	}
	
}
