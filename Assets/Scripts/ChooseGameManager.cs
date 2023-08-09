using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseGameManager : MonoBehaviour
{
    public void LoadSceneGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void LoadSceneStory()
    {
        SceneManager.LoadScene("RoleStory");
        LevelManager.instance.golevel = 1;
    }
}
