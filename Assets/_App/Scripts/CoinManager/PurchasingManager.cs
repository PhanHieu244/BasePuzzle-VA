using UnityEngine;

public class PurchasingManager : MonoBehaviour
{
   public void OnPressDown(int i)
   {
      var resetPowerQuantity = PlayerPrefs.GetInt("ResetPower", 0);
      var clearPowerQuantity = PlayerPrefs.GetInt("ClearPower", 0);
      var gold = ResourceType.Gold.Manager();
      switch (i)
      {
         case 1:
            
            if (gold.GetAmount() < 20)
            {
               UIToastManager.Instance.Show("Not enough gold");
               return;
            }
            gold.Subtract(20);
            PlayerPrefs.SetInt("ResetPower", resetPowerQuantity + 5);
            
            break;
         case 2:
            if (gold.GetAmount() < 40)
            {
               UIToastManager.Instance.Show("Not enough gold");
               return;
            }
            gold.Subtract(40);
            PlayerPrefs.SetInt("ClearPower", resetPowerQuantity + 5);
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
