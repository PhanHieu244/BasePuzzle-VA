using System;
using UnityEngine.Purchasing;

[Serializable]
public class IAPItem
{
	public string productID;

	public ProductType productType;

	public float price;

	public int value;
}
