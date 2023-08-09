using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public Button englishButton;
    public Button chineseButton;

    private void Start()
    {
        englishButton.onClick.AddListener(OnEnglishButtonClick);
        chineseButton.onClick.AddListener(OnChineseButtonClick);
    }

    private void OnEnglishButtonClick()
    {
        LanguageManager.Instance.SetLanguage("en");
        PlayerManager.Instance.userData.language = 1;
    }

    private void OnChineseButtonClick()
    {
        LanguageManager.Instance.SetLanguage("ch");
        PlayerManager.Instance.userData.language = 2;
    }
}
