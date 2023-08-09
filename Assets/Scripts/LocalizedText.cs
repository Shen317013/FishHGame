using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    public string key;

    private Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (textComponent != null)
        {
            if (LanguageManager.Instance != null)
            {
                string translation = LanguageManager.Instance.GetTranslationForKey(key);
                textComponent.text = translation;
            }
        }

    }

}
