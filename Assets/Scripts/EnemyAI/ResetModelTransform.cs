using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetModelTransform : MonoBehaviour
{
    [SerializeField][Range(0, 0.1f)] private float xOffsetTolerance;
    [SerializeField][Range(0, 0.1f)] private float zOffsetTolerance;

    private Transform parentTransform;
    
    private void Awake()
    {
        parentTransform = GetComponentInParent<Transform>();
    }

    void FixedUpdate()
    {
        if (transform.position.x > xOffsetTolerance || transform.position.x < -xOffsetTolerance)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }

        if (transform.position.z > zOffsetTolerance || transform.position.z < -zOffsetTolerance)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
