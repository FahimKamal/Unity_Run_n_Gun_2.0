using UnityEngine;

public enum pickupType { Coin, Health}

public class CoinRotateAndTrigger : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    public AudioSource audioSource;
    public AudioClip coinPickup;
    public DataContainer container;
    public pickupType type;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 180);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            audioSource.PlayOneShot(coinPickup, 0.2f);
            Destroy(gameObject, 0.5f);

            if(type == pickupType.Coin)
            {
                container.coin++;
                container.totalCoin++;
            }
            else if(type == pickupType.Health)
            {
                var playerHP = other.GetComponent<PlayerController>().playerHP;
                playerHP = Mathf.Clamp(playerHP += 30.0f, 0.0f, 100.0f);
                other.GetComponent<PlayerController>().playerHP = playerHP;
            }

            
        }
    }
}
