using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float lifetime;
    Timer lifeTimer;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        lifeTimer = new Timer(lifetime, () => { Destroy(this.gameObject); });
    }
    private void Update()
    {
        lifeTimer.DecrementTimer(Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(this.tag))
        {
            if (other.CompareTag("Environment"))
            {
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                print("hit Enemy");
                Destroy(this.gameObject);
            }
            else if (other.tag == "Player")
            {
                print(other.CompareTag("Player"));
                Destroy(this.gameObject);
            }
        }
    }
}
