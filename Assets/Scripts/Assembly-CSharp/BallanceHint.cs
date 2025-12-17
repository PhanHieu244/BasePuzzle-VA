using UnityEngine;

public class BallanceHint : MonoBehaviour
{
	private void Start()
	{
		if (!Purchaser.instance.isEnable)
		{
			base.gameObject.SetActive(false);
		}
	}
}
