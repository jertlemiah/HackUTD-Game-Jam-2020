using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public InputMaster controls;
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;

    private void Awake()
    {
        Debug.Log("PauseMenu Awake() called");
        controls = new InputMaster();
        //controls.Player.Movement.performed += context => Move(context.ReadValue<Vector2>());
        controls.Player.Pause.performed += context => ChangeGameState();
    }

    private void ChangeGameState()
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

    private void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }
}
