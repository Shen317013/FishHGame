using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Text TxtLanguage;
    public Text TxtScreen;

    private AudioManager audioManager; // 用于存储对 AudioManager 的引用
    public GameObject BgmOff;
    public GameObject BgmOn;
    public GameObject SoundOff;
    public GameObject SoundOn;

    public void SetLanguageAndRefreshText(int languageCode)
    {
        PlayerManager.Instance.userData.language = languageCode;
        PlayerManager.Instance.SaveUserData();

        if (languageCode == 1)
        {
            LanguageManager.Instance.SetLanguage("en");
            TxtLanguage.text = "English";
        }
        else if (languageCode == 2)
        {
            LanguageManager.Instance.SetLanguage("ch");
            TxtLanguage.text = "繁體中文";
        }
    }

    public void SetScreenAndRefreshText(int screenCode)
    {
        PlayerManager.Instance.userData.screen = screenCode;
        PlayerManager.Instance.SaveUserData();

        if (screenCode == 1)
        {
            TxtScreen.text = "視窗化";
        }
        else if (screenCode == 2)
        {
            TxtScreen.text = "全螢幕";
        }
    }


    // 在 Start 方法中调用 SetLanguageAndRefreshText
    private void Start()
    {
        SetLanguageAndRefreshText(PlayerManager.Instance.userData.language);
        SetScreenAndRefreshText(PlayerManager.Instance.userData.screen);

        // 在 Start 方法中查找 AudioManager 并引用它
        audioManager = FindObjectOfType<AudioManager>();

        // 获取 SoundOn 和 SoundOff 状态
        int soundOnValue = PlayerPrefs.GetInt("SoundOn", 1); // 默认值为1

        // 根据存储的值设置 SoundOn 和 SoundOff 游戏对象的状态
        if (soundOnValue == 1)
        {
            SoundOn.SetActive(true);
            SoundOff.SetActive(false);
        }
        else
        {
            SoundOff.SetActive(true);
            SoundOn.SetActive(false);
        }
    }

    // 在 OnBtnLanguageClick 方法中调用 SetLanguageAndRefreshText
    public void OnBtnLanguageClick()
    {
        if (PlayerManager.Instance.userData.language == 1)
        {
            SetLanguageAndRefreshText(2);
        }
        else if (PlayerManager.Instance.userData.language == 2)
        {
            SetLanguageAndRefreshText(1);
        }
    }

    public void LoadSceneGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }


    public void OnBtnScreenClick()
    {

        if (PlayerManager.Instance.userData.screen == 1)
        {
            SetScreenAndRefreshText(2);
        }
        else if(PlayerManager.Instance.userData.screen == 2)
        {
            SetScreenAndRefreshText(1);
        }
    }

    public void ToggleSound()
    {
        if (audioManager != null)
        {
            audioManager.ToggleSound();
        }
        else
        {
            Debug.LogWarning("AudioManager not found.");
        }
    }

    public void OnBtnSoundOnClick()
    {
        SoundOff.SetActive(true);
        SoundOn.SetActive(false);
    }

    public void OnBtnSoundOffClick()
    {
        SoundOn.SetActive(true);
        SoundOff.SetActive(false);
    }
}
