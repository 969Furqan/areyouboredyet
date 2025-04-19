using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardMovement : MonoBehaviour
{
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public GameObject fireball;
    public float speed = 7.0f;
    public float fireballSpeed = 10.0f;
    public int maxFireballs = 5;
    public float rechargeTime = 2.0f;

    public AudioSource deathNoise;

    public int maxHits = 3;

    private int fireballCount = 0;
    private float cooldownTimer = 0.0f;
    private bool isRecharging = false;
    private int hitCount = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;
    private Vector2 lastFacingDirection = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastFacingDirection = movement;
        }

        UpdateSprite();
        Move();

        if (!isRecharging && Input.GetKeyDown(KeyCode.Space))
        {
            ShootFireball();
        }

        if (isRecharging)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isRecharging = false;
                fireballCount = 0;
            }
        }
    }

    void UpdateSprite()
    {
        if (movement.x > 0)
            spriteRenderer.sprite = rightSprite;
        else if (movement.x < 0)
            spriteRenderer.sprite = leftSprite;
        else if (movement.y > 0)
            spriteRenderer.sprite = upSprite;
        else if (movement.y < 0)
            spriteRenderer.sprite = downSprite;
    }

    void Move()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void ShootFireball()
    {
        if (isRecharging)
            return;

        if (fireballCount >= maxFireballs)
        {
            isRecharging = true;
            cooldownTimer = rechargeTime;
            return;
        }

        GameObject fireballInstance = Instantiate(fireball, transform.position, Quaternion.identity);
        float angle = Mathf.Atan2(lastFacingDirection.y, lastFacingDirection.x) * Mathf.Rad2Deg;
        fireballInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        Rigidbody2D rbFireball = fireballInstance.GetComponent<Rigidbody2D>();
        if (rbFireball != null)
        {
            rbFireball.velocity = lastFacingDirection.normalized * fireballSpeed;
        }
        else
        {
            Debug.LogError("Fireball prefab does not have a Rigidbody2D component attached.");
        }

        Destroy(fireballInstance, 1.5f);
        fireballCount++;
    }

    public void TakeHit()
    {
        hitCount++;

        if (hitCount >= maxHits)
        {
            deathNoise.Play();  
            Invoke("DestroyPlayer", deathNoise.clip.length);  
        }
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
        LoadScene("death");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
