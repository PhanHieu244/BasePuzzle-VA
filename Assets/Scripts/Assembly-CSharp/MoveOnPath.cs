using System.Linq;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
	public Transform[] paths;

	public float speed = 0.7f;

	public iTween.LoopType loopType;

	public iTween.EaseType easeType = iTween.EaseType.linear;

	public int currentIndex;

	private int plus = 1;

	private Vector3[] waypoints;

	private void Start()
	{
		if (waypoints == null)
		{
			waypoints = paths.Select((Transform x) => x.position).ToArray();
		}
		MoveToPointComplete();
	}

	public void SetWaypoints(Vector3[] waypoints)
	{
		this.waypoints = waypoints;
	}

	private void MoveToPointComplete()
	{
		if (loopType == iTween.LoopType.none)
		{
			if (currentIndex == waypoints.Length - 1)
			{
				return;
			}
			currentIndex++;
		}
		else if (loopType == iTween.LoopType.loop)
		{
			currentIndex = (currentIndex + 1) % waypoints.Length;
		}
		else if (loopType == iTween.LoopType.pingPong)
		{
			plus = (((plus != -1 || currentIndex != 0) && currentIndex != waypoints.Length - 1) ? plus : (-plus));
			currentIndex += plus;
		}
		Vector3 vector = waypoints[currentIndex];
		iTween.MoveTo(base.gameObject, iTween.Hash("position", vector, "speed", speed, "easeType", easeType, "oncomplete", "MoveToPointComplete"));
	}
}
