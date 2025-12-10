using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DxCoder;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour {

    public TextMeshProUGUI CurrentLevelText;
    public Sprite SoundOn, SoundOff;
    public GameObject HandleOff, HandleOn;
    public GameObject button;
    [Header("Rate URL")]
    public string GameURl;
    public string MoreGamesURL;
    public string WebLink;
    int CurrentLevel;
    public Slider soundSlider;


    // Use this for initialization
    void Start () {
        // Set up event listeners for the sliders
        soundSlider.onValueChanged.AddListener(delegate { OnSoundVolumeChanged(); });

        // Load the saved sound and music volume settings
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic(SoundManager.Instance.MenuMusic);
        CurrentLevel = PlayerPrefs.GetInt("Level", 1);
        CurrentLevelText.text = "LEVEL " + CurrentLevel;
    }



    public void StartLoadGame() {
        PlaySound();
        SceneManager.LoadScene("Loading");
    }


    public void ShareClick()
    {
        PlaySound();

        StartCoroutine(StartShare());
    }

    IEnumerator StartShare()
    {
        yield return new WaitForEndOfFrame();
       // new NativeShare().SetSubject(Subject).SetText(Body).Share();
    }

    public void MuteSound()
    {
        if (SoundManager.Instance.IsMuted())
        {
            button.GetComponent<Image>().sprite = SoundOn;
            HandleOn.SetActive(true);
            HandleOff.SetActive(false);
        }
        else
        {
            button.GetComponent<Image>().sprite = SoundOff;
            HandleOn.SetActive(false);
            HandleOff.SetActive(true);


        }
        SoundManager.Instance.ToggleMute();

    }

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

    }
    public void PlayWooshSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

    }
    public void Rateus()
    {
        PlaySound();
        Application.OpenURL(GameURl);
    }

    public void MoreGames()
    {
        PlaySound();
        Application.OpenURL(MoreGamesURL);
    }

    public void OpenWeb()
    {
        PlaySound();
        Application.OpenURL(WebLink);
    }

    public void OnSoundVolumeChanged()
    {
        // Update the sound volume setting and the audio listener's volume
        float volume = soundSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", volume);
        AudioListener.volume = volume;
    }
}
