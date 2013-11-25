using UnityEngine;
using System.Collections;
namespace com.javierquevedo{
	
	public class BubbleMatrixGeometry {
		
		
		// TODO: Priority low
		// Make private ivars and create getters and setters
		
		/* Geometry */
		public float leftBorder;
		public float rightBorder;
		public float topBorder;
		public float depth;
		// Dimensions
		public int rows;
		public int columns;
		public float bubbleRadius;
		
		public BubbleMatrixGeometry(float leftBorder, float rightBorder, float  topBorder, float depth, int rows, int columns, float bubbleRadius){
			this.leftBorder = leftBorder;
			this.rightBorder = rightBorder;
			this.topBorder = topBorder;
			this.rows = rows;
			this.columns = columns;
			this.bubbleRadius = bubbleRadius;
			this.depth = depth;
		}
	
	}
}
