using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    public float time;

    void Start()
    {
        Invoke("DestroyYoSelf", time);
    }

    void DestroyYoSelf()
    {
        Destroy(transform.gameObject);
    }
}
