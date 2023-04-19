using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public GameObject powerupIndicator;
    public GameObject missile;
    [SerializeField] float speed;
    [SerializeField] bool hasPowerup = false;
    [SerializeField] PowerupType powerupType;
    [SerializeField] float powerupStrength;
    [SerializeField] float jumpStrength;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private bool isOnGround = true;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        InvokeRepeating("ShootMissiles", 1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0f,-0.5f,0f);
        if(hasPowerup && powerupType == PowerupType.Slam && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            isOnGround = false; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupType = other.gameObject.GetComponent<Powerup>().powerupType;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup && powerupType == PowerupType.Push)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collided with " + collision.gameObject.name + " with Powerup set to " + hasPowerup);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            if (hasPowerup && powerupType == PowerupType.Slam)
            {
                Slam();
            }

        }
    }

    private void ShootMissiles()
    {
        if (hasPowerup && powerupType == PowerupType.Missile)
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                Vector3 offset = (enemy.gameObject.transform.position - transform.position).normalized * 2;
                GameObject currmissile = Instantiate(missile, transform.position + offset, missile.transform.rotation);
                currmissile.GetComponent<Missile>().enemy = enemy.gameObject;
            }
        }
    }

    private void Slam()
    {
        var enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.GetComponent<Rigidbody>().AddExplosionForce(powerupStrength, transform.position, 10f, 0f,ForceMode.Impulse);
        }
    }
}
