using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsPage;
    public GameObject controlsPage;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void LeaveCredits()
    {
        creditsPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        creditsPage.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void LeaveControls()
    {
        controlsPage.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenControls()
    {
        controlsPage.SetActive(true);
        mainMenu.SetActive(false);
    }
}
