using UnityEngine;
using System.Collections;
/*
 * 
 * BubbleMatrix models the Matrix of Bubbles in the game
 * BubbleMatrix allows to insert or remove bubbles.
 * The BubbleMatrix also answers questions such as What is the cluster of similarly connected
 * colored bubbles? Or what bubbles are loose int he current matrix?.
 * 
 * 
 */

namespace com.javierquevedo{
	
	
	public class BubbleMatrix {
		
		/* Public properties */
		public bool isBaselineAlignedLeft; // Determines if the top row of the matrix is aligned to the LEFT
		
		/* Private iVars */
		private int _rows;
		private int _columns;
		private Bubble[,] _bubbleMatrix;
		
		/* @Constructor
		 * @params {Int, Int} rows, colums. The number of rows and columns of the Bubble matrix
		 * @return {BubbleMatrix}
		 */
		public BubbleMatrix(int rows, int columns){
			isBaselineAlignedLeft = true;
			this._rows = rows;
			this._columns = columns;
			this._bubbleMatrix = new Bubble[rows, columns];
		}
		
		/*
		 * Inserts a bubble into the bubble matrix
		 * @param {Int,Int} location
		 */
		public void insert(Bubble bubble, int x, int y){
			if (x < 0 || x > this._rows -1 || y < 0 || y > this._columns -1)
				throw new System.ArgumentException("Adding Bubble to ilegal coordinates");
			
			this._bubbleMatrix[x,y] = bubble;
		}
		
		
		/*
		 * Removes a bubble of the bubble matrix
		 * @param {Int,Int} location
		 */
		public void remove(int x, int y){
			if (x < 0 || x > this._rows -1 || y < 0 || y > this._columns -1)
				throw new System.ArgumentException("Removing Bubble from illegal coordinates");	

			this._bubbleMatrix[x,y] = null;
		}
		
		/*
		 * Removes a bubble of the matrix if this exists
		 * @param {Bubble} bubble
		 */
		public void remove(Bubble bubble){
			Vector2 location = this.location(bubble);
			if ((int)location.x > -1 && (int)location.y > -1)
				this.remove((int)location.x, (int)location.y);
		}
		
		/*
		 * Returns all of the bubbles in the Matrix
		 * @return {ArrayList}
		 */
		public ArrayList bubbles{
			get{
				
				ArrayList bubbles = new ArrayList();
				for (int i=0; i < this._rows; i++){
					for (int j=0; j< this._columns; j++){
						if (this._bubbleMatrix[i,j] != null)
							bubbles.Add(this._bubbleMatrix[i,j]);
					}
				}
				return bubbles;
			}
		}
		
		/*
		 * Returns true if there is a bubble in a particular location
		 * @return {bool}
		 */
		public bool hasBubble(int row, int column){
			return this.bubble(row, column) != null;
		}
		
		/*
		 * Returns the bubble in a particular location
		 * @param {int} row
		 * @param {int} column
		 * @return {Bubble}
		 */
		public Bubble bubble(int row, int column){
			return this._bubbleMatrix[row, column];
		}
		
		/*
		 * Returns all of the Bubbles which are in the immediate neighborhoud of a given location, including the bubble
		 *  in that particular location
		 * Returns null if there is no bubble in the current location
		 * @param {Vector2} location. Location of the matrix that we want to obtain the neighbours of
		 */
		public ArrayList neighbours(Vector2 location){
			int row = (int)location.x;
			int column = (int)location.y;
			
			ArrayList _neigbours = new ArrayList();
			if (row < 0 || row > this._rows -1 || column < 0 || column > this._columns -1)
				throw new System.ArgumentException("Looking for neighbors of an invalid location");	
			
			if (this._bubbleMatrix[row, column] != null){			
				_neigbours.AddNotNull(this._bubbleMatrix[row,column]);
				
				// Left and right neighbours
				if (column > 0) _neigbours.AddNotNull(this._bubbleMatrix[row,column -1]);
				if (column < this._columns-1) _neigbours.AddNotNull(this._bubbleMatrix[row,column+1]);
				
				// higher and lower neighbours
				bool isRowEven = row % 2 == 0;
				if ((isBaselineAlignedLeft && isRowEven) || (!isBaselineAlignedLeft && !isRowEven) ){
					if (row > 0){
							if (column > 0) _neigbours.AddNotNull(this._bubbleMatrix[row-1, column-1]);
							_neigbours.AddNotNull(this._bubbleMatrix[row-1, column]);
						}
						if (row < this._rows -1){
							if (column > 0) _neigbours.AddNotNull(this._bubbleMatrix[row+1, column-1]);
							_neigbours.AddNotNull(this._bubbleMatrix[row +1, column]);
						}
				}
				else{
					if (row > 0){
						_neigbours.AddNotNull(this._bubbleMatrix[row-1, column]);
						if (column < this._columns - 1) _neigbours.AddNotNull(this._bubbleMatrix[row-1, column+1]);
					}
					if (row < this._rows - 1){
						_neigbours.AddNotNull(this._bubbleMatrix[row+1, column]);
						if (column < this._columns - 1) _neigbours.AddNotNull(this._bubbleMatrix[row+1, column+1]);
					}
				}
				return _neigbours;
			}
			return null;
		}
		
