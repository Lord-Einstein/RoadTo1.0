using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeToBS : MonoBehaviour
{
    public string destinationScene;

    void OnTriggerEnter2D()
    {
        SceneManager.LoadScene(destinationScene);
    }
}
