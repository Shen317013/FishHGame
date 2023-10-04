using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text;

public class EndMessageManager : MonoBehaviour
{
    public Text nameText;
    public Text contentText;
    public RawImage roleImage;
    public Button nextButton;
    public Button continueButton;
    public Button skipButton;
    public float delay = 0.1f;

    private List<MessageItem> messageList;
    private int currentIndex = 0;
    private Coroutine displayCoroutine;

    void Start()
    {
        string jsonFilePath = "";

        if (LevelManager.instance.golevel == 1)
        {
            jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Json/TestEndMessage.json");
        }
        else if (LevelManager.instance.golevel == 2)
        {
            jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Json/FristEndMessage.json");
        }

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            MessageData messageData = JsonUtility.FromJson<MessageData>(jsonString);

            if (messageData != null && messageData.Message.Length > 0)
            {
                messageList = new List<MessageItem>(messageData.Message);
                LoadMessageItem(currentIndex);
            }
        }
        else
        {
            Debug.LogError("Json file not found: " + jsonFilePath);
        }

        nextButton.onClick.AddListener(NextButtonClicked);
        skipButton.onClick.AddListener(SkipButtonClicked);
        continueButton.onClick.AddListener(NextButtonClicked);
    }


    IEnumerator LoadRoleImage(string imagePath)
    {
        string imageFilePath = Path.Combine(Application.streamingAssetsPath, imagePath);

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageFilePath))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D roleTexture = DownloadHandlerTexture.GetContent(www);
                roleImage.texture = roleTexture;
            }
            else
            {
                Debug.LogError("Role image not found: " + imageFilePath);
            }
        }
    }

    IEnumerator DisplayContentText(string content)
    {
        contentText.text = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < content.Length; i++)
        {
            stringBuilder.Append(content[i]);
            contentText.text = stringBuilder.ToString();
            yield return new WaitForSeconds(delay);
        }
    }

    void LoadMessageItem(int index)
    {
        MessageItem messageItem = messageList[index];
        string name = messageItem.name;
        string roleImagePath = messageItem.role;

        nameText.text = name;

        StartCoroutine(LoadRoleImage(roleImagePath));

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        displayCoroutine = StartCoroutine(DisplayContentText(messageItem.content));
    }

    void NextButtonClicked()
    {
        currentIndex++;

        if (currentIndex < messageList.Count)
        {
            LoadMessageItem(currentIndex);
        }
        else
        {
            Debug.Log("Reached end of JSON data.");
            LevelManager.instance.golevel = 0;
            SceneManager.LoadScene("GameMenu");
        }
    }

    void SkipButtonClicked()
    {
        LevelManager.instance.golevel = 0;
        SceneManager.LoadScene("GameMenu");
    }

    [System.Serializable]
    public class MessageData
    {
        public MessageItem[] Message;
    }

    [System.Serializable]
    public class MessageItem
    {
        public string name;
        public string role;
        public string content;
    }
}
