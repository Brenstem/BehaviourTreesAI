using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] public float currentHealth { get; private set; }
    [SerializeField] public float percentageHealth { get { return currentHealth / startingHealth; } }
    [SerializeField] public bool invulnerable;
    [SerializeField] private Slider healthbar;

    [SerializeField] private GameObject hurtParticle;

    [HideInInspector] public bool isDead;

    private BaseAI aiInstance;

    private Animator animator;

    void Start()
    {
        aiInstance = GetComponent<BaseAI>();

        animator = GetComponentInChildren<Animator>();

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
            if (aiInstance != null)
            {
                //Action.RaiseInterruptEvent(new InterruptEventArgs(GetComponent<BaseAI>().GetBehaviourTreeInstance().context.id));
                aiInstance.GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);
            }

            if (damageVal != 0)
                animator.SetTrigger("Hurt");

            currentHealth -= damageVal;

            if (hurtParticle != null)
                // Instantiate(hurtParticle, transform.position, transform.rotation);

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
                aiInstance.animator.SetTrigger("Hurt");
                aiInstance.GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);
            }

            if (GetComponent<GeneralEnemyAI>() != null)
            {
                GetComponent<GeneralEnemyAI>().SoldierAI.GetBehaviourTreeInstance().context.localData.Set<bool>("GeneralHurt", true);
                GetComponent<GeneralEnemyAI>().GetBehaviourTreeInstance().context.localData.Set<bool>("TookDamage", true);
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
            {
                if (GetComponent<GeneralEnemyAI>() != null)
                {
                    GetComponent<GeneralEnemyAI>().SoldierAI.GetBehaviourTreeInstance().context.localData.Set<bool>("GeneralDead", true);
                }

                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("Dead", true);

        if (GetComponent<NavMeshAgent>() != null)
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
