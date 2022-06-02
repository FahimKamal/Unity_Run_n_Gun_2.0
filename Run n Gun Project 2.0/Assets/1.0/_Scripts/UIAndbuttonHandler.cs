using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIAndbuttonHandler : MonoBehaviour
{
    public PlayerController playerController;
    public GameManager gameManager;
    public GameObject aimCanvas;
    public GameObject thirdPersonCanvas;

    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject ui;
    public GameObject gameOverScreen;

    public Slider volumeSlider;
    public Slider playerHP;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;

    public DataContainer container;

    private bool isGamePlaying = false;
    private bool pauseMenuActive = false;
    private int optionMenuBackAddress;

    private void Start()
    {
        isGamePlaying = false;
        pauseMenuActive = false;
        volumeSlider.value = container.volume;
        hiScoreText.text = "Hi Score: " + container.hiScore.ToString();        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenuActive)
        {
            if (isGamePlaying)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }            
        }

        coinText.text ="Coin: " + container.coin.ToString();
        scoreText.text = "Score: " + container.score.ToString();
        // 
        if (container.score > container.hiScore)
        {
            hiScoreText.text = "Hi Score: " + container.score.ToString();
        }
        playerHP.value = playerController.playerHP;
    }

    void PauseGame()
    {
        isGamePlaying = false;
        Time.timeScale = 0.0f;
        gameManager.gameIsActive = false;
        ui.SetActive(false);
        pauseMenu.SetActive(true);

        if (playerController.isAimModeActive)
        {
            aimCanvas.SetActive(false);
        }
        else
            thirdPersonCanvas.SetActive(false);
        
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResumeGame()
    {
        isGamePlaying = true;
        Time.timeScale = 1.0f;
        gameManager.gameIsActive = true;
        ui.SetActive(true);
        pauseMenu.SetActive(false);
        //Cursor.lockState = CursorLockMode.Locked;

        if (playerController.isAimModeActive)
        {
            aimCanvas.SetActive(true);
        }
        else
            thirdPersonCanvas.SetActive(true);
    }

    public void StartGame(int difficulty)
    {
        if(difficulty == 1)
        {
            playerController.movementSpeed = playerController.easyMovementSpeed;
        }
        else if(difficulty == 2)
        {
            playerController.movementSpeed = playerController.normalMovementSpeed;
        }
        else if (difficulty == 3)
        {
            playerController.movementSpeed = playerController.hardMovementSpeed;
        }

        thirdPersonCanvas.SetActive(true);
        pauseMenuActive = true;
        isGamePlaying = true;
        mainMenu.SetActive(false);
        ui.SetActive(true);
        gameManager.gameIsActive = true;
        gameManager.IntGround();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("My Game");
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuActive = false;
        ui.SetActive(false);
        aimCanvas.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScreen.transform.Find("Score Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Your Score: " + container.score.ToString();
        gameOverScreen.transform.Find("Coin Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Coins Collected: " + container.coin.ToString();
    }

    public void UpdateVolume()
    {
        gameManager.gameObject.GetComponent<AudioSource>().volume = volumeSlider.value;
        container.volume = volumeSlider.value;
    }

    public void OptionButton(int address)
    {
        optionMenuBackAddress = address;
    }

    public void OptionMenuBackButton()
    {
        // Main menu
        if(optionMenuBackAddress == 0)
        {
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);
        }
        // Pause menu
        else if(optionMenuBackAddress == 1)
        {
            pauseMenu.SetActive(true);
            optionMenu.SetActive(false);
        }
    }

    public void AutoPilotButton()
    {
        container.autopilot = !container.autopilot;
        gameManager.autoPilot = true;
        playerController.isPlayerDead = true;
        playerController.transform.Find("Auto Pilot").gameObject.SetActive(true);
    }
}
