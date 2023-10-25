using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource soundSource;
    public AudioClip soundEffect1; // 第一个音效
    public AudioClip soundEffect2; // 第二个音效
    private bool isSoundOn = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 防止销毁
            soundSource = GetComponent<AudioSource>();
            soundSource.clip = soundEffect1;
            soundSource.enabled = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound1()
    {
        if (isSoundOn)
        {
            soundSource.clip = soundEffect1;
            soundSource.Play();
        }
    }

    public void PlaySound2()
    {
        if (isSoundOn)
        {
            soundSource.clip = soundEffect2;
            soundSource.Play();
        }
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        // 使用 PlayerPrefs 保存音效偏好设置
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save(); // 保存设置
    }
}
