using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneLoad : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "Ian's Scene";

    public void Newgame()
    {
        SceneManager.LoadScene(newGameLevel);
    }
}

