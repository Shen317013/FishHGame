using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class StoryGameManager : MonoBehaviour
{
    public void LoadSceneChoose()
    {
        SceneManager.LoadScene("ChooseRole");
    }

    public void LoadSceneMessage()
    {
        SceneManager.LoadScene("RoleMessage");
    }

    public Text storyText;
    public Text targetText;
    public RawImage roleImage;

    void Start()
    {
        string jsonFilePath = "";

        if (LevelManager.instance.golevel == 1)
        {
            jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Json/TestStory.json");
        }
        else if (LevelManager.instance.golevel == 2)
        {
            jsonFilePath = Path.Combine(Application.streamingAssetsPath, "Json/FristStory.json");
        }

        if (File.Exists(jsonFilePath))
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            StoryData storyData = JsonUtility.FromJson<StoryData>(jsonString);

            if (storyData != null && storyData.Story.Length > 0)
            {
                StoryItem storyItem = storyData.Story[0];
                string story = storyItem.story;
                string target = storyItem.target;
                string roleImagePath = storyItem.role;

                storyText.text = story;
                targetText.text = target;

                StartCoroutine(LoadRoleImage(roleImagePath));
            }
        }
        else
        {
            Debug.LogError("Json file not found: " + jsonFilePath);
        }
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

    [System.Serializable]
    public class StoryData
    {
        public StoryItem[] Story;
    }

    [System.Serializable]
    public class StoryItem
    {
        public string story;
        public string target;
        public string role;
    }
}
