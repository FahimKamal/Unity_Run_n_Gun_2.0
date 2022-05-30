using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    GameManager gameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().DeactiveAimMode();
            gameManagerScript.CreateNewGround();
            gameManagerScript.DeleteGround();
            gameManagerScript.AddScore();
        }
    }

}
