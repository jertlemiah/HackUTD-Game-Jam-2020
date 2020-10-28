using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public InputMaster controls;
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;

    private void OnEnable()
    {
        controls.Enable();
        IsPaused = false;
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
        controls.Player.Pause.performed += context => ChangePauseState();
    }

    private void ChangePauseState()
    {
        Debug.Log("ChangeGameState called");
        if (IsPaused == true)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Loading main menu");
        SceneManager.LoadScene("TitleMenu"); // this probably shouldn't be hardcoded
    }
}
