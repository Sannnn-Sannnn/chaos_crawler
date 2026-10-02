using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreenManager : MonoBehaviour
  {

    [SerializeField] private int gameSceneIndex;

    public void StartGame()
    {
      SceneManager.LoadScene(gameSceneIndex);
    }

    public void Exit()
    {
      Application.Quit();
    }
    
  }