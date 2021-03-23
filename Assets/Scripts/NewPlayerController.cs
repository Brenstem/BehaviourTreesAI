using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField] LayerMask environmentLayerMask;
    [SerializeField] float movementSpeed;
    [SerializeField] float shootCooldown;

    DemoWeaponScript weapon;
    Camera mainCamera;
    Animator animator;
    Rigidbody rigidbody;

    bool canShoot = true;

    private void Awake()
    {
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<DemoWeaponScript>();

        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Physics.Raycast(mousePosition, mainCamera.transform.forward, out hit, Mathf.Infinity, environmentLayerMask);
        transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        rigidbody.velocity = (transform.forward * moveZ + transform.right * moveX).normalized * movementSpeed;
        animator.SetFloat("moveZ", moveZ);
        animator.SetFloat("moveX", moveX);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && canShoot)
        {
            weapon.FireWeapon();

            animator.SetTrigger("Shoot");

            canShoot = false;
            StartCoroutine(ShootTimer());
        }
    }
    IEnumerator ShootTimer()
    {
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
