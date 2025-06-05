using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

     public static bool ShopIsOpen = false;

    public TimeManager timer;

    public GameObject PauseMenuUI;
    public GameObject GameoverUI;
    public GameObject ShopMenuUI;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (timer.CurrentYear >= 2050)
        {
            gameover();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (ShopIsOpen)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
    }

    public void gameover()
    {
        GameoverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void Menuknop()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }
    void OpenShop()
    {
        ShopMenuUI.SetActive(true);
        ShopIsOpen = true;
    }

    public void CloseShop()
    {
        ShopMenuUI.SetActive(false);
        ShopIsOpen = false;
    }
}
