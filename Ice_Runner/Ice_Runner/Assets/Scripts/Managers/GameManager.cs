using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gameState { mainMenu, inGame, deadScreen, leavingScreen, pauseScreen, winScreen }

public class GameManager : MonoBehaviour
{
    //Variables
    public static GameManager sharedInstance { get; private set; } //Singleton
    public gameState currentGameState = gameState.inGame;

    //References
    public GameObject player;
    public Camera mainCamera;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Start()
    {
        StartGame();
        ScaleCamera();
    }

    public void StartGame()
    {
        currentGameState = gameState.inGame;
        ScreensManager.sharedInstance.winScreen.SetActive(false);
        player.transform.position = GameObject.Find("PlayerStartPosition").transform.position;
        UnfreezePlayer();
    }

    public void FreezePlayer()
    {
        player.GetComponent<PlayerController>().anim.SetBool(player.GetComponent<PlayerController>().StopHashCode, true); //Stop player's idle animation
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnfreezePlayer()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void GoToWinScreen(int playerScore, float playerTime)
    {
        FreezePlayer();
        currentGameState = gameState.winScreen;
        StartCoroutine(EndLevel(playerScore, playerTime));
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
        UI_Manager.sharedInstance.countDownActive = false;
        UI_Manager.sharedInstance.inGameUI.SetActive(false);
        ScreensManager.sharedInstance.winScreen.SetActive(true);
        ScreensManager.sharedInstance.levelScore.text = playerScore.ToString();
        ScreensManager.sharedInstance.levelTime.text = playerTime.ToString();
    }
}
