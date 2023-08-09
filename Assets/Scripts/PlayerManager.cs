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

        if (PlayerPrefs.HasKey("userData"))
        {

            string jsonData = PlayerPrefs.GetString("userData");
            userData = JsonUtility.FromJson<UserData>(jsonData);
        }
        else
        {

            userData = new UserData
            {
                language = 1,
                level = 1,
                fishtime = 0f
            };
        }

 
        userData.language = 1;
        userData.level = 2;
        userData.fishtime = 0f;

        if (userData.language == 1)
        {
            LanguageManager.Instance.SetLanguage("en");
        }
        else if (userData.language == 2)
        {
            LanguageManager.Instance.SetLanguage("ch");
        }

        string updatedJsonData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("userData", updatedJsonData);
        PlayerPrefs.Save();
    }

    public void SaveUserData()
    {
        string updatedJsonData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("userData", updatedJsonData);
        PlayerPrefs.Save();
    }

}



[System.Serializable]
public class UserData
{
    public int language = 1;
    public int level = 1;
    public float fishtime = 0f;
}
