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
    private GameObject _playerHitBox;

    private void Awake()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        
        dungeonGenerator.RegenerateDungeon();
        ResetPlayerPosition();
        
        // _playerHealth = GameObject.FindWithTag("PlayerHitBox").GetComponent<PlayerHealth>();
        _playerHitBox = GameObject.FindWithTag("PlayerHitBox");
        Debug.Log("PlayerHitBox: " + _playerHitBox);
        _playerHealth = _playerHitBox.GetComponent<PlayerHealth>();
        Debug.Log("PlayerHealth: " + _playerHealth);
        _playerHealth.Restart();
    }

    private void FixedUpdate()
    {
        if (_playerHitBox == null)
        {
            Debug.Log("PlayerHitBox is null ");
        }
        else if (_playerHealth == null)
        {
            Debug.Log("PlayerHealth is null ");
        }
        else
        {
            Debug.Log("ALL OK");
        }
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
        Debug.Log("PlayerHealth: " + _playerHealth);
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