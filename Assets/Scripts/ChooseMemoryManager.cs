using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseMemoryManager : MonoBehaviour
{
    public Button HGirlButton1;
    public Button HGirlButton2;
    public Button HGirlButton3;
    public Button HGirlButton4;

    public GameObject LockHGirlButton1;
    public GameObject LockHGirlButton2;
    public GameObject LockHGirlButton3;
    public GameObject LockHGirlButton4;

    public Button BackButton;


    void Start()
    {
        LoadStart();

        // 連接按鈕事件
        HGirlButton1.onClick.AddListener(LoadOneMemory);
        HGirlButton2.onClick.AddListener(LoadTwoMemory);
        HGirlButton3.onClick.AddListener(LoadThreeMemory);
        HGirlButton4.onClick.AddListener(LoadFourMemory);

        BackButton.onClick.AddListener(LoadSceneGameMenu);
    }

    private void LoadStart()
    {
        if (PlayerManager.Instance.userData.level == 1)
        {
            AllHGirlButtonClose();
            HGirlButton1.gameObject.SetActive(true);
        }
        else if (PlayerManager.Instance.userData.level == 2)
        {
            AllHGirlButtonClose();
            HGirlButton2.gameObject.SetActive(true);
        }
        else if (PlayerManager.Instance.userData.level == 3)
        {
            AllHGirlButtonClose();
            HGirlButton3.gameObject.SetActive(true);
        }
        else if (PlayerManager.Instance.userData.level == 4)
        {
            AllHGirlButtonClose();
            HGirlButton4.gameObject.SetActive(true);
        }
    }

    private void LoadOneMemory()
    {
        LevelManager.instance.golevel = 1;
        SceneManager.LoadScene("HGame");
    }

    private void LoadTwoMemory()
    {
        LevelManager.instance.golevel = 2;
        SceneManager.LoadScene("HGame");
    }

    private void LoadThreeMemory()
    {
        LevelManager.instance.golevel = 3;
        SceneManager.LoadScene("HGame");
    }

    private void LoadFourMemory()
    {
        LevelManager.instance.golevel = 4;
        SceneManager.LoadScene("HGame");
    }

    private void LoadSceneGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    private void AllHGirlButtonClose()
    {
        HGirlButton1.gameObject.SetActive(false);
        HGirlButton2.gameObject.SetActive(false);
        HGirlButton3.gameObject.SetActive(false);
        HGirlButton4.gameObject.SetActive(false);

        LockHGirlButton1.gameObject.SetActive(true);
        LockHGirlButton2.gameObject.SetActive(true);
        LockHGirlButton3.gameObject.SetActive(true);
        LockHGirlButton4.gameObject.SetActive(true);
    }


}
