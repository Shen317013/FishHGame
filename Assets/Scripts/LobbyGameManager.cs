using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyGameManager : MonoBehaviour
{

    private AudioManager audioManager; // 用于存储对 AudioManager 的引用

    private void Start()
    {
        // 在 Start 方法中查找 AudioManager 并引用它
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void OnBtnLoadGameClick()
    {
        SceneManager.LoadScene("GameLoad");
    }

    public void OnBtnSettingClick()
    {
        SceneManager.LoadScene("Setting");
    }


    public void LoadSceneChooseRole()
    {
        SceneManager.LoadScene("ChooseRole");
    }

    public void LoadSceneChooseMemory()
    {
        SceneManager.LoadScene("ChooseMemory");
    }

    // 调用 AudioManager 的 PlaySound 方法
    public void PlaySound1()
    {
        if (audioManager != null)
        {
            audioManager.PlaySound1();
        }
        else
        {
            Debug.LogWarning("AudioManager not found.");
        }
    }

    public void PlaySound2()
    {
        if (audioManager != null)
        {
            audioManager.PlaySound2();
        }
        else
        {
            Debug.LogWarning("AudioManager not found.");
        }
    }
}
