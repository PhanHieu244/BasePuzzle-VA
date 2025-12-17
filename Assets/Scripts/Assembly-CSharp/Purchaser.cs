using System;
using UnityEngine;
using UnityEngine.Purchasing;	

public class Purchaser : MonoBehaviour, IStoreListener
{
	public bool isEnable = true;

	public static IStoreController m_StoreController;

	public static IExtensionProvider m_StoreExtensionProvider;

	public static string kProductIDConsumable = "consumable";

	public static string kProductIDNonConsumable = "nonconsumable";

	public static string kProductIDSubscription = "subscription";

	public IAPItem[] iapItems;

	private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

	private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

	public static Purchaser instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		if (!IsInitialized())
		{
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			configurationBuilder.AddProduct(kProductIDConsumable, ProductType.Consumable);
			configurationBuilder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
			
			IAPItem[] array = iapItems;
			foreach (IAPItem iAPItem in array)
			{
				configurationBuilder.AddProduct(iAPItem.productID, iAPItem.productType);
			}
			UnityPurchasing.Initialize(this, configurationBuilder);
		}
	}

	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuyProduct(int index)
	{
		BuyProductID(iapItems[index].productID);
	}

	public void BuyConsumable()
	{
		BuyProductID(kProductIDConsumable);
	}

	public void BuyNonConsumable()
	{
		BuyProductID(kProductIDNonConsumable);
	}

	public void BuySubscription()
	{
		BuyProductID(kProductIDSubscription);
	}

	public void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			Debug.Log("BuyProductID FAIL. Not initialized.");
			Toast.instance.ShowMessage("Check your internet connection and try again");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");
			
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("OnInitialized: PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		IAPItem iAPItem = null;
		IAPItem[] array = iapItems;
		foreach (IAPItem iAPItem2 in array)
		{
			if (string.Equals(args.purchasedProduct.definition.id, iAPItem2.productID, StringComparison.Ordinal))
			{
				Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
				iAPItem = iAPItem2;
			}
		}
		if (iAPItem.productType == ProductType.Consumable)
		{
			GameState.hint.ChangeValue(iAPItem.value);
			Toast.instance.ShowMessage("Your purchase is successful");
			CUtils.SetBuyItem();
		}
		else if (iAPItem.productType != ProductType.NonConsumable && iAPItem.productType != ProductType.Subscription)
		{
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
	}

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
