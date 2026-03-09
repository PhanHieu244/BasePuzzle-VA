using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SetupPopup : MonoBehaviour
{
    string nameCallback;
    BaseUIPopup uIPopup;

    private void Awake()
    {
        //Create UI
        name = name.Replace("(Clone)", "").Trim();
        uIPopup = GetComponent<BaseUIPopup>();
        nameCallback = uIPopup.name;
    }

    private void Start()
    {
        UILoadPopupUltis.dicActionOnOpenPopup[nameCallback]?.Invoke(uIPopup);
    }
}