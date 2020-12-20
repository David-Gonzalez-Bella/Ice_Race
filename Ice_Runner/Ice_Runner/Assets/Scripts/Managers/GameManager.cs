using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameState { mainMenu, controls, credits, chooseSkin, chooseLevel, inGame, settings, winScreen, deadScreen }

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton
    public gameState backgroundGameState;
    public gameState currentGameState = gameState.inGame;
    [HideInInspector] public GameObject currentLevel;
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
        //Set the aplication framerate 
        Application.targetFrameRate = 60;

        //Go to main menu
        currentGameState = gameState.mainMenu;
        ScreensManager.sharedInstance.EnableScreen("MainMenu");
        FreezePlayer();
        ScaleCamera();
    }

    public void StartGame()
    {
        InstantiateLevel(lastLevelIndex);
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
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
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
        AudioManager.sharedInstance.PlayMainTheme(0);
        currentGameState = gameState.mainMenu;
        ScreensManager.sharedInstance.StartTransitionAnim("MainMenu");
        player.GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<PlayerController>().overrideAnimation[0] as RuntimeAnimatorController;
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
        ScreensManager.sharedInstance.StartTransitionAnim("ControlsScreen");
    }

    public void GoToCreditsScreen()
    {
        currentGameState = gameState.credits;
        ScreensManager.sharedInstance.StartTransitionAnim("CreditsScreen");
    }

    public void GoToChooseSkinScreen()
    {
        currentGameState = gameState.chooseSkin;
        ScreensManager.sharedInstance.SetPreviewSkin(0);
        ScreensManager.sharedInstance.StartTransitionAnim("ChooseSkinScreen");
    }

    public void GoToChooseLevelScreen()
    {
        currentGameState = gameState.chooseLevel;
        ScreensManager.sharedInstance.StartTransitionAnim("ChooseLevelScreen");
    }

    public void GoToWinScreen(int playerScore, float playerTime)
    {
        FreezePlayer();
        FreezeEnemies();
        PlayWinSound();
        UI_Manager.sharedInstance.countDownActive = false;
        ScreensManager.sharedInstance.levelScore.text = playerScore.ToString();
        ScreensManager.sharedInstance.levelTime.text = playerTime.ToString();
        currentGameState = gameState.winScreen;
        ScreensManager.sharedInstance.StartTransitionAnim("WinScreen");
        StartCoroutine(WaitToDestroyLevel());
    }

    public void GoToDeadScreen()
    {
        FreezePlayer();
        FreezeEnemies();
        PlayGameOverSound();
        UI_Manager.sharedInstance.countDownActive = false;
        ScreensManager.sharedInstance.StartTransitionAnim("DeadScreen");
        currentGameState = gameState.deadScreen;
        StartCoroutine(WaitToDestroyLevel());
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
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void InstantiateLevel(int levelIndex)
    {
        //Transition animation
        ScreensManager.sharedInstance.StartTransitionAnim("InGame");

        //Reset all values
        ResetUI_Values();

        //Save this level as the last level
        lastLevelIndex = levelIndex;

        //Instantiate level
        currentLevel = Instantiate(levels[levelIndex], Vector3.zero, Quaternion.identity);
        UI_Manager.sharedInstance.pauseButton.interactable = true; //Enable pause button UI

        //Find ingame level references
        UI_Manager.sharedInstance.currentLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level_Info>();
        CameraFollow.sharedInstance.limitStopFollow = GameObject.FindGameObjectWithTag("CameraStopFollow").transform;

        //Set the player position to the level's start position
        player.transform.position = GameObject.Find("PlayerStartPosition").transform.position;
        CameraFollow.sharedInstance.transform.position = player.transform.position;

        //Set the player's initial variables so that a level can commence
        player.GetComponent<PlayerController>().isDead = false;
        player.GetComponent<Animator>().SetBool(player.GetComponent<PlayerController>().StopHashCode, true);
        player.GetComponent<PlayerController>().currentLevel = currentLevel.GetComponent<Level_Info>();

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

    private void ResetUI_Values()
    {
        player.GetComponent<PlayerController>().score = 0; //Reset values
        player.GetComponent<Health>().CurrentLifes = 3;
        UI_Manager.sharedInstance.UpdateLifesText(player.GetComponent<Health>()); //Update UI texts
        UI_Manager.sharedInstance.UpdateScoreText(player.GetComponent<PlayerController>().score);
        UI_Manager.sharedInstance.countDownTime = 150.0f; //Reset countdown
        UI_Manager.sharedInstance.countDownActive = true;
    }

    private void PlayWinSound()
    {
        AudioManager.sharedInstance.winCheers_SFX.Play();
    }

    private void PlayGameOverSound()
    {
        AudioManager.sharedInstance.gameOver_SFX.Play();
    }

    IEnumerator WaitToDestroyLevel()
    {
        yield return new WaitForSeconds(0.7f);
        DestroyCurrentLevel();
    }
}
