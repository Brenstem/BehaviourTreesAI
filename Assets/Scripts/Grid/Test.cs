using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private Grid grid;

    void Start()
    {
        grid = new Grid(6, 4, 10f, new Vector3(-15, 0));
        //new Grid(4, 2, 5f, new Vector3(-5, -3));
        //new Grid(3, 8, 8f, new Vector3(60, -10));
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //print(GetMouseWorldPosition());
            grid.AddToValue(GetMouseWorldPosition(), 1);
        }
        else if (Input.GetMouseButtonDown(1))
        { 
            print("value is " + grid.GetValue(GetMouseWorldPosition()));
        }
        else if (Input.GetMouseButtonDown(2))
        {
            grid.SetValue(GetMouseWorldPosition(), 0);
        }
    }



    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }
    Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
