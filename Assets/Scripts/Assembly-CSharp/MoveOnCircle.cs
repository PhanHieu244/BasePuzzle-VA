using UnityEngine;

public class MoveOnCircle : MonoBehaviour
{
	public float radius = 1f;

	public float speed = 1f;

	public float currentAngle;

	public Transform center;

	private void Update()
	{
		currentAngle += Time.deltaTime * speed;
		base.transform.position = new Vector3(center.position.x + Mathf.Cos(currentAngle) * radius, center.position.y + Mathf.Sin(currentAngle) * radius);
	}
}
