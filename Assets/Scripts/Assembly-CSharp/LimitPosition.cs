using UnityEngine;

public class LimitPosition : MonoBehaviour
{
	public Vector2 maxXAndY;

	public Vector2 minXAndY;

	private void LateUpdate()
	{
		float x = base.transform.position.x;
		float y = base.transform.position.y;
		x = Mathf.Clamp(x, minXAndY.x, maxXAndY.x);
		y = Mathf.Clamp(y, minXAndY.y, maxXAndY.y);
		base.transform.position = new Vector3(x, y, base.transform.position.z);
	}
}
