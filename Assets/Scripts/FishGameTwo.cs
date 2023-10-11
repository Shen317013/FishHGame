using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishGameTwo : MonoBehaviour
{
    public Text countdownText;      //倒數計時文本
    public GameObject pointerObject;        //指針物件
    public GameObject safeObject;           //判定物件
    public float moveDistanceRight = 35f;    //向右移動距離
    public float moveDistanceLeft = 7f;     //向左移動距離
    public float continuousClickTime = 0.2f;   //監聽連續點擊按鈕時間
    public float continuousMoveTime = 0.1f;    //監聽連續移動物件時間
    public int minMoveCount = 1;    //最小移動次數(向左
    public int maxMoveCount = 3;    //最大移動次數(向左
    public float stopLeftPosition = -1f;
    public float minLeftPosition = 300f; // 最小允許的 x 軸位置
    public Button button;
    public Button pauseButton; // 暫停按鈕
    public Button resetButton; // 重置按鈕

    private bool isButtonPressed = false;
    private bool isCountingDown = false;
    private float countdownTime = 8f;   //倒數計時時間
    private float lastClickTime = 0f;   //上次點擊按鈕時間
    private float lastMoveTime = 0f;    //上次移動時間
    private int moveCount = 0;  //當前移動次數
    private bool isTimeUp = false;

    private bool isPaused = false; // 是否遊戲暫停
    private Vector3 initialPointerPosition;

    private void Start()
    {
        button.onClick.AddListener(OnButtonPress);
        countdownText.text = countdownTime.ToString("F0");

        // 儲存初始指標位置
        initialPointerPosition = pointerObject.transform.position;

        // 監聽暫停按鈕的點擊事件
        pauseButton.onClick.AddListener(TogglePause);

        // 監聽重置按鈕的點擊事件
        resetButton.onClick.AddListener(ResetGame);

        // 自動開始倒數計時
        StartCountdown();
    }

    private void Update()
    {
        if (isCountingDown)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text = countdownTime.ToString("F0");

            if (countdownTime <= 0f)
            {
                isCountingDown = false;
                isTimeUp = true;
                CheckCollision();
            }
        }

        if (!isTimeUp)
        {
            if (isButtonPressed)
            {
                MovePointer(moveDistanceRight);
                lastClickTime = Time.time;
                isButtonPressed = false;

                if (!isCountingDown)
                {
                    isCountingDown = true;
                }
            }
            else if (Time.time - lastClickTime >= continuousClickTime && Time.time - lastMoveTime >= continuousMoveTime)
            {
                int randomMoveCount = Random.Range(minMoveCount, maxMoveCount + 1);
                float moveDistance = -moveDistanceLeft * randomMoveCount;
                float targetPosition = pointerObject.transform.position.x + moveDistance;

                // 檢查目標位置是否超出最小允許位置
                if (targetPosition >= stopLeftPosition && targetPosition >= minLeftPosition)
                {
                    MovePointer(moveDistance);
                    lastMoveTime = Time.time;
                    moveCount = randomMoveCount;
                }
            }
        }
    }

    private void MovePointer(float distance)
    {
        pointerObject.transform.Translate(distance, 0f, 0f);
    }

    public void OnButtonPress()
    {
        isButtonPressed = true;
    }

    private void CheckCollision()
    {
        Collider2D pointerCollider = pointerObject.GetComponent<Collider2D>();
        Collider2D safeCollider = safeObject.GetComponent<Collider2D>();
        bool isColliding = pointerCollider.IsTouching(safeCollider);

        if (isColliding)
        {
            GameOver(true);
        }
        else
        {
            GameOver(false);
        }
    }

    private void GameOver(bool isWin)
    {
        if (isWin)
        {
            Debug.Log("遊戲勝利!");
            // 在這裡處理遊戲勝利的邏輯

            PlayerManager.Instance.userData.fish += 1;
            SceneManager.LoadScene("FishingChoose");
        }
        else
        {
            Debug.Log("遊戲失敗!");
            // 在這裡處理遊戲失敗的邏輯
        }
    }

    private void StartCountdown()
    {
        isCountingDown = true;
    }

    public void ResetGame()
    {
        // 重置指標位置
        pointerObject.transform.position = initialPointerPosition;

        // 重置倒計時時間
        countdownTime = 8f;
        countdownText.text = countdownTime.ToString("F0");

        // 重置其他遊戲狀態和變數
        isCountingDown = false;
        isTimeUp = false;
        lastClickTime = 0f;
        lastMoveTime = 0f;
        moveCount = 0;

        StartCountdown();
    }

    // 暫停或恢復遊戲的方法
    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; // 暫停遊戲時間
            Debug.Log("遊戲已暫停");
        }
        else
        {
            Time.timeScale = 1f; // 恢復遊戲時間
            Debug.Log("遊戲已恢復");
        }
    }
}











