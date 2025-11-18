using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PBackgroundMusic : PSingleton<PBackgroundMusic> 
{
	[SerializeField] private AudioClip BGMusicMain;
	private AudioSource CurrentAudioSource;

	void Awake()
	{
		//Init audio source to play background music.
		CurrentAudioSource = GetComponent<AudioSource> ();
	}

	void Start()
	{
		Invoke ("StartBGMusic",1F);
	}

	void OnEnable()
	{
		//Register Audio status event
		PAudioManager.OnMusicStatusChangedEvent += OnMusicStatusChangedEvent;
	}


	void OnDisable()
	{
		//Unregister Audio status event
		PAudioManager.OnMusicStatusChangedEvent -= OnMusicStatusChangedEvent;
	}

	void StartBGMusic()
	{
		///Start playing music is music setting is enabled.
		if (PAudioManager.Instance.isMusicEnabled && !CurrentAudioSource.isPlaying ) {
			CurrentAudioSource.clip = BGMusicMain;
			CurrentAudioSource.loop = true;
			CurrentAudioSource.Play ();
		}
	}

	/// <summary>
	/// Pauses the background music.
	/// </summary>
	public void PauseBGMusic()
	{
		if (CurrentAudioSource.isPlaying) {
			CurrentAudioSource.Pause ();
		}
	}

	/// <summary>
	/// Resumes the background music.
	/// </summary>
	public void ResumeBGMusic()
	{
		if (!CurrentAudioSource.isPlaying) {
			CurrentAudioSource.Play ();
		}
	}

	/// <summary>
	/// Raises the music status changed event event.
	/// </summary>
	/// <param name="isMusicEnabled">If set to <c>true</c> is music enabled.</param>
	void OnMusicStatusChangedEvent (bool isMusicEnabled)
	{
		if (!isMusicEnabled) {
			PauseBGMusic ();
		} else {
			ResumeBGMusic ();
		}
	}
}
