using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public enum PaltformTypes
{
  TWITCH,
  YOUTUBE
}

public class NameManager : MonoBehaviour
{
  public TMP_Dropdown platformSelector;
  public static NameManager instance;
  public List<string> allNames;
  public List<string> uniqueNames;

  private void Awake()
  {
    if (instance == null) instance = this;
    if (instance != this)
    {
      DestroyImmediate(gameObject);
      return;
    }

    DontDestroyOnLoad(this);
  }

  public void FetchNames(TextMeshProUGUI channelNameField)
  {
    string channelName = channelNameField.text.Trim((char)8203).ToLower();
    var selectedIndex = platformSelector.value;

    switch (selectedIndex)
    {
      case 0:
        StartCoroutine(FetchFromTwitch(channelName));
        break;
      case 1:
        StartCoroutine(FetchFromTwitch(channelName));
        break;
    }
  }

  private IEnumerator FetchFromTwitch(string channelName)
  {
    using (UnityWebRequest www = UnityWebRequest.Get($"https://tmi.twitch.tv/group/user/{channelName}/chatters"))
    {
      yield return www.SendWebRequest();

      if (www.isNetworkError || www.isHttpError)
      {
        Debug.Log(www.error);
      }
      else
      {
        TwitchData twitchChatters = new TwitchData();
        JsonUtility.FromJsonOverwrite(www.downloadHandler.text, twitchChatters);
        allNames = twitchChatters.GetAsList();
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
      }
    }
  }

  public string GetRandomUniqueName()
  {
    if (uniqueNames.Count == 0) uniqueNames = new List<string>(allNames);
    int index = UnityEngine.Random.Range(0, uniqueNames.Count);
    string randomName = uniqueNames[index];
    uniqueNames.RemoveAt(index);

    return randomName;
  }
}


[Serializable]
public class TwitchData
{
  public Chatters chatters;

  public List<string> GetAsList()
  {
    List<string> chattersNames = new List<string>();

    foreach (string name in chatters.vips)
    {
      chattersNames.Add(name);
    }

    foreach (string name in chatters.moderators)
    {
      chattersNames.Add(name);
    }

    foreach (string name in chatters.viewers)
    {
      chattersNames.Add(name);
    }

    return chattersNames;
  }
}

[Serializable]
public class Chatters
{
  public List<string> vips;
  public List<string> moderators;
  public List<string> viewers;
}