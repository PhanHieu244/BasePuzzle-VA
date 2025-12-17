using UnityEngine.UI;

public class ShopDialog : Dialog
{
	public Text[] numHintTexts;

	public Text[] priceTexts;

	protected override void Start()
	{
		base.Start();
		int num = 0;
		Text[] array = numHintTexts;
		foreach (Text text in array)
		{
			text.text = Purchaser.instance.iapItems[num].value + " hints";
			num++;
		}
		num = 0;
		Text[] array2 = priceTexts;
		foreach (Text text2 in array2)
		{
			text2.text = Purchaser.instance.iapItems[num].price + "$";
			num++;
		}
	}

	public override void Close()
	{
		base.Close();
		SubscriptionsPopup.instance.Show(Purchaser.m_StoreController, 1.5f);
	}

	public void OnBuyProduct(int index)
	{
		Purchaser.instance.BuyProduct(index);
	}
}
