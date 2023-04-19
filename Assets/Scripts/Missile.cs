using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody missileRb;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        missileRb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy != null)
        {
            Vector3 lookDirection = (enemy.transform.position - transform.position).normalized;
            missileRb.AddForce(lookDirection * speed);

            if (transform.position.y < -10)
            {
                Destroy(gameObject);
            }
        }
    }
}
