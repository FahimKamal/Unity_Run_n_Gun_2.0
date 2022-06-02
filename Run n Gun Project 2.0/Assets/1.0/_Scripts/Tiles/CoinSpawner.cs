using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinTrail;
    public GameObject health;
    // Start is called before the first frame update
    void Start()
    {
        bool spawn = Random.value > 0.5f;

        if (spawn)
        {
            if(Random.value > 0.5f)
                health.SetActive(true);
            else
                coinTrail.SetActive(true);
        }
    }

    
}