		/*
		 * Returns the location of a bubble in the matrix
		 * @param {Bubble} bubble
		 */
		public Vector2 location(Bubble bubble){
			for (int i = 0 ; i< this._rows; i++)
			{
				for (int j = 0; j< this._columns; j++){
					Bubble someBubble = this._bubbleMatrix[i,j];
					if (bubble == someBubble){
						return new Vector2(i, j);
					}
				}				
			}
			return new Vector2(-1,-1);
		}
		
		/*
		 * Returns the neighbours of a given bubble
		 * @param {Bubble} bubble
		 * @return {ArrayList}
		 */
		public ArrayList neighbours(Bubble bubble){
			return neighbours(location(bubble));
		}
		
		/*
		 * Returns the cluster of connected similar colored bubble for a given bubble
		 * @param {Bubble} bubble
		 * @return {ArrayList}
		 */
		public ArrayList colorCluster(Bubble bubble){
			return colorClusterRecursive(bubble, new ArrayList()).Distinct();
		}
		
		/*
		 * Returns an array consiting of all the Bubbles which are not connected to anything
		 * i.e, the balls that should fall
		 */
		public ArrayList looseBubbles(){
			ArrayList anchoredBubbles = this.anchoredBubbles();
			ArrayList connectedBubbles = new ArrayList();
			
			foreach (Bubble anchoredBubble in anchoredBubbles){
				ArrayList connected = this.connectedBubbles(anchoredBubble);
				connectedBubbles.AddRange(connected);
				connectedBubbles = connectedBubbles.Distinct();
			}
			ArrayList theBubbles = this.bubbles;
			theBubbles.Exclusive(connectedBubbles);
			return theBubbles;
		}
		
		private ArrayList anchoredBubbles(){
			ArrayList anchoredBubbles = new ArrayList();
			for (int j = 0; j < this._columns; j++){
				if (this._bubbleMatrix[0,j] != null){
					anchoredBubbles.Add(this._bubbleMatrix[0,j]);
				}
			}
			return anchoredBubbles;
		}
			
		
		private ArrayList connectedBubbles(Bubble bubble){
			return connectedBubblesRecursive(bubble, new ArrayList(), isBaselineAlignedLeft);	
		}
		
		private ArrayList connectedBubblesRecursive(Bubble bubble, ArrayList visited, bool isBasedAlignedLeft){
			ArrayList neighboursNotVisited = this.neighbours(bubble);
			neighboursNotVisited.Exclusive(visited);
			//neighboursNotVisited.Remove(bubble);
			visited.Add(bubble);
			ArrayList returnArray = new ArrayList();
			returnArray.Add(bubble);
			foreach (Bubble someBubble in neighboursNotVisited){
				if (bubble != someBubble)
					returnArray.AddRange(connectedBubblesRecursive(someBubble, visited, isBasedAlignedLeft));
			}
			return returnArray;
		}
		
		private bool anchoresToBaseline(ArrayList bubbles){
			foreach (Bubble bubble in bubbles){
				if (this.location(bubble).x ==0)
					return true;
			}
			return false;
		}
		
		/*
		 * Backtrackng recursive function that obtains the cluster of bubble
		 * which share the same color for a given location
		 */
		private ArrayList colorClusterRecursive(Bubble bubble, ArrayList visited){		
			ArrayList similarColorNeighbours = JQUtils.FilterByColor(this.neighbours(bubble), bubble.color);
			similarColorNeighbours.Exclusive(visited);
			visited.Add(bubble);
			ArrayList returnArray = new ArrayList();
			returnArray.Add(bubble);
			foreach (Bubble aBubble in similarColorNeighbours){
				if (bubble != aBubble)
					returnArray.AddRange(colorClusterRecursive(aBubble, visited));
			}
			return returnArray;
		}
		
		/*Lowers all of the rows one row
		 * @return {bool} True if there was overflow
		 */
		public bool shiftOneRow()
		{
			bool overflows = false;
			for (int i = this._rows -1; i >= 0; i--)
			{
				for (int j = 0; j < this._columns; j++)
				{
					if (_bubbleMatrix[i,j] != null)
					{
						if (i >= this._rows -1)
						{
							overflows = true;
						}else					
						{
							_bubbleMatrix[i+1,j] = _bubbleMatrix[i,j];
							_bubbleMatrix[i,j] = null;
						}
					}else{
						//Debug.Log("null item");
						// throw exception
					}	
				}
			}
			isBaselineAlignedLeft = !isBaselineAlignedLeft;
			return overflows;
		}
	}

}