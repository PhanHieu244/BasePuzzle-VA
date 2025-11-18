using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasingManager : MonoBehaviour
{
   public void OnPressDown(int i)
   {
      var resetPowerQuantity = PlayerPrefs.GetInt("ResetPower", 0);
      var clearPowerQuantity = PlayerPrefs.GetInt("ClearPower", 0);
      switch (i)
      {
         case 1:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ResetPower", resetPowerQuantity + 5);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK1);
            break;
         case 2:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ResetPower", resetPowerQuantity + 10);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK2);
            break;
         case 3:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ResetPower", resetPowerQuantity + 20);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK3);
            break;
         case 4:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ClearPower", clearPowerQuantity + 5);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK4);
            break;
         case 5:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ClearPower", clearPowerQuantity + 20);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK5);
            break;
         case 6:
            IAPManager.OnPurchaseSuccess = () =>
            {
               PlayerPrefs.SetInt("ClearPower", clearPowerQuantity + 40);
            };
            IAPManager.Instance.BuyProductID(IAPKey.PACK6);
            break;
      }
      
      GameObject currentWindow = PStackManager.Instance.PeekWindow ();
      PStackManager.Instance.PopWindow ();
      Destroy (currentWindow);
      
      PGameController.Instance.UpdateResetPowerQuantityText();
      PGameController.Instance.UpdateClearPowerQuantityText();
   }

   public void Sub(int i)
   {
      GameDataManager.Instance.playerData.SubDiamond(i);
   }
}
