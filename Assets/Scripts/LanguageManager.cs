using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; set; }

    private Dictionary<string, Dictionary<string, string>> languageDictionary;
    private string currentLanguage = "ch"; 

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
        }
    }

    public void LoadLanguage()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "language.json");

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            LanguageData languageData = JsonUtility.FromJson<LanguageData>(jsonString);

            if (languageData != null)
            {
                languageDictionary = new Dictionary<string, Dictionary<string, string>>();

                foreach (LanguageItem item in languageData.languages)
                {
                    Dictionary<string, string> translations = new Dictionary<string, string>();

                    foreach (Translation translation in item.translations)
                    {
                        translations[translation.key] = translation.value;
                    }

                    languageDictionary[item.languageCode] = translations;
                }

                UpdateText();
            }
            else
            {
                Debug.LogError("Language data is null");
            }
        }
        else
        {
            Debug.LogError("Json file not found: " + jsonFilePath);
        }
    }

    public void SetLanguage(string languageCode)
    {
        if (languageDictionary.ContainsKey(languageCode))
        {
            currentLanguage = languageCode;
            UpdateText();
        }
        else
        {
            Debug.LogWarning("Language not found: " + languageCode);
        }
    }

    public string GetTranslationForKey(string key)
    {
        if (languageDictionary.ContainsKey(currentLanguage))
        {
            Dictionary<string, string> translations = languageDictionary[currentLanguage];

            if (translations.ContainsKey(key))
            {
                return translations[key];
            }
            else
            {
                Debug.LogWarning("Translation not found for key: " + key);
            }
        }
        else
        {
            Debug.LogWarning("Language not found: " + currentLanguage);
        }

        return string.Empty;
    }

    public void UpdateText()
    {
        LocalizedText[] localizedTexts = FindObjectsOfType<LocalizedText>();

        if (localizedTexts != null)
        {
            foreach (LocalizedText localizedText in localizedTexts)
            {
                localizedText.UpdateText();
            }
        }
        else
        {
            Debug.LogWarning("No LocalizedText components found");
        }
    }


    [System.Serializable]
    public class LanguageData
    {
        public LanguageItem[] languages;
    }

    [System.Serializable]
    public class LanguageItem
    {
        public string languageCode;
        public Translation[] translations;
    }

    [System.Serializable]
    public class Translation
    {
        public string key;
        public string value;
    }
}
