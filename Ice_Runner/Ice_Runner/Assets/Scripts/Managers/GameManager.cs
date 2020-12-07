using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce;

public enum gameState { mainMenu, controls, credits, chooseSkin, chooseLevel, inGame, settings, winScreen, deadScreen }

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton
    private gameState backgroundGameState;
    public gameState currentGameState = gameState.inGame;
    [HideInInspector] public GameObject currentLevel;
    private int lastLevelIndex;
    private bool colorblindMode = false;

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
        //Set the aplication framerate 
        Application.targetFrameRate = 60;

        //Go to main menu
        currentGameState = gameState.mainMenu;
        ScreensManager.sharedInstance.EnableScreen("MainMenu");
        FreezePlayer();
        ScaleCamera();
    }

    public void StartGame(int levelEndScreen)
    {
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

    public void FreezeEnemies()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().anim.SetBool(enemy.GetComponent<Enemy>().StopHashCode, true); //Stop player's idle animation
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void UnfreezeEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().anim.SetBool(enemy.GetComponent<Enemy>().StopHashCode, false); //Start player's idle animation
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void GoToMainMenu()
    {
        if (GameObject.FindGameObjectWithTag("Level") != null)
            DestroyCurrentLevel();
        currentGameState = gameState.mainMenu;
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        ScreensManager.sharedInstance.darkBackground.SetActive(false);
        ScreensManager.sharedInstance.EnableScreen("MainMenu");
        player.transform.position = Vector3.zero;
    }

    public void GoToWannaLeaveMMScreen(bool screenEnabled)
    {
        ScreensManager.sharedInstance.ShowWannaLeaveMM(screenEnabled);
        ScreensManager.sharedInstance.darkBackground.SetActive(screenEnabled);
    }

    public void GoToWannaLeaveIGScreen(bool screenEnabled)
    {
        ScreensManager.sharedInstance.ShowWannaLeaveIG(screenEnabled);
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
        UI_Manager.sharedInstance.countDownActive = false;
        ScreensManager.sharedInstance.levelScore.text = playerScore.ToString();
        ScreensManager.sharedInstance.levelTime.text = playerTime.ToString();
        ScreensManager.sharedInstance.EnableScreen("WinScreen");
        currentGameState = gameState.winScreen;
    }

    public void GoToDeadScreen()
    {
        FreezePlayer();
        UI_Manager.sharedInstance.countDownActive = false;
        ScreensManager.sharedInstance.EnableScreen("DeadScreen");
        currentGameState = gameState.deadScreen;
    }

    public void GoToSettingsScreen(bool settingsEnabled)
    {
        if (settingsEnabled)
        {
            backgroundGameState = currentGameState;
            currentGameState = gameState.settings;
        }
        else
        {
            currentGameState = backgroundGameState;
        }
        if (backgroundGameState == gameState.inGame)  //If the game was paused then the player is frozen, and so we have to unfreeze it.
        {
            UnfreezePlayer();
            UnfreezeEnemies();
        }
        ScreensManager.sharedInstance.darkBackground.SetActive(settingsEnabled);
        ScreensManager.sharedInstance.ShowSettings(settingsEnabled);
    }

    public void DestroyCurrentLevel()
    {
        Destroy(currentLevel);
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InstantiateLevel(int levelIndex)
    {
        //Reset all values
        ResetUI_Values();

        //Save this level as the last level
        lastLevelIndex = levelIndex;

        //Instantiate level
        currentLevel = Instantiate(levels[levelIndex], Vector3.zero, Quaternion.identity);
        UI_Manager.sharedInstance.inGameUI.SetActive(true); //Enable ingame UI

        //Disable Choose Level Screen
        ScreensManager.sharedInstance.screens[2].SetActive(false);

        //Find ingame level references
        UI_Manager.sharedInstance.currentLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level_Info>();
        CameraFollow.sharedInstance.limitStopFollow = GameObject.Find("CameraStopFollow").transform;

        //Set the player position to the level's start position
        player.transform.position = GameObject.Find("PlayerStartPosition").transform.position;
        CameraFollow.sharedInstance.transform.position = player.transform.position;

        //We are now playing
        currentGameState = gameState.inGame;
        UnfreezePlayer();
    }

    private void ScaleCamera()
    {
        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        float srcWidth = Screen.width;
        float srcHeight = Screen.height;

        float DEVICE_SCREEN_ASPECT = srcWidth / srcHeight;
        mainCamera.aspect = DEVICE_SCREEN_ASPECT;
    }
    
    public void ColorBlindMode()
    {
        colorblindMode = !colorblindMode;
        int mode = colorblindMode ? 1 : 0;
        mainCamera.GetComponent<Colorblind>().Type = mode;
    }


    private void ResetUI_Values()
    {
        player.GetComponent<PlayerController>().score = 0;
        player.GetComponent<Health>().CurrentLifes = 3;
        UI_Manager.sharedInstance.countDownTime = 150.0f;
        UI_Manager.sharedInstance.countDownActive = true;
    }
}
