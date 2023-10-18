using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyGameManager : MonoBehaviour
{
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
}
