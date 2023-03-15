using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class TextLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://github.com/KhronosGroup/glTF-Sample-Models/blob/master/sourceModels/AttenuationTest/Notes.txt");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    public void GetPosts()
    {
        StartCoroutine(GetRequest("https://jsonplaceholder.typicode.com/posts", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Post[] posts = JsonConvert.DeserializeObject<Post[]>(req.downloadHandler.text);

                foreach (Post post in posts)
                {
                    Debug.Log(post.title);
                }
            }
        }));
    }
    /*
    public void GetText()
    {
        StartCoroutine(GetRequest("https://github.com/KhronosGroup/glTF-Sample-Models/blob/master/sourceModels/AttenuationTest/Notes.txt", (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Debug.Log($"{req.error}: {req.downloadHandler.text}");
            }
            else
            {
                Debug.Log(req.downloadHandler.text);
            }
        }));
    }
    */
    IEnumerator GetRequest(string endpoint, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(endpoint))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            callback(request);
        }
    }

}

// Data Classes
public class Post
{
    public int userId;
    public int id;
    public string title;
    public string body;
}
