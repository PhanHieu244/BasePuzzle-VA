using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PSoundButton : MonoBehaviour 
{
	// The button to turn on/off sound.
	public Button btnSound;	
	// Image of the button on which sound sprite will get assigned. Default on
	public Image btnSoundImage; 
	// Sound on sprite.
	public Sprite soundOnSprite;
	// Sounf off sprite.
	public Sprite soundOffSprite;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		btnSound.onClick.AddListener(() => 
		{
			if (PInputManager.Instance.CanInput ()) {
				PAudioManager.Instance.PlayButtonClickSound ();
				PAudioManager.Instance.ToggleSoundStatus();
			}
		});
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		PAudioManager.OnSoundStatusChangedEvent += OnSoundStatusChanged;
		initSoundStatus ();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		PAudioManager.OnSoundStatusChangedEvent -= OnSoundStatusChanged;
	}

	/// <summary>
	/// Inits the sound status.
	/// </summary>
	void initSoundStatus()
	{
		btnSoundImage.sprite = (PAudioManager.Instance.isSoundEnabled) ? soundOnSprite : soundOffSprite;
	}

	/// <summary>
	/// Raises the sound status changed event.
	/// </summary>
	/// <param name="isSoundEnabled">If set to <c>true</c> is sound enabled.</param>
	void OnSoundStatusChanged (bool isSoundEnabled)
	{
		btnSoundImage.sprite = (isSoundEnabled) ? soundOnSprite : soundOffSprite;
	}	
}
