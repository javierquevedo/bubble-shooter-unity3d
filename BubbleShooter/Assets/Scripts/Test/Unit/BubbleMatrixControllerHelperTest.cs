using UnityEngine;
using System.Collections;
namespace com.javierquevedo {
public class BubbleMatrixControllerHelperTest : MonoBehaviour {


		/*
		 * TODO: INTALL PROPER UNIT TEST FRAMEWORK
		 * 
		 * 
		 */
		void Start () {
			this.runTest();
		}
		
		
		void runTest()
		{		
				BubbleMatrixGeometry geometry = new BubbleMatrixGeometry(0.0f, 10.5f, 10.0f, 0.0f, 10, 10, 0.5f);
				Vector3 position1 = new Vector3(0.6f, 9.6f, 0.0f);
				bool isBaselineAlignedLeft1 = true;
				Vector2 expectedAnswer1 = new Vector2(0f,0f);
				Vector2 answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y)));
				
				
				position1 = new Vector3(0.6f, 9.6f, 0.0f);
				isBaselineAlignedLeft1 = false;
				expectedAnswer1 = new Vector2(0f,0f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y)));
			
				
				position1 = new Vector3(1.2f, 9.6f, 0.0f);
				isBaselineAlignedLeft1 = true;
				expectedAnswer1 = new Vector2(0f,1f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				bool correct = ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y));
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + correct);

				
				position1 = new Vector3(1.2f, 9.6f, 0.0f);
				isBaselineAlignedLeft1 = false;
				expectedAnswer1 = new Vector2(0f,0f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				correct = ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y));
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + correct);
			
				
				position1 = new Vector3(0.2f, 11.0f, 0.0f);
				isBaselineAlignedLeft1 = false;
				expectedAnswer1 = new Vector2(0f,0f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				correct = ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y));
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + correct);
			
			
				
				position1 = new Vector3(-2.2f, 13.0f, 0.0f);
				isBaselineAlignedLeft1 = true;
				expectedAnswer1 = new Vector2(0f,0f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				correct = ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y));
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + correct);
			
				
				position1 = new Vector3(10.45f, 13.0f, 0.0f);
				isBaselineAlignedLeft1 = false;
				expectedAnswer1 = new Vector2(0f,9f);
				answer1 =  BubbleMatrixControllerHelper.CellForPosition(position1, geometry, isBaselineAlignedLeft1);
				correct = ((expectedAnswer1.x == answer1.x) && (expectedAnswer1.y == answer1.y));
				Debug.Log ("[BubbleMatrixControllerHelper] Test correct?  " + correct);
				//public static Vector2 CellForPosition(Vector3 position, BubbleMatrixGeometry geometry, bool isBaselineAlignedLeft){
					
			}
	}
}
