using UnityEngine;

public class MarginObject : MonoBehaviour
{
	public bool isMarginLeft;

	public bool isMarginRight;

	public bool isMarginTop;

	public bool isMarginBottom;

	public float marginLeft;

	public float marginRight;

	public float marginTop;

	public float marginBottom;

	private void Start()
	{
		float width = UICamera.instance.GetWidth();
		float height = UICamera.instance.GetHeight();
		if (isMarginLeft)
		{
			base.transform.position = new Vector3(0f - width + marginLeft, base.transform.position.y, base.transform.position.z);
		}
		if (isMarginRight)
		{
			base.transform.position = new Vector3(width - marginRight, base.transform.position.y, base.transform.position.z);
		}
		if (isMarginTop)
		{
			base.transform.position = new Vector3(base.transform.position.x, height - marginTop, base.transform.position.z);
		}
		if (isMarginBottom)
		{
			base.transform.position = new Vector3(base.transform.position.x, 0f - height + marginBottom, base.transform.position.z);
		}
	}
}
