using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public UserData userData;

    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (LanguageManager.Instance == null)
        {
            GameObject languageManagerObj = new GameObject("LanguageManager");
            LanguageManager.Instance = languageManagerObj.AddComponent<LanguageManager>();
            LanguageManager.Instance.LoadLanguage();
        }

    }

    private void Start()
    {
        userData = new UserData
        {
            language = 1,
            level = 0,
            fishtime = 0f,
            fish = 0,
            screen = 1
        };
    }

    private void Update()
    {
        if (userData.language == 1)
        {
            LanguageManager.Instance.SetLanguage("en");
        }
        else if (userData.language == 2)
        {
            LanguageManager.Instance.SetLanguage("ch");
        }

        if (userData.screen == 1)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else if (userData.screen == 2)
        {
            Screen.SetResolution(1920, 1080, false);
        }
    }

    public void SaveUserData()
    {
        string updatedJsonData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("userData", updatedJsonData);
        PlayerPrefs.Save();
    }

    public void LoadUserData()
    {
        string jsonData = PlayerPrefs.GetString("userData");
        userData = JsonUtility.FromJson<UserData>(jsonData);
    }

    public void DeleteUserData()
    {
        // 删除UserData存档
        PlayerPrefs.DeleteKey("userData");
        // 重置UserData
        userData = new UserData();
    }

}



[System.Serializable]
public class UserData
{
    public int language = 1;
    public int level = 0;
    public float fishtime = 0f;
    public int fish = 0;
    public int screen = 1;
}
