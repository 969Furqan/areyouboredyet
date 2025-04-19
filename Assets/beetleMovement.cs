using UnityEngine;

public class BeetleMovement : MonoBehaviour
{
    public Transform playerTransform;
    public float speed = 3.0f;
    public int health = 1;  
    private AudioSource deathSound;

    public DungeonGenerator dungeonGenerator;


    void Awake()
    {
        deathSound = GetComponent<AudioSource>();  
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - transform.position;
            direction.Normalize();
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<WizardMovement>().TakeHit();
        }
        else if (collision.gameObject.CompareTag("fireball"))
        {
            health -= 1;
            if (health <= 0)
            {
                if (deathSound != null)
                {
                    deathSound.Play();
                    Destroy(gameObject, deathSound.clip.length); 
                    if (dungeonGenerator != null)
                    {
                        dungeonGenerator.EnemyDefeated(); 
                    }
                }
                else
                {
                    Destroy(gameObject); // No sound to play, destroy immediately
                }
            }
            Destroy(collision.gameObject); // Destroy the fireball on impact
        }
    }

}
