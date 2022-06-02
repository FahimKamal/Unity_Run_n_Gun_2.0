using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject impactEffect;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(impact, 3f);
        FindObjectOfType<AudioManager>().Play("Explosion");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player taking damage.");
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(18.5f);
        }
    }

}
