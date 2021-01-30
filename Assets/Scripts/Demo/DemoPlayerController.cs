using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoPlayerController : MonoBehaviour
{
    [SerializeField] LayerMask environmentLayerMask;
    [SerializeField] float rotationSpeed;


    DemoWeaponScript weapon;
    NavMeshAgent agent;
    Camera mainCamera;

    IEnumerator rotationCorutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<DemoWeaponScript>();
        rotationCorutine = FaceDirection(transform.forward);
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
        else if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

            if (Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask))
            {
                StopCoroutine(rotationCorutine);
                rotationCorutine = FaceDirection((new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position).normalized);
                StartCoroutine(rotationCorutine);
            }
        }
    }

    // wack 
    private IEnumerator FaceDirection(Vector3 targetDirection)
    {
        while (Vector3.Angle(transform.forward, targetDirection) > 5)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation.normalized, lookAtRotation.normalized, Time.time * rotationSpeed);

            yield return new WaitForEndOfFrame();
        }
        weapon.FireWeapon();
    }
}
