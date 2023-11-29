using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    public List<Character> listCharacters;

    public GameObject mainMenuCanvas;
    
    public void CharacterSelected(int btnId)
    {
        switch (btnId)
        {
            case 0:
                Debug.Log(listCharacters[btnId]);
                GameInstance.instance.characterSelected(listCharacters[btnId]);
                SceneManager.LoadScene(1);
                break;
            default:
                Debug.Log("The character have not been implemented yet");
                break;
        }
    }
    
    public void GoBackToMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
