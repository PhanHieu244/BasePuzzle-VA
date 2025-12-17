using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	public float time;

	private void Start()
	{
		Object.Destroy(base.gameObject, time);
	}
}
