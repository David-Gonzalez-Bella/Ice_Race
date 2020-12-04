using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameState { mainMenu, controls, credits, chooseSkin, chooseLevel, inGame, winScreen }

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton
    public gameState currentGameState = gameState.inGame;
    private GameObject currentLevel;
    private int lastLevelIndex;

    //References
    public GameObject player;
    public Camera mainCamera;
    public GameObject[] levels;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        currentGameState = gameState.mainMenu;
        ScreensManager.sharedInstance.EnableScreen("MainMenu");
        FreezePlayer();
        ScaleCamera();
    }

    public void StartGame(int levelEndScreen)
    {
        currentGameState = gameState.inGame;
        InstantiateLevel(lastLevelIndex);
        ScreensManager.sharedInstance.screens[levelEndScreen].SetActive(false); //Disable win sreen
    }

    public void FreezePlayer()
    {
        player.GetComponent<PlayerController>().anim.SetBool(player.GetComponent<PlayerController>().StopHashCode, true); //Stop player's idle animation
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnfreezePlayer()
    {
        player.GetComponent<PlayerController>().anim.SetBool(player.GetComponent<PlayerController>().StopHashCode, false); //Start player's idle animation
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void GoToMainMenu()
    {
        currentGameState = gameState.mainMenu;
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        ScreensManager.sharedInstance.EnableScreen("MainMenu");
        player.transform.position = Vector3.zero;
    }

    public void GoToControlsScreen()
    {
        currentGameState = gameState.controls;
        ScreensManager.sharedInstance.EnableScreen("ControlsScreen");
    }

    public void GoToCreditsScreen()
    {
        currentGameState = gameState.credits;
        ScreensManager.sharedInstance.EnableScreen("CreditsScreen");
    }

    public void GoToChooseSkinScreen()
    {
        currentGameState = gameState.chooseSkin;
        ScreensManager.sharedInstance.EnableScreen("ChooseSkinScreen");
    }

    public void GoToChooseLevelScreen()
    {
        currentGameState = gameState.chooseLevel;
        ScreensManager.sharedInstance.EnableScreen("ChooseLevelScreen");
    }

    public void GoToWinScreen(int playerScore, float playerTime)
    {
        FreezePlayer();
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        StartCoroutine(EndLevel(playerScore, playerTime));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InstantiateLevel(int levelIndex)
    {
        currentGameState = gameState.inGame;
        lastLevelIndex = levelIndex;
        
        UnfreezePlayer();

        currentLevel = Instantiate(levels[levelIndex], Vector3.zero, Quaternion.identity);
        UI_Manager.sharedInstance.inGameUI.SetActive(true); //Enable ingame UI

        //Disable Choose Level Screen
        ScreensManager.sharedInstance.screens[2].SetActive(false);

        //Find ingame level references
        UI_Manager.sharedInstance.currentLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level_Info>();
        CameraFollow.sharedInstance.limitStopFollow = GameObject.Find("CameraStopFollow").transform;

        //Set the player position to the level's start position
        player.transform.position = GameObject.Find("PlayerStartPosition").transform.position;
    }

    private void ScaleCamera()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        float srcWidth = Screen.width;
        float srcHeight = Screen.height;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;
        mainCamera.aspect = DEVICE_SCREEN_ASPECT;
    }

    IEnumerator EndLevel(int playerScore, float playerTime)
    {
        yield return new WaitForSeconds(1.0f);
        currentGameState = gameState.winScreen;
        UI_Manager.sharedInstance.countDownActive = false;
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        ScreensManager.sharedInstance.levelScore.text = playerScore.ToString();
        ScreensManager.sharedInstance.levelTime.text = playerTime.ToString();
        ScreensManager.sharedInstance.EnableScreen("WinScreen");
        Destroy(currentLevel);
        CameraFollow.sharedInstance.transform.position = Vector3.zero; //Whenever we go back to the main menu we reset the camera position
    }
}
