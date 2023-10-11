using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FishGameThree : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float minRotationZ = -4f;    //外框的旋轉最低角度
    public float maxRotationZ = 4f;     //外框的旋轉最高角度
    public GameObject parentObject;     //外框物件
    public Transform childObject;       //球體物件
    public Transform judgeObject;       //判定物件
    public Transform[] failureObjects;  //失敗物件陣列
    public Text countdownText;          //倒數計時

    private bool gameStarted = true;    //判斷開始遊戲
    private float countdownDuration = 10f;    //倒數計時秒數
    private float currentCountdown;     //當前倒數計時
    private bool mouseClickEnabled = true;  //是否允許滑鼠點擊
    private float overlapTimer = 0f;    //球體與失敗物件重疊計時

    public GameObject leftObject;   //滑鼠點擊顯示(左)
    public GameObject rightObject;  //滑鼠點擊顯示(右)

    public Button pauseButton;      //暫停按鈕
    public Button resetButton;      //重置按鈕
    private bool isPaused = false;

    private Vector3 initialChildLocalPosition;
    private Quaternion initialChildLocalRotation;

    private bool isRotatingCoroutineRunning = false;

    private float currentRandomRotateWaitTime = 0f;
    private Coroutine rotatingCoroutine;

    private void Start()
    {
        if (parentObject == null || failureObjects == null || failureObjects.Length == 0)
        {
            Debug.LogError("父物件或失敗物件為空");
            return;
        }

        // 隨機旋轉一次外框
        RandomRotateParent();

        //儲存球體物件座標與旋轉角度
        initialChildLocalPosition = childObject.localPosition;
        initialChildLocalRotation = childObject.localRotation;

        //監聽暫停按鈕的點擊事件
        pauseButton.onClick.AddListener(TogglePause);

        //監聽重置按鈕的點擊事件
        resetButton.onClick.AddListener(ResetGame);

        //初始化倒數計時
        countdownText.text = "10";
        currentCountdown = countdownDuration;

        //啟動外框隨機旋轉並保存協程引用
        rotatingCoroutine = StartCoroutine(RandomRotateCoroutine());
    }

    private void Update()
    {
        if (!isPaused)
        {
            //更新倒數計時
            if (gameStarted)
            {
                currentCountdown -= Time.deltaTime;
                countdownText.text = Mathf.Max(Mathf.RoundToInt(currentCountdown), 0).ToString();

                //倒數計時結束時 停用外框隨機旋轉與滑鼠點擊旋轉
                if (currentCountdown <= 0f)
                {
                    gameStarted = false;
                    StopCoroutine(RandomRotateCoroutine());
                    mouseClickEnabled = false;

                    //讓球體停止移動並保留原地
                    Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();
                    childRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                    //判斷球體是否完全在判定區域內
                    if (IsChildInJudge())
                    {
                        //遊戲勝利邏輯
                        Debug.Log("遊戲勝利！");
                        PlayerManager.Instance.userData.fish += 1;
                        SceneManager.LoadScene("FishingChoose");
                    }
                    else
                    {
                        //遊戲失敗邏輯
                        Debug.Log("遊戲失敗！");
                    }
                }

                //判斷球體是否重疊失敗物件
                if (IsChildOverlappingFailureObject())
                {
                    //增加重疊時間
                    overlapTimer += Time.deltaTime;

                    //如果重疊超過1秒 遊戲失敗
                    if (overlapTimer >= 1f)
                    {
                        gameStarted = false;
                        StopCoroutine(RandomRotateCoroutine());
                        mouseClickEnabled = false;

                        //讓球體停止移動並保留原地
                        Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();
                        childRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                        //遊戲失敗邏輯
                        Debug.Log("遊戲失敗，球體與失敗物件重疊超過1秒");
                    }
                }
                else
                {
                    //如果未重疊 重置重疊時間
                    overlapTimer = 0f;
                }

                if (parentObject.transform.rotation.eulerAngles.z == 0f)
                {
                    StopChildRotation();
                }
            }

            //如果允許滑鼠點擊與遊戲開始 開啟滑鼠點擊旋轉與顯示點擊圖片
            if (mouseClickEnabled && gameStarted)
            {
                if (Input.GetMouseButtonDown(0)) //左鍵點擊
                {
                    RotateZAxis(1);
                    ShowObject(leftObject);
                }
                else if (Input.GetMouseButtonDown(1)) //右鍵點擊
                {
                    RotateZAxis(-1);
                    ShowObject(rightObject);
                }
            }
        }
    }

    private void RotateZAxis(int direction)
    {
        //獲取外框的當前旋轉角度
        Vector3 currentRotation = parentObject.transform.rotation.eulerAngles;

        //歸化當前角度到-180度到180度之間
        float normalizedRotationZ = Mathf.Repeat(currentRotation.z + 180f, 360f) - 180f;

        //根據旋轉方向與旋轉角度計算新的z軸角度
        float newRotationZ = normalizedRotationZ + (direction * rotationSpeed);

        //將新的z軸角度限制在限定範圍內
        newRotationZ = Mathf.Clamp(newRotationZ, minRotationZ, maxRotationZ);

        //應用新的旋轉角度到外框上
        parentObject.transform.rotation = Quaternion.Euler(0f, 0f, newRotationZ);
    }

    private System.Collections.IEnumerator RandomRotateCoroutine()
    {
        //確保外框隨機旋轉只有一個協成在執行
        if (isRotatingCoroutineRunning)
        {
            yield break;
        }

        isRotatingCoroutineRunning = true;

        //初始化隨機旋轉頻率
        currentRandomRotateWaitTime = Random.Range(2f, 5f);

        //在協程中使用等待時間變量
        yield return new WaitForSeconds(currentRandomRotateWaitTime);

        while (true)
        {
            //當允許滑鼠點擊才執行外框旋轉
            if (!isPaused && mouseClickEnabled)
            {
                //隨機選擇旋轉角度
                int[] randomRotationOptions = { -4, -3, -2, 2, 3, 4 };  //隨機的角度清單
                int randomIndex = Random.Range(0, randomRotationOptions.Length);
                int randomRotationZ = randomRotationOptions[randomIndex];

                //應用旋轉角度到外框
                parentObject.transform.rotation = Quaternion.Euler(0f, 0f, randomRotationZ);

                //生成新的隨機旋轉頻率
                currentRandomRotateWaitTime = Random.Range(2f, 5f);

                //在協程中使用新的等待時間變量
                yield return new WaitForSeconds(currentRandomRotateWaitTime);
            }
            else
            {
                //不允許滑鼠點擊時，暫停隨機協程
                yield return null;
            }
        }
    }

    private bool IsChildInJudge()
    {
        //獲取球體Collider2D組件
        Collider2D childCollider = childObject.GetComponent<Collider2D>();

        //獲取球體四個邊角
        Vector2 bottomLeft = childCollider.bounds.min;
        Vector2 topRight = childCollider.bounds.max;

        //判斷球體物件使否完全在判定物件內
        Collider2D[] overlappedColliders = Physics2D.OverlapAreaAll(bottomLeft, topRight);
        foreach (Collider2D collider in overlappedColliders)
        {
            if (collider == judgeObject.GetComponent<Collider2D>())
            {
                return true;
            }
        }

        return false;
    }

    private bool IsChildOverlappingFailureObject()
    {
        //獲取球體Collider2D組件
        Collider2D childCollider = childObject.GetComponent<Collider2D>();

        //判斷球體是否重疊失敗物件
        foreach (Transform failure in failureObjects)
        {
            Collider2D failureCollider = failure.GetComponent<Collider2D>();
            if (childCollider.IsTouching(failureCollider))
            {
                return true;
            }
        }

        return false;
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

    private void ResetGame()
    {
        //停止隨機外框旋轉協程
        if (rotatingCoroutine != null)
        {
            StopCoroutine(rotatingCoroutine);
        }

        //重置遊戲狀態
        isPaused = false;
        gameStarted = true;
        currentCountdown = countdownDuration;
        countdownText.text = "10";
        isRotatingCoroutineRunning = false;

        //讓球體停止移動並保留原地
        Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();
        childRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //隱藏左右點擊圖片
        leftObject.SetActive(false);
        rightObject.SetActive(false);

        //重置外框旋轉角度
        parentObject.transform.rotation = Quaternion.identity;

        //使用一開始儲存的球體位置與旋轉角度 並應用在球體上
        childObject.localPosition = initialChildLocalPosition;
        childObject.localRotation = initialChildLocalRotation;

        //重新啟用球體的重力
        childRigidbody.constraints = RigidbodyConstraints2D.None;
        childRigidbody.gravityScale = 300f;

        // 隨機旋轉一次外框
        RandomRotateParent();

        //重置隨機外框旋轉時間
        currentRandomRotateWaitTime = Random.Range(2f, 5f);

        //啟動隨機外框旋轉協程
        rotatingCoroutine = StartCoroutine(RandomRotateCoroutine());
    }

    //暫停或恢復遊戲的方法
    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            StopCoroutine(RandomRotateCoroutine());
            //讓球體停止移動並保留原地
            Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();
            childRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            StartCoroutine(RandomRotateCoroutine());
            //重新啟用球體的重力
            Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();
            childRigidbody.constraints = RigidbodyConstraints2D.None;
            childRigidbody.gravityScale = 300f;
        }
    }

    private void StopChildRotation()
    {
        // 获取 childObject 上的 Rigidbody2D 组件
        Rigidbody2D childRigidbody = childObject.GetComponent<Rigidbody2D>();

        // 停用 childObject 的旋转
        childRigidbody.angularVelocity = 0f;
    }

    private void RandomRotateParent()
    {
        // 隨機選擇旋轉角度
        int[] randomRotationOptions = { -4, -3, -2, 2, 3, 4 };  // 隨機的角度清單
        int randomIndex = Random.Range(0, randomRotationOptions.Length);
        int randomRotationZ = randomRotationOptions[randomIndex];

        // 應用旋轉角度到外框
        parentObject.transform.rotation = Quaternion.Euler(0f, 0f, randomRotationZ);
    }


}
