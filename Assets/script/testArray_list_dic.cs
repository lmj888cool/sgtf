using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class testArray_list_dic : MonoBehaviour {

	#region PUBLIC_DECLARATIONS  

	public int numberOfIterations = 10000000;  

	#endregion  

	#region PRIVATE_DECLARATIONS  

	private Stopwatch stopWatch;  

	private List<int> intList;                 // 整数列表  
	private Dictionary<int,int> intDictionary;    // 一本字典，键和值为整数。  
	private int[] intArray;                     // 一个整数数组  

	#endregion  

	#region UNITY_CALLBACKS  

	void Start()  
	{  
		stopWatch = new Stopwatch();  
		intArray = new int[numberOfIterations];  
		intList = new List<int>();  
		intDictionary = new Dictionary<int, int>();  

		AddFakeValuesInArray(numberOfIterations);  
		AddFakeValuesInList(numberOfIterations);  
		AddFakeValuesInDictionay(numberOfIterations);  
	}  

	void Update()  
	{  
		if (Input.GetKeyDown(KeyCode.Space))  
		{  
			PerformTest();  
		}  

		if (Input.GetKeyDown(KeyCode.S))  
		{  
			SearchInList(111);  
			SearchInDictionary(numberOfIterations - 1);  
			UnityEngine.Debug.Log("SearchComplete");  
		}  
	}  

	#endregion  

	#region PRIVATE_METHODS  

	private void AddFakeValuesInArray(int iterations)  
	{  
		for (int i = 0; i < iterations; i++)  
		{  
			intArray[i] = Random.Range(0, 100);  
		}  
	}  

	private void AddFakeValuesInList(int iterations)  
	{  
		for (int i = 0; i < iterations; i++)  
		{  
			intList.Add(Random.Range(0, 100));  
		}  
		intList[iterations - 1] = 111;  
	}  


	private void AddFakeValuesInDictionay(int iterations)  
	{  
		for (int i = 0; i < iterations; i++)  
		{  
			intDictionary.Add(i, Random.Range(0, 100));  
		}  
		intDictionary[iterations - 1] = 111;  
	}  

	private void SearchInList(int value)  
	{  
		#region FIND_IN_LIST  
		stopWatch.Start();  
		int index = intList.FindIndex(item => item == value);  

		stopWatch.Stop();  
		UnityEngine.Debug.Log("Index " + index);  
		UnityEngine.Debug.Log("Time Taken to Find in List  "+stopWatch.ElapsedMilliseconds+" ms");  
		stopWatch.Reset();  
		#endregion  

		#region CHECK_IF_CONTAINS_VALUE_IN_LIST  
		stopWatch.Start();  
		bool containsValue = intList.Contains(value);  

		stopWatch.Stop();  
		UnityEngine.Debug.Log(containsValue);  
		UnityEngine.Debug.Log("Time Taken To Check in List "+stopWatch.ElapsedMilliseconds+" ms");  
		stopWatch.Reset();  
		#endregion  
	}  

	private void SearchInDictionary(int key)  
	{  
		#region FIND_IN_DICTIONARY_USING_REQUIRED_KEY  
		stopWatch.Start();  
		int value = intDictionary[key];  
		stopWatch.Stop();  
		UnityEngine.Debug.Log("Time Taken to Find in Dictionary   "+ stopWatch.ElapsedMilliseconds +" ms");
			stopWatch.Reset();  
			#endregion  

			#region CHECK_IF_DICTIONARY_CONTAINS_VALUE  
			stopWatch.Start();  
			bool containsKey = intDictionary.ContainsKey(key);  

			stopWatch.Stop();  
			UnityEngine.Debug.Log(containsKey);  
			UnityEngine.Debug.Log("Time taken to check if it contains key in Dictionary" + stopWatch.ElapsedMilliseconds + "ms");
			stopWatch.Reset();  
			#endregion  
			}  

			private void PerformTest()  
			{  

				#region ARRAY_ITERATION        // 循环遍历数组  
				stopWatch.Start();  

				for (int i = 0; i < intArray.Length; i++)  
				{  

				}  

				stopWatch.Stop();  
				UnityEngine.Debug.Log("Time Taken By Array "+stopWatch.ElapsedMilliseconds+ "ms");  
				stopWatch.Reset();  

				#endregion  

				#region LIST_ITERATION            //  循环遍历列表中使用简单的 for 循环  
				stopWatch.Start();  
				for (int i = 0; i < intList.Count; i++)  
				{  

				}  
				stopWatch.Stop();  
				UnityEngine.Debug.Log("Time Taken By List "+stopWatch.ElapsedMilliseconds+ "ms");  
				stopWatch.Reset();  
				#endregion  

				#region LIST_ITERATION_BY_FOREACH_LOOP             //  遍历列表中通过使用 foreach 循环  
				stopWatch.Start();  
				foreach (var item in intList)  
				{  

				}  
				stopWatch.Stop();  
				UnityEngine.Debug.Log("Time Taken By List Using foreach  "+stopWatch.ElapsedMilliseconds+ "ms");  
				stopWatch.Reset();  
				#endregion  

				#region DICTIONARY_ITERATIOn_LOOP                //  遍历字典  
				stopWatch.Start();  

				foreach (var key in intDictionary.Keys)  
				{  

				}  
				stopWatch.Stop();  
				UnityEngine.Debug.Log("Time Taken By Dictionary "+stopWatch.ElapsedMilliseconds+ "ms");  
				stopWatch.Reset();  
				#endregion  
			} 
}
#endregion UNITY_CALLBACKS 