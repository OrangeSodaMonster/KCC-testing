using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
    [SerializeField] Transform racerToFollow;

    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;

    Quaternion newRotation;
    Vector3 newPosition;

    void Start()
    {
        //    transform.position = new Vector3(racerToFollow.position.x, 0, racerToFollow.position.z) + positionOffset;
        //    transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y + racerToFollow.rotation.eulerAngles.y, rotationOffset.z);
    }

    void LateUpdate()
    {
        Vector3 racerPosNoY = new Vector3(racerToFollow.position.x, 0, racerToFollow.position.z);
        newPosition = racerPosNoY + positionOffset;

        float racerYRot = racerToFollow.rotation.eulerAngles.y;
        newRotation = Quaternion.Euler(rotationOffset.x, transform.rotation.eulerAngles.y, rotationOffset.z);

        transform.position = newPosition;
        transform.rotation = newRotation;
    }
}
