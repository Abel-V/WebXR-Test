using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadDesertScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadHitTestScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadARWTScene()
    {
        SceneManager.LoadScene(3);
    }
}
