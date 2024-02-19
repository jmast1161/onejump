using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    WaitingGameplayInput,
    MenuActive
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    
    [SerializeField] 
    private UnityEngine.UI.Button retryButton;
    [SerializeField]
    private UnityEngine.UI.Button nextLevelButton;
    private GameState gameState;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
        player.LevelComplete += OnLevelComplete;
        player.PlayerDied += OnPlayerDied;
        gameState = GameState.WaitingGameplayInput;
        retryButton.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        if (buildIndex == 3)
        {
            var nextLevelButtonText = nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();
            nextLevelButtonText.text = "Thanks For Playing!";
        }

        nextLevelButton.onClick.AddListener(() => {
            if (buildIndex == 3)
            {
                SceneManager.LoadScene(0);
            }
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            gameState = GameState.WaitingGameplayInput;
        });
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.WaitingGameplayInput:
                retryButton.gameObject.SetActive(false);
                nextLevelButton.gameObject.SetActive(false);
                player.MovePlayer();
                break;
            case GameState.MenuActive:
                break;
        }
    }

    private void OnLevelComplete(Player player)
    {
        retryButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(true);
        gameState = GameState.MenuActive;
        player.StopPlayer();
    }

    private void OnPlayerDied(Player player)
    {
        retryButton.gameObject.SetActive(true);
        gameState = GameState.MenuActive;
        player.StopPlayer();
    }
}
