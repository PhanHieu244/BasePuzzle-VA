using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGameManager : MonoBehaviour
{
	

	public string Product1;

	public string Product2;

	private static bool m_consumeOnPurchase;

	private static bool _consumeOnQuery;

	private Dropdown _dropdown;

	private List<Dropdown.OptionData> options;

	private static Text _textField;

	private static bool _initialized;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Information");
		_textField = gameObject.GetComponent<Text>();
		_textField.text = "Please Click Init to Start";
		gameObject = GameObject.Find("Dropdown");
		_dropdown = gameObject.GetComponent<Dropdown>();
		_dropdown.ClearOptions();
		_dropdown.options.Add(new Dropdown.OptionData(Product1));
		_dropdown.options.Add(new Dropdown.OptionData(Product2));
		_dropdown.RefreshShownValue();
		InitUI();
	}

	private static void Show(string message, bool append = false)
	{
		_textField.text = ((!append) ? message : string.Format("{0}\n{1}", _textField.text, message));
	}

	private void InitUI()
	{
		GetButton("InitButton").onClick.AddListener(delegate
		{
			_initialized = false;
			Debug.Log("Init button is clicked.");
			Show("Initializing");
			//StoreService.Initialize(initListener);
		});
		GetButton("BuyButton").onClick.AddListener(delegate
		{
			if (!_initialized)
			{
				Show("Please Initialize first");
			}
			else
			{
				string text2 = _dropdown.options[_dropdown.value].text;
				Debug.Log("Buy button is clicked.");
				Show("Buying Product: " + text2);
				m_consumeOnPurchase = false;
				Debug.Log(_dropdown.options[_dropdown.value].text + " will be bought");
				//StoreService.Purchase(text2, null, "{\"AnyKeyYouWant:\" : \"AnyValueYouWant\"}", purchaseListener);
			}
		});
		GetButton("BuyConsumeButton").onClick.AddListener(delegate
		{
			if (!_initialized)
			{
				Show("Please Initialize first");
			}
			else
			{
				string text = _dropdown.options[_dropdown.value].text;
				Show("Buying Product: " + text);
				Debug.Log("Buy&Consume button is clicked.");
				m_consumeOnPurchase = true;
				//StoreService.Purchase(text, null, "buy and consume developer payload", purchaseListener);
			}
		});
		List<string> productIds = new List<string> { Product1, Product2 };
		GetButton("QueryButton").onClick.AddListener(delegate
		{
			if (!_initialized)
			{
				Show("Please Initialize first");
			}
			else
			{
				_consumeOnQuery = false;
				Debug.Log("Query button is clicked.");
				Show("Querying Inventory");
				//StoreService.QueryInventory(productIds, purchaseListener);
			}
		});
		GetButton("QueryConsumeButton").onClick.AddListener(delegate
		{
			if (!_initialized)
			{
				Show("Please Initialize first");
			}
			else
			{
				_consumeOnQuery = true;
				Show("Querying Inventory");
				Debug.Log("QueryConsume button is clicked.");
				//StoreService.QueryInventory(productIds, purchaseListener);
			}
		});
	}

	private Button GetButton(string buttonName)
	{
		GameObject gameObject = GameObject.Find(buttonName);
		if (gameObject != null)
		{
			return gameObject.GetComponent<Button>();
		}
		return null;
	}
}
