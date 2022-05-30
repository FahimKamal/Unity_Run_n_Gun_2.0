using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private int health = 100;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform bulletSpawnpoint;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject healthBar;
    [SerializeField]
    private DataContainer container;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        healthBar.transform.LookAt(mainCamera.transform);
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(bullet, bulletSpawnpoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 30f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2f, ForceMode.Impulse);

    }

    public void TakeDamage(int damageAmount)
    {
        healthBar.SetActive(true);
        var healthSlider = healthBar.gameObject.GetComponentInChildren<Slider>();
        health -= damageAmount;
        healthSlider.value = health / 100.0f;

        if(health <= 0)
        {
            // Play death animation
            animator.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled = false;
            container.coin += 30;
        }
        else
        {
            // Play take damage animation
            animator.SetTrigger("damage");
        }
    }
}
