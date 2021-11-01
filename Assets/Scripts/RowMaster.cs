using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowMaster : MonoBehaviour
{
    public int numberOfRows;
    float heightPriv;

    private void Start()
    {
        numberOfRows = transform.childCount;
    }

    public void RowChecker(float height)
    {
        heightPriv = height;
        Invoke("RealRowChecker", 0.02f);   
    }

    void RealRowChecker()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).position.y < heightPriv)
            {

            }
            else if (transform.GetChild(i).position.y > heightPriv)
            {
                transform.GetChild(i).GetComponent<Row>().StartMovingDown();
            }
        }
    }
}
