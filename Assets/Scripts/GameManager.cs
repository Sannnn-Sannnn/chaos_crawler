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
    [SerializeField] private int healthIncrement;
    
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        
        dungeonGenerator.RegenerateDungeon();
        ResetPlayerPosition();
        
        _playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        _playerHealth.Restart();
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
        _playerHealth.IncreaseMaxHealth(healthIncrement);
        dungeonGenerator.RegenerateDungeon();
    }

    private void ResetPlayerPosition()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        player.transform.position = playerSpawnPoint.transform.position;
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
        SceneManager.LoadScene(mainMenuSceneIndex);
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