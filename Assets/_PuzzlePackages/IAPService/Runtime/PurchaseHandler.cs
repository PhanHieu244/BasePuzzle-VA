using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace BasePuzzle.PuzzlePackages.IAPService
{
	using SRF;

	public class PurchaseHandler
	{
		private readonly List<BuyPurchaseProcess>     _buyProcesses    =new List<BuyPurchaseProcess>();
		private readonly List<RestorePurchaseProcess> _restoreProcesses=new List<RestorePurchaseProcess>();
		private readonly IAPStoreHandler              _storeHandler;

		private StoreController m_StoreController;
		private IAPService      _service;

		private bool _canMakePurchase;
		public  bool CanMakePurchase => _canMakePurchase;

		public PurchaseHandler()
		{
#if UNITY_EDITOR
			_storeHandler=new IAPFakeStoreHandler();
#elif UNITY_ANDROID
            _storeHandler=new IAPGooglePlayHandler();
#elif UNITY_IOS
            _storeHandler=new IAPAppstoreHandler();
#else
            _storeHandler=new IAPFakeStoreHandler();
#endif
		}

		public async void Init(ProductSettings settings,IAPService iapService)
		{
			_service=iapService;

			/*var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			_storeHandler.SetCanMakePurchase(builder, ref _canMakePurchase);

			foreach (var product in settings.Products)
			{
			    builder.AddProduct(product.productID, product.type);
			}

			UnityPurchasing.Initialize(this, builder);*/

			m_StoreController=UnityIAPServices.StoreController();

			m_StoreController.OnPurchasePending+=OnPurchasePending;

			await m_StoreController.Connect();

			m_StoreController.OnProductsFetched +=OnProductsFetched;
			m_StoreController.OnPurchasesFetched+=OnPurchasesFetched;

			var initialProductsToFetch=new List<ProductDefinition> { };

			foreach(var product in settings.Products)
			{
				initialProductsToFetch.Add(new(product.productID,product.type));
			}

			m_StoreController.FetchProducts(initialProductsToFetch);

			m_StoreController.OnPurchaseFailed+=OnPurchaseFailed;
			m_StoreController.OnPurchaseFailed+=OnPurchaseFailed;

			await m_StoreController.Connect();
		}

		void OnPurchaseFailed(FailedOrder obj) { }

		void OnPurchasePending(PendingOrder obj) { }

		void OnProductsFetched(List<Product> products)
		{
			// Handle fetched products  
			m_StoreController.FetchPurchases();
		}
		void OnPurchasesFetched(Orders orders)
		{
			// Process purchases, e.g. check for entitlements from completed orders  
		}


		public void OnInitializeFailed(InitializationFailureReason error) { OnInitializeFailed(error,null); }

		public void OnInitializeFailed(InitializationFailureReason error,string message)
		{
			var errorMessage=$"{_storeHandler.GetType()} > Lỗi init IAP. Nguyên nhân: {error}.";

			if(message!=null)
			{
				errorMessage+=$" Chi tiết: {message}";
			}

			Debug.LogError(errorMessage);
		}

		public void Purchase(string productID,Action<Product> success,Action<PurchaseFailure> failure)
		{
			if(m_StoreController==null)
			{
				Debug.LogError($"{typeof(PurchaseHandler)} > Bạn cần phải khởi tạo IAP Service trước.");
				return;
			}

			if(_storeHandler.Store==PurchaseStore.FakeStore)
			{
				success?.Invoke(m_StoreController.GetProductById(productID));
				return;
			}

			if(_buyProcesses.Any(p => p.HasProductID(productID)))
			{
				Debug.LogError(
				$"{_storeHandler.GetType()} > Purchase > Đã có 1 quá trình thanh toán khác của productID này đang diễn ra. Đợi thanh toán hoàn tất trước khi thực hiện thanh toán mới");
				return;
			}

			var process=new BuyPurchaseProcess(
			productID,
			product =>
			{
				Log(product,"iap");
				success?.Invoke(product);
			},
			failure,
			_service.Timeout);
			_buyProcesses.Add(process);
			m_StoreController.PurchaseProduct(productID);
		}

		private static void Log(Product product,string where)
		{
			var productID=product.definition.id;
			var metadata =InAppPurchaser.GetProductMetadata(productID);
			if(metadata==null) return;

			var price        =metadata.localizedPrice;
			var currencyCode =metadata.isoCurrencyCode;
			var transactionID=product.transactionID;
			var purchaseToken=InAppPurchaser.GetPurchaseToken(product);
			var level        =LevelDataController.instance.Level;


		}

		public void OnPurchaseFailed(Product product,PurchaseFailureReason failureReason)
		{
			Debug.LogError(
			$"{_storeHandler.GetType()} > Lỗi thanh toán - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");

			OnPurchaseFailed(product,failureReason.ToString());
		}

		public void OnPurchaseFailed(Product product,PurchaseFailureDescription failureDescription)
		{
			Debug.LogError(
			$"{_storeHandler.GetType()} > Lỗi thanh toán - Product: '{product.definition.id}',"+
			$" Lý do: {failureDescription.reason},"+
			$" Chi tiết: {failureDescription.message}");

			OnPurchaseFailed(product,failureDescription.message);
		}

		private void OnPurchaseFailed(Product product,string msg)
		{
			foreach(var process in _buyProcesses)
			{
				if(!process.HasProductID(product.definition.id)) continue;

				_buyProcesses.Remove(process);
				process.OnPurchaseFailed(msg);
				return;
			}
		}


		public void OnReceiveAppsflyerValidation(string validationInfo)
		{
			Dictionary<string,object> dictionary=Json.Deserialize(validationInfo) as Dictionary<string,object>;
			if(dictionary==null)
			{
				Debug.LogWarning($"{nameof(OnReceiveAppsflyerValidation)} > data is null.");
				return;
			}

			_storeHandler.AppsflyerValidation(
			dictionary,
			validationInfo,
			OnAppsflyerValidationSuccess,
			OnAppsflyerFraudDetected);
		}

		private void OnAppsflyerValidationSuccess(string transactionID)
		{
			foreach(var bp in _buyProcesses)
			{
				if(!bp.HasTransactionID(transactionID)) continue;

				Debug.Log($"$[{typeof(PurchaseHandler)}] > Appsflyer Validation [BuyProcess]: Xác minh thanh toán thành công");
				bp.ValidationSucceeded();
				return;
			}

			foreach(var rp in _restoreProcesses)
			{
				if(!rp.HasTransactionID(transactionID)) continue;

				Debug.Log($"$[{typeof(PurchaseHandler)}] > Appsflyer Validation [RestoreProcess]: Xác minh thanh toán thành công");
				rp.ValidationSucceeded();
				return;
			}
		}

		private void OnAppsflyerFraudDetected(string transactionID)
		{
			foreach(var bp in _buyProcesses)
			{
				if(!bp.HasTransactionID(transactionID)) continue;

				Debug.Log($"$[{typeof(PurchaseHandler)}] > Appsflyer Validation [BuyProcess]:  Phát hiện hack.");
				bp.HackDetected();
				return;
			}

			foreach(var rp in _restoreProcesses)
			{
				if(!rp.HasTransactionID(transactionID)) continue;

				Debug.Log($"$[{typeof(PurchaseHandler)}] > Appsflyer Validation [RestoreProcess]:  Phát hiện hack.");
				rp.HackDetected();
				return;
			}
		}


		public void Dispose()
		{
			foreach(var bp in _buyProcesses)
			{
				bp.ReleaseCTS();
			}

			foreach(var rp in _restoreProcesses)
			{
				rp.ReleaseCTS();
			}
		}
		public ProductMetadata GetProductMetadata(string productID) { return m_StoreController.GetProductById(productID)?.metadata; }
	}
}
