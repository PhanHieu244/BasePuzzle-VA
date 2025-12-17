using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class AdUnit
{
	public enum LoadStatus
	{
		loading = 0,
		loaded = 1,
		failed = 2
	}

	public UnityEvent onAdClick;

	protected AdNetworksManager caller;

	[HideInInspector]
	public bool GDPRConsent;

	internal LoadStatus _loadStatus;

	public LoadStatus loadStatus
	{
		get
		{
			return _loadStatus;
		}
	}

	public abstract void Initialize(AdNetworksManager caller, bool GDPRConsent, float delay = 0f);

	public virtual bool IsLoaded()
	{
		return _loadStatus == LoadStatus.loaded;
	}

	internal void AdStatusChanged(object sender, EventArgs args, LoadStatus status)
	{
		_loadStatus = status;
	}
}
