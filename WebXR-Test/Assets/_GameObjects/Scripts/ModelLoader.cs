using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Siccity.GLTFUtility;
using UnityEngine.UI;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] GameObject modelParent;
    [SerializeField] Text textLeft;
    [SerializeField] Text textRight;

    //meter dinámicamente por playerprefs o similar
    string urlModel = "https://webxr-vf.s3.eu-west-3.amazonaws.com/UNITY/Web+Resources/cell_phone.glb";
    string urlTextLeft = "https://webxr-vf.s3.eu-west-3.amazonaws.com/UNITY/Web+Resources/textleft.txt";
    string urlTextRight = "https://webxr-vf.s3.eu-west-3.amazonaws.com/UNITY/Web+Resources/textright.txt";

    GameObject wrapper;
    string filePath;

    private void Start()
    {
        filePath = $"{Application.persistentDataPath}/Files/";
        wrapper = new GameObject
        {
            name = "Model"
        };
        wrapper.transform.SetParent(modelParent.transform);

        DownloadFile(urlModel);
        StartCoroutine(GetText(textLeft, urlTextLeft));
        StartCoroutine(GetText(textRight, urlTextRight));
    }

    public void DownloadFile(string url)
    {
        string path = GetFilePath(url);
        if (File.Exists(path))
        {
            Debug.Log("Found file locally, loading...");
            LoadModel(path);
            return;
        }

        StartCoroutine(GetFileRequest(url, (UnityWebRequest req) =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                // Log any errors that may happen
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            }
            else
            {
                // Save the model into a new wrapper
                LoadModel(path);
            }
        }));
    }

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
    }

    void LoadModel(string path)
    {
        ResetWrapper();
        GameObject model = Importer.LoadFromFile(path);
        model.transform.SetParent(wrapper.transform);
        //TEST (ELIMINAR)
        model.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    IEnumerator GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));
            yield return req.SendWebRequest();
            callback(req);
        }
    }

    void ResetWrapper()
    {
        if (wrapper != null)
        {
            foreach (Transform trans in wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }

    IEnumerator GetText(Text text, string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            text.text = www.downloadHandler.text;
            Debug.Log(www.downloadHandler.text);
            // Or retrieve results as binary data
            //byte[] results = www.downloadHandler.data;
        }
    }

}
