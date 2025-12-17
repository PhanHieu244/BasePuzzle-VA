using System.Collections.Generic;
using UnityEngine;

public class MultiPooler : MonoBehaviour
{
	public GameObject[] objectPrefabs;

	private Stack<GameObject>[] arrayPooledObjects;

	private void Awake()
	{
		int num = objectPrefabs.Length;
		arrayPooledObjects = new Stack<GameObject>[num];
		for (int i = 0; i < num; i++)
		{
			arrayPooledObjects[i] = new Stack<GameObject>();
		}
	}

	public GameObject GetPooledObject(int index)
	{
		if (arrayPooledObjects[index].Count > 0)
		{
			GameObject gameObject = arrayPooledObjects[index].Pop();
			gameObject.SetActive(true);
			return gameObject;
		}
		return Object.Instantiate(objectPrefabs[index]);
	}

	public void Push(GameObject obj, int index)
	{
		obj.SetActive(false);
		arrayPooledObjects[index].Push(obj);
	}

	public GameObject GetObjectPrefab(int index)
	{
		return objectPrefabs[index];
	}
}
