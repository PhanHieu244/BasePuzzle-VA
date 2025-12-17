using UnityEngine;

public class MoveOnLine : MonoBehaviour
{
	public Transform pointA;

	public Transform pointB;

	public float speed = 0.7f;

	public iTween.EaseType easetype = iTween.EaseType.linear;

	protected virtual void Start()
	{
		if (!CUtils.EqualVector3(base.transform.position, pointA.position))
		{
			iTween.MoveTo(base.gameObject, iTween.Hash("position", pointA.position, "speed", speed, "easeType", easetype, "oncomplete", "OnMoveToPointComplete"));
		}
		else
		{
			OnMoveToPointComplete();
		}
	}

	private void OnMoveToPointComplete()
	{
		iTween.MoveTo(base.gameObject, iTween.Hash("position", pointB.position, "looptype", "pingpong", "speed", speed, "easeType", easetype));
	}
}
