using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSelectLanguage : MonoBehaviour 
{
	[SerializeField] 
	private GameObject LangaugeButtonTemplate;

	[SerializeField] 
	private Transform langaugeSelectionContent;


	void Start()
	{
		CreateLanguageList ();
	}

	void CreateLanguageList()
	{
		foreach (Langauge lang in LocalizationManager.Instance.LanguageList) {
			if (lang.isAvailable) {
				CreateLanguageButton (lang);
			}
		}
	}

	void CreateLanguageButton(Langauge languauge)
	{
		GameObject languageButton = (GameObject)Instantiate (LangaugeButtonTemplate);
		languageButton.name = "btn-" + languauge.LanguageName;
		languageButton.GetComponent<PLanguageButton> ().SetLangugaeDetail (languauge);
		languageButton.transform.SetParent (langaugeSelectionContent);
		languageButton.transform.localScale = Vector3.one;
		languageButton.SetActive (true);
	}

	public void OnCloseButtonPressed()
	{
		if (PInputManager.Instance.CanInput ()) {
			PAudioManager.Instance.PlayButtonClickSound ();
			PStackManager.Instance.OnCloseButtonPressed ();
		}
	}
}
