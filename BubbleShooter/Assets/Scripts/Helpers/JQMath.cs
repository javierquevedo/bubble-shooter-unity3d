using UnityEngine;
using System.Collections;

namespace com.javierquevedo{
	
	public class JQMath {
		
		/*
		 * Truncates a number to a particular interval
		 * @param {int} number. The number to truncate
		 * @param {int} min. The min of the interval
		 * @param {int} number. The number to truncate
		 */
		public static int TruncateToInterval(int number, int min, int max){
			int outcome;
			outcome = number;
			if (number < min) outcome = min;
			if (number > max) outcome = max;
			return outcome;
		}
		
	}
}
