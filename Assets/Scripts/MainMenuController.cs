using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public UnityEngine.UI.Button startGameButton;
    public UnityEngine.UI.Button optionsButton;
    public Canvas OptionsMenu;
    public Canvas Story;

    void Start()
    {
        
    }

    public void ToggleOptionsMenu()
    {
        Debug.Log("Toggle display Options");
        OptionsMenu.enabled = !OptionsMenu.enabled;
    }

    public void ToggleStory()
    {
        Story.enabled = !Story.enabled;
    }

    public void ResetHighScores()
    {
        Debug.Log("Deleting all saved player preferences.");
        PlayerPrefs.DeleteAll();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
