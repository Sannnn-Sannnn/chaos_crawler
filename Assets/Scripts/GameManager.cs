using Dungeon;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Menus")] [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private int mainMenuSceneIndex;

    [Header("Game Objects")] [SerializeField]
    private DungeonGenerator dungeonGenerator;
    
    [Header("Stats")] [SerializeField] private int currentFloor;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }
    
    // Floor navigation
    
    public int CurrentFloor()
    {
        return  currentFloor;
    }

    public void NextFloor()
    {
        currentFloor++;
        ResetPlayerPosition();
        dungeonGenerator.RegenerateDungeon();
    }

    private void ResetPlayerPosition()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Vector3.zero;
        
        var playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        playerHealth.Restart();
    }
    
    // Death

    public void ShowDeath()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        Time.timeScale = 1f;
    }
    
    // Main Menu

    public void MainMenu()
    {

    }
    
    // Pause

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}