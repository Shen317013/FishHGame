using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class LevelDataLoad : MonoBehaviour
{
    public Text Time;
    public Text playerFishText;
    private int playerFish = 0;
    public float countdownTime = 0f; // 倒數計時
    private bool isGameOver = false; // 遊戲失敗判定

    private void Start()
    {
        string jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Json/LevelData.json");
        string json = File.ReadAllText(jsonFilePath);

        LevelData levelData = JsonUtility.FromJson<LevelData>(json);


        foreach (Level level in levelData.levels)
        {
            //  判斷進去關卡是json的哪一關
            if (level.level == LevelManager.instance.golevel)
            {
                // 設置關卡的倒數計時
                countdownTime = level.time;
                Time.text = countdownTime.ToString();

                // 獲取關卡需要捕的魚數量
                int requiredFish = level.fish;

                // 更新 playerFish 的值
                playerFish = 0;

                // 更新 playerfish 文本组件的值
                playerFishText.text = playerFish.ToString();

                StartCoroutine(Countdown()); //開始倒數時

                // 檢查獲勝條件
                if (playerFish >= requiredFish)
                {
                    // 獲勝函數
                    Victory();
                }

                break;
            }
        }
    }

    private IEnumerator Countdown()
    {
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1f);
            countdownTime--;
            Time.text = countdownTime.ToString();

            // 檢查失敗條件
            if (countdownTime == 0 && !isGameOver)
            {
                Failure();
            }
        }
    }

    private void Victory()
    {
        isGameOver = true;
        Debug.Log("遊戲勝利");
    }

    private void Failure()
    {
        isGameOver = true;
        Debug.Log("遊戲失敗");
    }
}

[System.Serializable]
public class LevelData
{
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public int level;
    public int time;
    public int fish;
}
