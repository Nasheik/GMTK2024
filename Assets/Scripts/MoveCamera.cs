using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraTarget;

    void Update()
    {
        if(cameraTarget) transform.position = cameraTarget.position;
    }
}
