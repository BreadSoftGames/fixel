using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public int numberOfAddonPlaces;
    public GameObject playerPickup;
    public Material addonMat;

    // Start is called before the first frame update
    void Start()
    {
        numberOfAddonPlaces = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == "Addon place")
            {
                numberOfAddonPlaces++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfAddonPlaces <= 0 && transform.childCount !=0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).tag == "Jail cube")
                {
                    GameObject go = Instantiate(playerPickup, transform.GetChild(i).position + Vector3.down, Quaternion.identity);
                    go.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    //go.GetComponent<PlayerAddon>().somethingElse = addonMat;
                    Destroy(transform.GetChild(i).gameObject);
                }
                else
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }
        }
        else if(transform.childCount == 0)
        {
            Destroy(transform.gameObject);
            transform.GetComponentInParent<RowMaster>().numberOfRows--;
            transform.GetComponentInParent<RowMaster>().RowChecker(transform.position.y);
        }
    }

    public void StartMovingDown()
    {
        StartCoroutine(MoveOverSpeed(transform.gameObject, transform.position + Vector3.down * 2, 10f));
    }

    public IEnumerator MoveOverSpeed(GameObject objectToMove, Vector3 end, float speed)
    {
        while (objectToMove.transform.position != end)
        {
            objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, end, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
