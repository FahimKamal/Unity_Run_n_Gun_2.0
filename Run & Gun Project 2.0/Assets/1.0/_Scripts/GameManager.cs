using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> groundTiles;
    [SerializeField] List<GameObject> leftRightTiles;
    [SerializeField] List<GameObject> groundTileList = new List<GameObject>();
    [SerializeField] bool turnAvailable = false;
    public bool autoPilot = false;
    [SerializeField] int minCountTillTurnAvailable = 2;
    private int counter;

    public bool gameIsActive = false;

    private Transform spawnPosition;
    private Quaternion spawnRotation;
    private GameObject newGround;

    public UIAndbuttonHandler ui;
    public DataContainer container;

    // Start is called before the first frame update
    void Start()
    {
        gameIsActive = false;
        Time.timeScale = 1.0f;
        container.coin = 0;
        container.score = 0;
        container.autopilot = false;

        gameObject.GetComponent<AudioSource>().volume = container.volume;
    }
    public void IntGround()
    {
        
        for (int i = 0; i < 5; i++)
        {
            CreateGround();
        }
    }
    
    void CreateGround()
    {
        int lastIndex = groundTileList.Count - 1;
        spawnPosition = groundTileList[lastIndex].transform.Find("SpawnPoint");
        spawnRotation = spawnPosition.gameObject.transform.rotation;
        newGround = Instantiate(groundTiles[0], spawnPosition.position, spawnRotation);
        groundTileList.Add(newGround);
    }

    /// <summary>
    /// Creates new ground prepab upon called and adds it to list.
    /// </summary>
    public void CreateNewGround()
    {
        if (counter == minCountTillTurnAvailable)
        {
            turnAvailable = true;
            counter = 0;
        }

        int lastIndex = groundTileList.Count - 1;
        spawnPosition = groundTileList[lastIndex].transform.Find("SpawnPoint");
        spawnRotation = spawnPosition.gameObject.transform.rotation;

        if (turnAvailable && !autoPilot)
        {
            int randIndex = Random.Range(0, leftRightTiles.Count);

            newGround = Instantiate(leftRightTiles[randIndex], spawnPosition.position, spawnRotation);
            turnAvailable = false;
        }
        else
        {
            int randIndex = Random.Range(0, groundTiles.Count);

            newGround = Instantiate(groundTiles[randIndex], spawnPosition.position, spawnRotation);
        }

        groundTileList.Add(newGround);
        counter++;        
    }

    /// <summary>
    /// Delete the first element of the list.
    /// </summary>
    public void DeleteGround()
    {
        GameObject test = groundTileList[0];
        groundTileList.Remove(test);
        Destroy(test, 0.5f);
    }

    public void GameOver()
    {
        Debug.Log("You hit by a Obstacle.");
        gameIsActive = false;
        container.updateHiScore();
        ui.GameOver();
    }

    public void AddScore()
    {
        container.score += 10;
    }
}
