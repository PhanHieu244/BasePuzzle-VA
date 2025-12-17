using UnityEngine;

public class UIRotate : MonoBehaviour
{
	private RectTransform rect;

	[SerializeField]
	private float speed = -20f;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		rect.Rotate(Vector3.forward, Time.deltaTime * speed);
	}
}
