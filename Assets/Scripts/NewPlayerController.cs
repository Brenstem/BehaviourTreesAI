using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] LayerMask environmentLayerMask;
    [SerializeField] float rotationSpeed;

    //[SerializeField] float debugRadius;
    //[SerializeField] GameObject debugObject;

    DemoWeaponScript weapon;
    NavMeshAgent agent;
    Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<DemoWeaponScript>();
        //rotationCorutine = FaceDirection(transform.forward);

        //agent.updateRotation = false;
    }

    void Update()
    {
        RaycastHit hit;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask);

        //transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

        if (Input.GetMouseButtonDown(0))
        {
            //RaycastHit hit;

            //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            if (Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask))
            {
                agent.SetDestination(hit.point);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            weapon.FireWeapon();

            //RaycastHit hit;

            //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            //if (Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask))
            //{
            //    StopCoroutine(rotationCorutine);
            //    rotationCorutine = FaceDirection((new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position).normalized);
            //    StartCoroutine(rotationCorutine);
            //}
        }
    }
}
