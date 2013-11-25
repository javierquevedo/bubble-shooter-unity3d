using UnityEngine;
using System.Collections;
namespace com.javierquevedo{
	
	public static class ArrayListExtension{
		
		/*
		 * ArrayList class extension method to add values only they are not null
		 */
		public static void AddNotNull(this ArrayList arrayList, object theObject){
			if (theObject != null){
				arrayList.Add(theObject);
			}
		}
		
		/*
		 * Removes from the ArrayList those elements that exist in a given ArrayList
		 */
		public static void Exclusive(this ArrayList arrayList, ArrayList sourceArrayList){
			ArrayList exclusives = new ArrayList();
			foreach (object anObject in arrayList){
				if (!sourceArrayList.Contains(anObject)){
					exclusives.Add (anObject);
				}
			}
			arrayList.RemoveRange(0, arrayList.Count);
			arrayList.AddRange(exclusives);
		}
		
		
		/* Removes all duplications */
		public static ArrayList Distinct(this ArrayList arrayList){
			ArrayList returnArray = new ArrayList();
			foreach (object someObject in arrayList)
			{
				if (!returnArray.Contains(someObject)){
					returnArray.Add(someObject);
				}
			}
			return returnArray;
		}
	}

}
