using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishGameOne : MonoBehaviour
{
    public float moveSpeed = 2f;  // 移動速度
    public float moveDistance = 5f;  // 移動距離
    public GameObject fishObject;  // 魚的遊戲物件
    public GameObject imageObject;  // 圖片的遊戲物件
    public float minX = -5f; // 圖片的最小X軸位置
    public float maxX = 5f; // 圖片的最大X軸位置
    public Button stopButton; // 停止按鈕
    public Button resetButton; // 重置按鈕
    public Button pauseButton; // 暫停按鈕
    public float countdownDuration = 10f; // 倒數計時的總時長
    public Text countdownText; // 顯示倒數計時的文本

    private Vector3 fishOriginalPosition;
    private float fishMoveDirection = 1f;  // 魚的移動方向（1代表向右，-1代表向左）
    private bool isFishMoving = true; // 魚是否正在移動
    private float currentTime; // 當前倒數計時的時間
    private bool isGameLost = false; // 是否遊戲失敗

    private bool isPaused = false; // 是否遊戲暫停

    private bool isCountdownPaused = false; // 倒數計時是否暫停

    private void Start()
    {
        fishOriginalPosition = fishObject.transform.position;
        SetRandomImagePosition();

        // 監聽停止按鈕的點擊事件
        stopButton.onClick.AddListener(CheckGameStatus);

        // 監聽重置按鈕的點擊事件
        resetButton.onClick.AddListener(ResetGame);

        // 監聽暫停按鈕的點擊事件
        pauseButton.onClick.AddListener(TogglePause);

        // 初始化倒數計時
        currentTime = countdownDuration;
    }

    private void Update()
    {
        if (isPaused)
        {
            // 遊戲暫停時的邏輯處理
            return;
        }


        if (isFishMoving)
        {
            // 魚的移動
            float fishTargetX = fishOriginalPosition.x + fishMoveDirection * moveDistance;
            fishObject.transform.position = Vector3.MoveTowards(fishObject.transform.position, new Vector3(fishTargetX, fishOriginalPosition.y, fishOriginalPosition.z), moveSpeed * Time.deltaTime);

            // 檢查是否到達魚的目標位置，如果是則改變移動方向
            if (Vector3.Distance(fishObject.transform.position, new Vector3(fishTargetX, fishOriginalPosition.y, fishOriginalPosition.z)) <= 0.01f)
            {
                fishMoveDirection *= -1f;
            }
        }

        if (!isCountdownPaused)
        {
            // 更新倒數計時
            currentTime -= Time.deltaTime;

            // 檢查是否倒數計時結束
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                if (!isGameLost)
                {
                    isGameLost = true;
                    // 在這裡添加遊戲失敗的處理邏輯
                    StopFishMovement();
                    Debug.Log("遊戲失敗！");
                }
            }

            // 更新倒數計時文本顯示
            countdownText.text = currentTime.ToString("F0");

        }
    }

    private void SetRandomImagePosition()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 newPosition = new Vector3(randomX, imageObject.transform.position.y, imageObject.transform.position.z);
        imageObject.transform.position = newPosition;
    }

    private void CheckGameStatus()
    {
        // 檢查是否達到遊戲勝利條件
        if (isFishMoving && IsFishTouchingImage())
        {
            StopFishMovement();
            Debug.Log("遊戲勝利！");
            isCountdownPaused = true;
            PlayerManager.Instance.userData.fish += 1;
            SceneManager.LoadScene("FishingChoose");
        }
        else
        {
            StopFishMovement();
            Debug.Log("遊戲失敗！");
            isCountdownPaused = true;
        }
    }

    private bool IsFishTouchingImage()
    {
        Collider2D fishCollider = fishObject.GetComponent<Collider2D>();
        Collider2D imageCollider = imageObject.GetComponent<Collider2D>();

        if (fishCollider != null && imageCollider != null)
        {
            return fishCollider.IsTouching(imageCollider);
        }

        return false;
    }

    private void StopFishMovement()
    {
        isFishMoving = false;
    }

    private void ResetGame()
    {
        isGameLost = false;
        currentTime = countdownDuration;
        countdownText.text = currentTime.ToString("F0");
        isFishMoving = true;
        SetRandomImagePosition();
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
