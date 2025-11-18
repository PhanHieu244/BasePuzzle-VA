using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUIScreen : MonoBehaviour 
{
	void Awake()
	{
        Application.targetFrameRate = 60;
		init ();	
	}

	public virtual void init()
	{
		PStackManager.Instance.PushWindow (gameObject);
	}
}
