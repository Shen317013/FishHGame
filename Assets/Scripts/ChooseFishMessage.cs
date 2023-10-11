using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseFishMessage : MonoBehaviour
{
    public GameObject buttonPrefab;     //小魚訊
    public GameObject specialButtonPrefab;  //大魚訊
    public Transform buttonParent;  //魚訊按鈕的父級對象
    public RectTransform buttonArea;    //魚訊可以出現的區域範圍
    public string[] sceneNames;  //小魚訊的跳轉場景
    public string specialButtonScene;   //大魚訊的跳轉場景

    public int maxButtonCount = 3;  //最多同時出現3個小魚訊
    public float minDelay = 3f;     //小魚訊出現的最少時間
    public float maxDelay = 8f;  //小魚訊出現的最多時間
    public float ButtonDurationMin = 3f;     //小魚訊出現的最少持續時間
    public float ButtonDurationMax = 10f;    //小魚訊出現的最多持續時間

    public float specialButtonIntervalMin = 30f;    //大魚訊出現的最少時間
    public float specialButtonIntervalMax = 60f;    //大魚訊出現的最多時間
    public float specialButtonDurationMin = 5f;     //大魚訊出現的最少持續時間
    public float specialButtonDurationMax = 20f;    //大魚訊出現的最多持續時間

    private bool isSpecialButtonActive = false; //大魚訊出現偵測

    public Button BackButton;

    private void Start()
    {
        StartCoroutine(SpawnButtonsCoroutine());
        StartCoroutine(SpawnSpecialButtonCoroutine());

        BackButton.onClick.AddListener(LoadSceneGameMenu);
    }

    private IEnumerator SpawnButtonsCoroutine()
    {
        while (true)
        {
            if (buttonParent.childCount < maxButtonCount)
            {
                Vector2 randomPosition = GetRandomPosition();
                bool overlap = CheckButtonOverlap(randomPosition);
                if (!overlap)
                {
                    GameObject button = Instantiate(buttonPrefab, buttonParent);
                    button.transform.localPosition = randomPosition;
                    button.SetActive(true);
                    Button buttonComponent = button.GetComponent<Button>();
                    buttonComponent.onClick.AddListener(() => JumpToScene(GetRandomScene()));
                    float activeDuration = Random.Range(ButtonDurationMin, ButtonDurationMax);
                    Invoke("DeactivateButton", activeDuration);
                }
            }
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    private IEnumerator SpawnSpecialButtonCoroutine()
    {
        while (true)
        {
            if (!isSpecialButtonActive && buttonParent.childCount < maxButtonCount)
            {
                Vector2 specialButtonPosition = GetRandomPosition();
                bool overlap = CheckButtonOverlap(specialButtonPosition);
                if (!overlap)
                {
                    GameObject specialButton = Instantiate(specialButtonPrefab, buttonParent);
                    specialButton.transform.localPosition = specialButtonPosition;
                    specialButton.SetActive(true);
                    Button specialButtonComponent = specialButton.GetComponent<Button>();
                    specialButtonComponent.onClick.AddListener(() => JumpToScene(specialButtonScene));

                    float specialButtonDuration = Random.Range(specialButtonDurationMin, specialButtonDurationMax);
                    Invoke("DeactivateSpecialButton", specialButtonDuration);
                    isSpecialButtonActive = true;
                }
            }
            yield return new WaitForSeconds(Random.Range(specialButtonIntervalMin, specialButtonIntervalMax));
        }
    }

    private void DeactivateButton()
    {
        if (buttonParent.childCount > 0)
        {
            GameObject button = buttonParent.GetChild(0).gameObject;
            Destroy(button);
        }
    }

    private void DeactivateSpecialButton()
    {
        if (buttonParent.childCount > 0)
        {
            GameObject specialButton = buttonParent.GetChild(0).gameObject;
            Destroy(specialButton);
        }
        isSpecialButtonActive = false;
    }

    private Vector2 GetRandomPosition()
    {
        float minX = buttonArea.rect.xMin;
        float maxX = buttonArea.rect.xMax;
        float minY = buttonArea.rect.yMin;
        float maxY = buttonArea.rect.yMax;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Vector2 randomPosition = new Vector2(x, y);

        return randomPosition;
    }

    private bool CheckButtonOverlap(Vector2 position)   //不讓按鈕重疊
    {
        foreach (Transform child in buttonParent)
        {
            if (Vector2.Distance(child.localPosition, position) < 2f) //判斷圖像直徑大小
            {
                return true;
            }
        }
        return false;
    }

    private string GetRandomScene()
    {
        return sceneNames[Random.Range(0, sceneNames.Length)];
    }

    private void JumpToScene(string sceneName)
    {
        float countdownTime = FindObjectOfType<LevelDataLoad>().countdownTime;
        PlayerManager.Instance.userData.fishtime = countdownTime;
//        PlayerManager.Instance.SaveUserData();
        SceneManager.LoadScene(sceneName);
    }

    private void LoadSceneGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }
}
