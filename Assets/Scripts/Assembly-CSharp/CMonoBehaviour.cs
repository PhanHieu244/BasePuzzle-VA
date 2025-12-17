using UnityEngine;

public class CMonoBehaviour : MonoBehaviour
{
	public void DestroyIfExist(Transform objTransform)
	{
		if (objTransform != null)
		{
			Object.Destroy(objTransform.gameObject);
		}
	}
}
