using System.Collections;
using MirkoAquaro.Assets.Subscriptions;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public static GameMaster instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private IEnumerator Start()
	{
		while (Purchaser.m_StoreController == null)
		{
			yield return new WaitForEndOfFrame();
		}
		SubscriptionsRewardsController.instance.Initialize(Purchaser.m_StoreController, Purchaser.m_StoreExtensionProvider);
	}
}
