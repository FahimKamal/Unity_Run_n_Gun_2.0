using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTurn : MonoBehaviour
{
    Collider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().turnAvailable = true;
            boxCollider.gameObject.SetActive(false);
        }
    }
}
