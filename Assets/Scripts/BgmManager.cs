using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BgmManager : MonoBehaviour
{
    public static BgmManager instance;

    public AudioClip gameMenuBGM; // GameMenu场景的BGM
    public AudioClip chooseRoleBGM; // ChooseRole场景的BGM
    public AudioSource audioSource;

    private bool isBgmOn = true;

    private string currentSceneName = "";

    private void Start()
    {
        // 获取当前场景名称
        currentSceneName = SceneManager.GetActiveScene().name;

        // 播放适当的 BGM
        PlayBGMForScene(currentSceneName);
    }

    private void Update()
    {
        // 检查当前场景是否发生变化
        string newSceneName = SceneManager.GetActiveScene().name;

        if (newSceneName != currentSceneName)
        {
            // 场景发生变化，更新当前场景名称并播放相应的 BGM
            currentSceneName = newSceneName;
            PlayBGMForScene(currentSceneName);
        }
    }

    private void PlayBGMForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "GameMenu":
                PlayBGM(gameMenuBGM);
                break;
            case "ChooseRole":
                PlayBGM(chooseRoleBGM);
                break;
            default:
                PlayDefaultBGM();
                break;
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent <AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip bgm)
    {
        if (isBgmOn)
        {
            audioSource.clip = bgm;
            audioSource.Play();
        }
    }


    private void PlayDefaultBGM()
    {
        // 在其他场景播放默认BGM或采取其他适当的措施
    }

    public void ToggleBgm()
    {
        isBgmOn = !isBgmOn;

        // 使用 PlayerPrefs 保存音效偏好设置
        PlayerPrefs.SetInt("BgmOn", isBgmOn ? 1 : 0);
        PlayerPrefs.Save(); // 保存设置

        if (isBgmOn)
        {
            // 如果 BGM 打开，播放当前场景的 BGM
            PlayBGM(GetCurrentSceneBGM());
        }
        else
        {
            // 如果 BGM 关闭，停止播放当前的 BGM
            audioSource.Stop();
        }
    }

    private AudioClip GetCurrentSceneBGM()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        switch (currentSceneName)
        {
            case "GameMenu":
                return gameMenuBGM;
            case "ChooseRole":
                return chooseRoleBGM;
            // 添加其他场景的BGM
            default:
                // 如果未在上面的 case 中匹配到场景，可以返回一个默认的BGM 或者 null
                return null;
        }
    }
}
