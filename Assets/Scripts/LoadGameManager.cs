using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameManager : MonoBehaviour
{
    public void OnBtnLoadBackClick()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void OnBtnSaveClick()
    {
        // «O¦sUserData
        PlayerManager.Instance.SaveUserData();
    }

    public void OnBtnLoadClick()
    {
        PlayerManager.Instance.LoadUserData();
        OnBtnLoadBackClick();
    }

    public void OnBtnDeleteClick()
    {
        PlayerManager.Instance.DeleteUserData();
    }
}
