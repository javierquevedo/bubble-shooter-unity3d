using UnityEngine;
using System.Collections;
using com.javierquevedo.events;
namespace com.javierquevedo{
	
	
	
	public class BubbleMatrixController : MonoBehaviour {
		// Geometry */
		public float leftBorder = 0.0f;
		public float rightBorder = 10.5f;
		public float topBorder = 10.0f;
		
		// Dimensions
		public int rows = 10;
		public int columns = 10;
		public float bubbleRadius = 0.5f;
		public float addRowPeriod = 10.0f;
		/* View Properties */
		public BubbleMatrixGeometry geometry;
		
		
		// Constants
		private const float _bubbleLinearSpeed = 12.0f;
		private const string _bubblePrefabName = "Prefabs/BubblePrefab";
		private const int _defaultRowsCount = 5;
		
		// Private iVars
		
		private BubbleMatrix _matrix;
		private GameObject _bubblesContainer;
		private GameObject _bubbleShooter;
		private BubbleController _currentBubble;
		private ArrayList _bubbleControllers;
		private bool _pendingToAddRow;
		private bool _isPlaying;
		//private float _shiftAnimationDuration = 0.2f;
		
		void Awake(){
			this._isPlaying = false;
			this._pendingToAddRow = false;
			this._matrix = new BubbleMatrix(rows, columns);
			this._bubbleControllers = new ArrayList();
		}
		
		void Start(){
		}
		
		public void startGame(){
			
			this._bubblesContainer = GameObject.Find("Bubbles");
			this._bubbleShooter = GameObject.Find ("BubbleShooter");
			this.geometry = new BubbleMatrixGeometry(leftBorder, rightBorder, topBorder, 0.0f, rows, columns, bubbleRadius);
			this._currentBubble = this.createBubble();
			this._isPlaying = true;
			StartCoroutine("addRowScheduler");
			
			for (int i =0 ; i < _defaultRowsCount; i++){
				this.addRow();
			}
			
		}
		
		void Update(){
			if (Input.GetMouseButtonDown(0) && this._isPlaying){
				if (this._currentBubble != null){
					this._currentBubble.isMoving = true;
					this._currentBubble.angle = this.shootingRotation();
					this._currentBubble = null;
				}
			}
		}
		
		private BubbleController createBubble(){
			GameObject bubblePrefab = Instantiate(Resources.Load(_bubblePrefabName)) as GameObject;
			bubblePrefab.transform.parent = _bubblesContainer.transform;
			bubblePrefab.transform.position = new Vector3((rightBorder - leftBorder) / 2.0f - geometry.bubbleRadius / 2.0f, -0.65f, 0);
			BubbleController bubbleController = bubblePrefab.GetComponent<BubbleController>();
			bubbleController.leftBorder = this.geometry.leftBorder;
			bubbleController.rightBorder = this.geometry.rightBorder;
			bubbleController.topBorder = this.geometry.topBorder;
			bubbleController.radius = this.geometry.bubbleRadius;
			bubbleController.linearSpeed = _bubbleLinearSpeed;
			bubbleController.angle = 90.0f;
			bubbleController.isMoving = false;
			bubbleController.CollisionDelegate = onBubbleCollision;
			bubbleController.MotionDelegate = canMoveToPosition;
			this._bubbleControllers.Add(bubbleController);
			return bubbleController;
		}	
		
		private float shootingRotation(){
			float shooterRotation = this._bubbleShooter.transform.eulerAngles.z;
				float ballRotation = 90;
				if (shooterRotation <= 360 &&  shooterRotation >= 270.0){
					ballRotation = shooterRotation - 270;
				}
				if (shooterRotation <= 90 &&  shooterRotation >= 0){
					ballRotation = 90 + shooterRotation;
				}
			return ballRotation;
		}
		
		private void destroyBubble(BubbleController bubbleController, bool explodes){
			this._matrix.remove(bubbleController.bubble);
			this._bubbleControllers.Remove(bubbleController);
			bubbleController.CollisionDelegate = null;
			bubbleController.kill(explodes);
			//Destroy(bubbleController.gameObject);
		}
		
		private BubbleController controllerForBubble(Bubble bubble){
			foreach (BubbleController bubbleController in this._bubbleControllers){
				if (bubbleController.bubble == bubble)
					return bubbleController;
			}
			return null;
		}
		
		IEnumerator addRowScheduler(){
			
				yield return new WaitForSeconds(addRowPeriod);
				this._pendingToAddRow = true;
			
		}
		
