using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] public float currentHealth { get; private set; }
    [SerializeField] public float percentageHealth { get { return currentHealth / startingHealth; } }
    [SerializeField] public bool invulnerable;
    [SerializeField] private Slider healthbar;

    [SerializeField] private GameObject hurtParticle;

    [HideInInspector] public bool isDead;

    void Start()
    {
        isDead = false;
        currentHealth = startingHealth;

        if (healthbar != null)
        {
            healthbar.maxValue = startingHealth;
            healthbar.value = currentHealth;
        }
    }

    public void Damage(float damageVal)
    {
        if (!invulnerable)
        {
            Debug.Log("Damage");

            if (GetComponent<BaseAI>() != null)
            {
                Debug.Log("Event raised");

                Action.RaiseInterruptEvent(new InterruptEventArgs(GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));
            }

            currentHealth -= damageVal;

            if (hurtParticle != null)
                Instantiate(hurtParticle, transform.position, transform.rotation);

            if (healthbar != null)
                healthbar.value = currentHealth;

            if (currentHealth <= 0)
                Die();
        }
    }

    private void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            print("ded");
            isDead = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
