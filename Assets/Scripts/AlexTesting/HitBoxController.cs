﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DanesUnityLibrary;

public class HitBoxController : MonoBehaviour
{
    [Header("Hitbox Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private Vector3 hitBoxExtents;
    [SerializeField] private LayerMask targetLayer;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private bool showHitBox;

    public bool isActive { get; private set; }
    public bool wasActive { get; private set; }

    private Timer hitBoxTimer;
    private bool doneDamage;

    private void Start()
    {
        hitBoxTimer = new Timer(0.01f);
    }

    private void Update()
    {
        wasActive = isActive;

        hitBoxTimer.DecrementTimer(Time.deltaTime);

        if (!hitBoxTimer.Done && !doneDamage)
        {
            if (debug)
                showHitBox = true;

            //delar på 2 för Physics.OverlapBox vill ha "halfExtents" men Gizmos.DrawWireCube vill ha "size" ¯\_(ツ)_/¯
            Collider[] hits = Physics.OverlapBox(transform.position, hitBoxExtents/2, Quaternion.identity, targetLayer);

            foreach (Collider col in hits)
            {
                if (col.GetComponent<Health>() == true)
                {
                    col.GetComponent<Health>().Damage(damage);
                    doneDamage = true;
                }
            }
        }
        else if (hitBoxTimer.Done)
        {
            if (debug)
                showHitBox = false;

            doneDamage = false;
            isActive = false;
        }
    }

    public void ExposeHitBox()
    {
        hitBoxTimer = new Timer(lifeTime);
        doneDamage = false;
        isActive = true;
    }

    public void OnDrawGizmos()
    {
        if (showHitBox)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, hitBoxExtents);
        }
    }
}