		private void destroyCluster(ArrayList cluster, bool explodes){
			foreach (Bubble bubble in cluster){
				this.destroyBubble(this.controllerForBubble(bubble), explodes);
			}
		}
		
		
		private void addRow(){
			this._pendingToAddRow = false;
			bool overflows = this._matrix.shiftOneRow();
			
			
			for (int i = 0; i<this.geometry.columns; i++){
				BubbleController bubbleController = this.createBubble();
				bubbleController.isMoving = false;
				this._matrix.insert(bubbleController.bubble, 0,i);
			}
			
			foreach (BubbleController bubbleController in this._bubbleControllers){
				if (bubbleController != this._currentBubble){
					Vector3 position = BubbleMatrixControllerHelper.PositionForCell(this._matrix.location(bubbleController.bubble), geometry, this._matrix.isBaselineAlignedLeft);
					//bubbleController.moveTo(position, this._shiftAnimationDuration);				
					bubbleController.transform.position = position;
				}
				
			}
			
			if (overflows){
				this.FinishGame(GameState.Loose);
				return;
			}
		}
		
		/*
		 * Common game finishing sequence
		 * @param {GameState} the state of the game, whether it was won or lost
		 */
		private void FinishGame(GameState state){
			BubbleShooterController shooterController = this._bubbleShooter.GetComponent<BubbleShooterController>();
			shooterController.isAiming = false;
			GameEvents.GameFinished(state);
			this._isPlaying = false;
		}
		
		
		/* 
		 * Delegate Handlers
		 */
		/*
		 * Collision Delegate Handler
		 */
		void onBubbleCollision(GameObject bubble){
			
			// If the ball falls under the amoun of rows, the game is over
			Vector2 bubblePos = BubbleMatrixControllerHelper.CellForPosition(bubble.transform.position, this.geometry, this._matrix.isBaselineAlignedLeft);
			if ((int)bubblePos.x >= this.geometry.rows){
				this.FinishGame(GameState.Loose);
				return;
			}
				
			// Create the new bubble
			BubbleController bubbleController = bubble.GetComponent<BubbleController>();
			Vector2 matrixPosition = BubbleMatrixControllerHelper.CellForPosition(bubble.transform.position, this.geometry, this._matrix.isBaselineAlignedLeft);

			// Update the model
			this._matrix.insert(bubbleController.bubble, (int)matrixPosition.x, (int)matrixPosition.y);
	
			// if we don't have to add a new row (because of the timer), move the bubble smoothly to its snapping point			
			if (!this._pendingToAddRow){
				bubbleController.moveTo(BubbleMatrixControllerHelper.PositionForCell(matrixPosition, geometry, this._matrix.isBaselineAlignedLeft), 0.1f);
			}else{
				// otherwise move it rapidly
				bubbleController.transform.position = BubbleMatrixControllerHelper.PositionForCell(matrixPosition, geometry, this._matrix.isBaselineAlignedLeft);
			}
			
			// Explode the bubbles that need to explode
			// The the cluster of bubbles with a similar color as the colliding one
			ArrayList cluster = this._matrix.colorCluster(bubbleController.bubble);
			
			if (cluster.Count > 2){
				// Explode the cluster
				bubbleController.transform.position = BubbleMatrixControllerHelper.PositionForCell(matrixPosition, geometry, this._matrix.isBaselineAlignedLeft);
				this.destroyCluster(cluster, true);
				// Notify that bubbles have been removed
				GameEvents.BubblesRemoved(cluster.Count, true);
			}
			
			// Drop the bubbles that fall
			cluster = this._matrix.looseBubbles();
			this.destroyCluster(cluster, false);
			if (cluster.Count > 0)
				GameEvents.BubblesRemoved(cluster.Count, false);
			
			// Add a new Row of random bubbles if required
			if (_pendingToAddRow){
				this.addRow();
				StartCoroutine("addRowScheduler");
			}
			
			// If there are no bubble lefts, win the game
			if (this._matrix.bubbles.Count == 0){
				this.FinishGame(GameState.Win);
				return;
			}
			
			// Prepare the new bubble to shoot it
			this._currentBubble = this.createBubble();
		}
		
		/*
		 * Delegate method to lets the bubble controler know if the move to location is allowed
		 * Used to avoid a situation when two balls can end up in the same snapping point
		 * if the Time.delta is sufficently large between two frames
		 * 
		*/
		bool canMoveToPosition(Vector3 position){
			Vector2 location = BubbleMatrixControllerHelper.CellForPosition(position, this.geometry, this._matrix.isBaselineAlignedLeft);
			if ((int)location.x <= this.geometry.rows-1){
				return !this._matrix.hasBubble((int)location.x, (int)location.y);	 
			}
			return true;
		}

		
	}
	
}
