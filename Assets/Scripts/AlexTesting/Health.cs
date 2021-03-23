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

    private DemoEnemyAI aiInstance;

    void Start()
    {
        aiInstance = GetComponent<DemoEnemyAI>();

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
            if (GetComponent<BaseAI>() != null)
            {
                //Action.RaiseInterruptEvent(new InterruptEventArgs(GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));
                aiInstance.GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);

                if (damageVal != 0)
                {
                    aiInstance.animator.SetTrigger("Hurt");
                }
            }

            currentHealth -= damageVal;

            if (hurtParticle != null && damageVal > 0)
                Instantiate(hurtParticle, transform.position, transform.rotation);

            if (healthbar != null)
                healthbar.value = currentHealth;

            if (currentHealth <= 0)
                Die();
        }
    }

    public void Damage(float damageVal, Vector3 hurtPFXDirection)
    {
        if (!invulnerable)
        {
            if (GetComponent<BaseAI>() != null)
            {
                //Action.RaiseInterruptEvent(new InterruptEventArgs(GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));

                aiInstance.GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);

                if (damageVal != 0)
                {
                    aiInstance.animator.SetTrigger("Hurt");
                }
            }

            currentHealth -= damageVal;

            if (hurtParticle != null && damageVal > 0) 
                Instantiate(hurtParticle, transform.position, Quaternion.LookRotation(hurtPFXDirection));

            if (healthbar != null)
                healthbar.value = currentHealth;

            if (currentHealth <= 0)
                Die();
        }
    }

    public void Damage(float damageVal, Vector3 hurtPFXDirection, AudioClip hitClip)
    {
        if (!invulnerable)
        {
            if (GetComponent<BaseAI>() != null)
            {
                //Action.RaiseInterruptEvent(new InterruptEventArgs(GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));

                aiInstance.GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);

                if (damageVal != 0)
                {
                    aiInstance.animator.SetTrigger("Hurt");
                }
            }

            currentHealth -= damageVal;

            if (hurtParticle != null && damageVal > 0)
            {
                GameObject instance = Instantiate(hurtParticle, transform.position, Quaternion.LookRotation(hurtPFXDirection));
                instance.GetComponent<AudioSource>().PlayOneShot(hitClip);
            }


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
            isDead = true;
        }
        else
        {
            isDead = true;
            aiInstance.animator.SetBool("Dead", true);
        }
    }
}
