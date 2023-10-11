using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BigFishGame : MonoBehaviour
{
    public GameObject[] enemyHealth;
    public GameObject[] enemyHealthBroke;
    private int Heath;

    public RawImage[] objects;
    public Texture2D[] directions;
    private List<int> currentSequence = new List<int>();

    private int currentIndex = 0;
    private Coroutine gameTimerCoroutine;
    private bool isGameActive = true;

    public GameObject upObject;
    public GameObject downObject;
    public GameObject leftObject;
    public GameObject rightObject;


    void Start()
    {
        Heath = 3;
        AssignRandomTextures();
        gameTimerCoroutine = StartCoroutine(GameTimer());
    }

    void Update()
    {
        if (!isGameActive) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("上");
            ShowObject(upObject);
            CheckInput(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("下");
            ShowObject(downObject);
            CheckInput(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("左");
            ShowObject(leftObject);
            CheckInput(2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("右");
            ShowObject(rightObject);
            CheckInput(3);
        }
    }

    void AssignRandomTextures()
    {
        currentSequence.Clear();

        for (int i = 0; i < objects.Length; i++)
        {
            int randomIndex = Random.Range(0, directions.Length);
            objects[i].texture = directions[randomIndex];
            currentSequence.Add(randomIndex);
        }
    }

    void CheckInput(int input)
    {
        if (currentSequence[currentIndex] == input)
        {
            currentIndex++;

            if (currentIndex == currentSequence.Count)
            {
                HealthDecrease();

                // 重置時間並立即分配新的隨機圖片
                ResetTimer();
                AssignRandomTextures();
                currentIndex = 0;
            }
        }
        else
        {
            GameLose();
        }
    }

    void HealthDecrease()
    {
        Heath--;

        if (Heath >= 0 && Heath < enemyHealth.Length)
        {
            enemyHealth[Heath].SetActive(false);
            enemyHealthBroke[Heath].SetActive(true);
        }

        if (Heath == 0)
        {
            GameWin();
        }
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(5f);

        if (isGameActive && currentIndex < currentSequence.Count)
        {
            GameLose();
        }
    }

    void ResetTimer()
    {
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
        }
        gameTimerCoroutine = StartCoroutine(GameTimer());
    }

    void GameWin()
    {
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
            gameTimerCoroutine = null;
        }

        isGameActive = false;
        Debug.Log("遊戲勝利");

        PlayerManager.Instance.userData.fish += 5;
        SceneManager.LoadScene("FishingChoose");
    }

    void GameLose()
    {
        if (gameTimerCoroutine != null)
        {
            StopCoroutine(gameTimerCoroutine);
            gameTimerCoroutine = null;
        }

        isGameActive = false;
        Debug.Log("遊戲失敗");
    }

    private void ShowObject(GameObject obj)
    {
        //顯示滑鼠點擊圖片 0.2秒後隱藏
        obj.SetActive(true);
        StartCoroutine(HideObjectAfterDelay(obj, 0.2f));
    }

    private IEnumerator HideObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
