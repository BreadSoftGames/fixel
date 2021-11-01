using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    Vector3 desiredPos;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = desiredPos = new Vector3(transform.position.x, target.transform.position.y + 13f, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target.transform.position.y < transform.position.y - 13)
        {
            desiredPos = new Vector3(transform.position.x, target.transform.position.y + 13f, transform.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, desiredPos, 0.05f);
    }
}
