using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreensManager : MonoBehaviour
{
    //Variables
    public static ScreensManager sharedInstance { get; private set; }

    //References
    public Image skinSelected; //Screen manager has all skins previews
    public Sprite[] penguinSkins;
    public GameObject[] screens;
    public GameObject darkBackground;
    public TransitionFadeIn transitionAnim;
    public Text levelTime;
    public Text levelScore;
    public Text fpsText;
    public Slider mainThemeVolumeBar;
    public Slider sfxVolumeBar;
    private float deltaTime;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil(fps).ToString();
    }

    public void ShowSettings(bool settingsEnabled)
    {
        screens[7].SetActive(settingsEnabled);
    }

    public void EnableButtonBack(Button button)
    {
        button.interactable = !button.interactable;
    }

    public void ShowWannaLeaveMM(bool settingsEnabled)
    {
        screens[8].SetActive(settingsEnabled);
    }

    public void ShowWannaLeaveIG(bool settingsEnabled)
    {
        screens[9].SetActive(settingsEnabled);
    }

    public void StartTransitionAnim(string screenName)
    {
        transitionAnim.nextScreen = screenName;
        transitionAnim.gameObject.SetActive(true);
    }

    public void HideAllScreens()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(false);
        }
    }

    public void DisableButton(Button button)
    {
        StartCoroutine(ButtonInteractable(button));
    }

    public void EnableScreen(string screenName) //This method enables the screen we want, and disables all other screens
    {
        if (screenName.CompareTo("MainMenu") == 0)
        {
            darkBackground.SetActive(false);

            if (GameObject.FindGameObjectWithTag("Level") != null)
            {
                GameManager.sharedInstance.DestroyCurrentLevel();
                GameManager.sharedInstance.player.transform.position = Vector3.zero;
            }
        }

        for (int i = 0; i < screens.Length; i++)
        {
            if (screens[i].name.CompareTo(screenName) == 0)
                screens[i].SetActive(true);
            else if (screens[i].activeSelf)
                screens[i].SetActive(false);
        }
    }

    public void SetPreviewSkin(int spriteIndex)
    {
        skinSelected.sprite = penguinSkins[spriteIndex];

    }

    IEnumerator ButtonInteractable(Button button)
    {
        button.interactable = false;
        yield return new WaitForSeconds(1.5f);
        button.interactable = true;
    }
}

