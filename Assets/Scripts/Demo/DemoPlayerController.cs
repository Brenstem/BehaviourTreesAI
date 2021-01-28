using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoPlayerController : MonoBehaviour
{
    [SerializeField] LayerMask environmentLayerMask;

    DemoWeaponScript weapon;

    NavMeshAgent agent;
    Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<DemoWeaponScript>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            if (Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask))
            {
                agent.SetDestination(hit.point);
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            weapon.FireWeapon();
        }
    }
}
