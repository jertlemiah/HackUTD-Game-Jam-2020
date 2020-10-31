using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : Singleton<StateManager>
{
    public InputMaster controls;
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    public GameObject WinUI;
    public GameObject PlayerGUI;
    public static List<GameObject> availableBones = new List<GameObject>();
    [SerializeField]
    public GameObject playerObject;



    public static GameObject CreateBone(GameObject BonePrefab, Vector2 location)
    {
        GameObject bone = Instantiate(BonePrefab, location, Quaternion.identity) as GameObject;
        if(bone.tag == "Bone")
        {
            availableBones.Add(bone);
        }        
        return bone;
    }

    public static void RemoveBone(GameObject bone)
    {
        if (availableBones.Contains(bone))
        {
            Debug.Log("Removing bone at " + bone.transform.position + " from available bones");
            availableBones.Remove(bone);
        }
        else
        {
            Debug.Log("RemoveBone called, but given object is not a valid GameObject");
        }
    }

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

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.);
    }

    public void OpenGameOverUI()
    {
        GameOverUI.SetActive(true);
        IsPaused = true;
    }

    public void OpenWinUI()
    {
        WinUI.SetActive(true);
        IsPaused = true;
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
        PlayerGUI.SetActive(true);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void PauseGame()
    {
        PauseMenuUI.SetActive(true);
        PlayerGUI.SetActive(false);
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
