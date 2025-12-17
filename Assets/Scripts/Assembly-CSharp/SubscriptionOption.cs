using UnityEngine;
using UnityEngine.UI;

public class SubscriptionOption : MonoBehaviour
{
	public string associatedProductID;

	public string priceTextSuffix;

	[SerializeField]
	private Text buttonText;

	[SerializeField]
	private Text priceText;

	[SerializeField]
	private Button _purchaseButton;

	[SerializeField]
	private UnityStringEvent _OnButtonClick;

	private void Awake()
	{
		_purchaseButton.onClick.AddListener(OnButtonClick);
	}

	public void SetPriceText(string text)
	{
		priceText.text = text + priceTextSuffix;
	}

	public void SetDescriptionText(string text)
	{
		buttonText.text = text;
	}

	private void OnButtonClick()
	{
		_OnButtonClick.Invoke(associatedProductID);
	}
}
