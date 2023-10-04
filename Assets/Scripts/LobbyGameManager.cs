using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyGameManager : MonoBehaviour
{

    public GameObject GameMenu;
    public GameObject GameLoad;
    public GameObject Setting;

    public void OnBtnLoadGameClick()
    {
        GameLoad.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void OnBtnLoadBackClick()
    {
        GameLoad.SetActive(false);
        GameMenu.SetActive(true);
    }

    public void OnBtnSettingClick()
    {
        GameMenu.SetActive(false);
        Setting.SetActive(true);
    }

    public void OnBtnSettingBackClick()
    {
        Setting.SetActive(false);
        GameMenu.SetActive(true);
    }

    public void LoadSceneChooseRole()
    {
        SceneManager.LoadScene("ChooseRole");
    }

    public void LoadSceneChooseMemory()
    {
        SceneManager.LoadScene("ChooseMemory");
    }

    public void ResetGame()
    {
        PlayerPrefs.SetInt("PlayerFish", 0);
        PlayerManager.Instance.userData.level = 0;

        PlayerManager.Instance.SaveUserData();
    }
}
