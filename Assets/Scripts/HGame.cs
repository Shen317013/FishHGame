using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class HGame : MonoBehaviour
{
    public Image m_Image;
    public Text m_Text;
    public Text m_Speed;
    float m_CurProgressValue = 0;
    float m_TargetProgressValue = 100;
    float m_ProgressSpeed = 1.0f;
    float progressSpeed;
    public float newSpeed = 1.5f;

    private float totalScrollAmount = 0.0f;
    public float ScrollOneLow = 2.0f;
    public float ScrollOneHigh = 10.0f;
    public float ScrollTwoLow = 5.0f;
    public float ScrollTwoHigh = 12.0f;
    public float ScrollThreeLow = 8.0f;
    public float ScrollThreeHigh = 18.0f;

    public int Hlevel = 1;

    private bool hasResetScrollAmountOne = false;
    private bool hasResetScrollAmountTwo = false;

    public Button resetButton1;
    public Button resetButton2;
    public Button resetButton3;
    public Button shotButton;

    public SkeletonGraphic HGirlA1;
    public SkeletonGraphic HGirlA2;


    public int Hstage = 0;

    public Button BackButton;


    void Update()
    {
        HandleGameProgress();
        HandleGameButton();
    }

    private void Start()
    {
        // 連接按鈕事件
        resetButton1.onClick.AddListener(ResetProgress1);
        resetButton2.onClick.AddListener(ResetProgress2);
        resetButton3.onClick.AddListener(ResetProgress3);
        shotButton.onClick.AddListener(ShotSemen);
        BackButton.onClick.AddListener(LoadSceneGameMenu);

    }

    private void ResetProgress1() // 點擊按鈕時重置進度條並執行HandleGameProgress()
    {
        m_CurProgressValue = 0;
        totalScrollAmount = 0;
        hasResetScrollAmountOne = false;
        hasResetScrollAmountTwo = false;
        HandleGameProgress();

        if (LevelManager.instance.golevel == 1) {
            AllHGirlClose();
            HGirlA1.gameObject.SetActive(true);
        }
    }

    private void ResetProgress2() // 點擊按鈕時重置進度條並執行HandleGameProgress()
    {
        m_CurProgressValue = 0;
        totalScrollAmount = 0;
        hasResetScrollAmountOne = false;
        hasResetScrollAmountTwo = false;
        HandleGameProgress();

        if (LevelManager.instance.golevel == 1 && Hlevel == 2)
        {
            AllHGirlClose();
            HGirlA2.gameObject.SetActive(true);
        }
    }

    private void ResetProgress3() // 點擊按鈕時重置進度條並執行HandleGameProgress()
    {
        m_CurProgressValue = 0;
        totalScrollAmount = 0;
        hasResetScrollAmountOne = false;
        hasResetScrollAmountTwo = false;
        HandleGameProgress();
    }

    private void ShotSemen()
    {
        //   LevelManager.instance.golevel = 0;
        Hstage = 0;
        SceneManager.LoadScene("EndMessage");
    }

    private void HandleGameButton()
    {
        if (Hstage == 0)
        {
            resetButton1.gameObject.SetActive(true);
            resetButton2.gameObject.SetActive(false);
            resetButton3.gameObject.SetActive(false);
        }
        else if (Hstage == 1)
        {
            resetButton1.gameObject.SetActive(true);
            resetButton2.gameObject.SetActive(true);
            resetButton3.gameObject.SetActive(false);
        }
        else if (Hstage == 2)
        {
            resetButton1.gameObject.SetActive(true);
            resetButton2.gameObject.SetActive(true);
            resetButton3.gameObject.SetActive(true);
        }
    }

        private void HandleGameProgress()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        totalScrollAmount += scrollInput;
        m_Speed.text = totalScrollAmount.ToString("F1");

        if (scrollInput != 0)
        {
            Debug.Log("Total Scroll Amount: " + totalScrollAmount);
        }

        if (m_CurProgressValue < 50)
        {
            HandleFirstPhase();
        }
        else if (m_CurProgressValue >= 50 && m_CurProgressValue < 75)
        {
            Hlevel = 2;
            HandleSecondPhase();
        }
        else if (m_CurProgressValue >= 75)
        {
            Hlevel = 3;
            HandleThirdPhase();
        }

        UpdateProgressValue();
    }

    private void HandleFirstPhase()     //第一階段
    {
        if (totalScrollAmount >= ScrollOneLow && totalScrollAmount <= ScrollOneHigh)
        {
            progressSpeed = m_ProgressSpeed + 2;
            m_Speed.text = "甜蜜點1";
            Debug.Log("找到甜蜜點");
            Spine.AnimationState animationState = HGirlA1.AnimationState;
            animationState.TimeScale = newSpeed;

        }
        else
        {
            progressSpeed = m_ProgressSpeed;
        }
    }

    private void HandleSecondPhase()    //第二階段
    {
        if (!hasResetScrollAmountOne)
        {
            totalScrollAmount = 0;
            hasResetScrollAmountOne = true;
        }

        if (totalScrollAmount >= ScrollTwoLow && totalScrollAmount <= ScrollTwoHigh)
        {
            progressSpeed = m_ProgressSpeed + 2;
            m_Speed.text = "甜蜜點2";
            Debug.Log("找到第二個甜蜜點");
        }
        else
        {
            progressSpeed = m_ProgressSpeed;
        }
    }

    private void HandleThirdPhase()     //第三階段
    {
        if (!hasResetScrollAmountTwo)
        {
            totalScrollAmount = 0;
            hasResetScrollAmountTwo = true;
        }

        if (totalScrollAmount >= ScrollThreeLow && totalScrollAmount <= ScrollThreeHigh)
        {
            progressSpeed = m_ProgressSpeed + 2;
            m_Speed.text = "甜蜜點3";
            Debug.Log("找到第三個甜蜜點");
        }
        else
        {
            progressSpeed = m_ProgressSpeed;
        }
    }

    private void UpdateProgressValue()      //進度條進度
    {
        m_CurProgressValue += progressSpeed * Time.deltaTime;
        m_CurProgressValue = Mathf.Clamp(m_CurProgressValue, 0, m_TargetProgressValue);
        m_Text.text = Mathf.RoundToInt(m_CurProgressValue) + "%";
        m_Image.fillAmount = m_CurProgressValue / m_TargetProgressValue;

        if (Mathf.Approximately(m_CurProgressValue, m_TargetProgressValue) && Hstage > 1)
        {
            m_Text.text = "OK";
            m_Speed.gameObject.SetActive(false);
            shotButton.gameObject.SetActive(true);
            // 這裡可以射邏輯
        } else if (Mathf.Approximately(m_CurProgressValue, m_TargetProgressValue))
        {
            m_CurProgressValue = 0;
            totalScrollAmount = 0;
            hasResetScrollAmountOne = false;
            hasResetScrollAmountTwo = false;
            Hstage++;
            HandleGameProgress();
        }
    }

    private void AllHGirlClose()
    {
        HGirlA1.gameObject.SetActive(false);
        HGirlA2.gameObject.SetActive(false);
    }

    private void LoadSceneGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
