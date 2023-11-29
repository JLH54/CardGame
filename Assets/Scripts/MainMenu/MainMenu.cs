using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject characterSelectionCanvas;

    public GameObject settingsCanvas;

    public void PlayButton()
    {
        characterSelectionCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SettingsButton()
    {
        settingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
