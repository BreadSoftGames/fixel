using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAddon : MonoBehaviour
{
    public bool detached;
    public bool fell;
    public bool deducted;
    public Material somethingElse;

    public bool inCharge;
    bool finished;

    public static Material currendPlayerMat;

    GameObject addonPlace;
    public GameObject fallDust;
    public GameObject playerController;
    public GameObject playerAddon;

    Row row;

    Rigidbody rb;


    // Start is called before the first frame update
    void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Controller");
        detached = false;
        fell = false;
        deducted = false;
        inCharge = false;
        finished = false;
    }

    private void Start()
    {
        Material[] priv = new Material[2];
        priv[0] = currendPlayerMat;
        priv[1] = currendPlayerMat;
        somethingElse = currendPlayerMat;
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = priv;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Fall" && other.transform.GetComponent<FallInfo>().howManyAddons > 0 && !detached && inCharge)
        {
            if (transform.parent != null)
            {
                transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                other.transform.GetComponent<FallInfo>().howManyAddons--;
                transform.GetComponentInParent<PlayerMover>().lastAddonPos -= 2;
                transform.SetParent(null);
                PlayerMover.changeAnim = true;
                transform.GetComponent<Animator>().SetBool("Grounded", false);
                detached = true;
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
        }

        if (other.transform.tag == "Finish" && !detached && inCharge)
        {
            if (transform.parent != null)
            {
                transform.SetParent(null);
                PlayerMover.gameOver = true;
                PlayerMover.changeAnim = true;
                transform.GetComponent<Animator>().SetBool("Grounded", false);
                transform.position = new Vector3(other.transform.position.x, transform.position.y, transform.position.z);
                detached = true;
                transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
        }

        if (other.transform.tag == "Addon place" && fell && !deducted)
        {
            deducted = true;

            RaycastHit hitLeft;
            Ray left = new Ray(transform.position + Vector3.up, Vector3.left);

            if (Physics.Raycast(left, out hitLeft, 1f))
            {
                if(hitLeft.transform.tag == "Cube")
                {
                    hitLeft.transform.GetComponent<MeshRenderer>().material = somethingElse;
                }
            }

            RaycastHit hitRight;
            Ray right = new Ray(transform.position + Vector3.up, Vector3.right);

            if (Physics.Raycast(right, out hitRight, 1f))
            {
                if(hitRight.transform.tag == "Cube")
                {
                    hitRight.transform.GetComponent<MeshRenderer>().material = somethingElse;
                }
            }

            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            addonPlace = other.gameObject;
            transform.GetComponent<Animator>().SetBool("Grounded", true);
            transform.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
            transform.SetParent(addonPlace.GetComponentInParent<Transform>());
            transform.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            transform.gameObject.layer = 0;
            GameObject go = Instantiate(fallDust, transform.position,Quaternion.identity);
            var priv = go.transform.rotation;
            go.GetComponent<Renderer>().material = currendPlayerMat;
            priv.eulerAngles = new Vector3(90f, 0f, 0f);
            go.transform.rotation = priv;
            Invoke("Destruction", 1f);
        }

        if(other.transform.tag == "Finish place" && fell && !finished)
        {
            finished = true;
            PlayerMover.finished = true;
            transform.GetComponent<Animator>().SetBool("Grounded", true);
            transform.GetComponent<Animator>().SetFloat("MoveSpeed", 0);
            PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage") + 1);
        }
    }

    void Destruction()
    {
        addonPlace.GetComponentInParent<Row>().numberOfAddonPlaces--;
        if (addonPlace.GetComponentInParent<Row>().numberOfAddonPlaces <= 0)
        {
            Destroy(addonPlace);
            Destroy(transform.gameObject);
        }
        else
        {
            transform.SetParent(addonPlace.GetComponentInParent<Transform>());
            transform.position = new Vector3(transform.position.x, transform.GetComponentInParent<Transform>().position.y, transform.position.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (detached)
        {
            if (collision.transform.tag == "Cube")
            {
                fell = true;
            }

            if (collision.transform.tag == "Addon")
            {

                if (collision.transform.GetComponent<PlayerAddon>().fell)
                {
                    fell = true;
                }
            }
        }
        if (collision.transform.tag == "Pickup" && inCharge)
        {
            transform.GetComponentInParent<PlayerMover>().lastAddonPos += 2;
            GameObject go = Instantiate(playerAddon, new Vector3(0f, playerController.transform.GetChild(playerController.transform.childCount - 1).position.y + 2f, 0f), Quaternion.identity, transform);
            go.transform.SetParent(playerController.transform);
            go.transform.localPosition = new Vector3(0f, playerController.transform.GetChild(playerController.transform.childCount - 2).localPosition.y + 2f, 0f);
            go.transform.localScale = Vector3.one * 2;
            go.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            Destroy(collision.gameObject);
        }
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
