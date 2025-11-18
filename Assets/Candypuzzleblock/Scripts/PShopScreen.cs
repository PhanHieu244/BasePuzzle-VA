using UnityEngine;
using System.Collections;

public class PShopScreen : MonoBehaviour
{
    bool hasGameExit = false;

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        #region time mode
        if (PGamePlay.Instance != null && (PGameController.gameMode == GameMode.TIMED || PGameController.gameMode == GameMode.CHALLENGE)) {
            PGamePlay.Instance.timeSlider.PauseTimer ();
        }
        #endregion
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        #region time mode
        if(!hasGameExit)
        {
            if (PGamePlay.Instance != null && (PGameController.gameMode == GameMode.TIMED || PGameController.gameMode == GameMode.CHALLENGE)) {
                PGamePlay.Instance.timeSlider.ResumeTimer ();
            }
        }
        #endregion
    }
    
    /// <summary>
    /// Raises the close button pressed event.
    /// 
    /// </summary>
    public void OnCloseButtonPressed ()
    {
        if (PInputManager.Instance.CanInput ()) {
            PAudioManager.Instance.PlayButtonClickSound ();
            PStackManager.Instance.OnCloseButtonPressed ();
        }
    }
}