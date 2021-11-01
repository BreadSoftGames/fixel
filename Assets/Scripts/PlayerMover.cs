using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public GameObject playerAddon;
    public float lastAddonPos;
    public float speed;
    Rigidbody rb;
    public Animator currentAnim;

    public static bool gameOver;
    public static bool gameFailed;
    public static bool finished;

    public GameObject camera;

    bool stopped;
    public static bool changeAnim;
    private void Awake()
    {
        gameOver = false;
        gameFailed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        stopped = false;
        changeAnim = false;
        rb = transform.GetComponent<Rigidbody>();
        lastAddonPos = transform.GetChild(transform.childCount-1).transform.localPosition.y;
        Debug.Log(transform.GetChild(0).name);
        currentAnim = transform.GetChild(0).GetComponent<Animator>();
        camera.GetComponent<CameraMovement>().target = transform.GetChild(0);
        transform.GetChild(0).GetComponent<PlayerAddon>().inCharge = true;
    }

    private void Update()
    {
        InputChecker();
        if (!gameOver && changeAnim && transform.childCount !=0)
        {
            currentAnim = transform.GetChild(0).GetComponent<Animator>();
            camera.GetComponent<CameraMovement>().target = transform.GetChild(0);
            transform.GetChild(0).GetComponent<PlayerAddon>().inCharge = true;
            changeAnim = false;
        }
        else if(transform.childCount == 0 && !gameFailed)
        {
            gameFailed = true;
        }
    }

    void InputChecker()
    {
        if (Input.GetMouseButtonDown(0))
        {
            stopped = false;
        }
        else if (Input.GetMouseButton(0))
        {
            float xPos = Camera.main.ScreenPointToRay(Input.mousePosition).direction.x * 40f;
            Vector3 desiredPos = new Vector3(Mathf.Clamp(xPos, -6f, 6f), transform.position.y, transform.position.z);
            if ((transform.position.x - desiredPos.x) > 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.right);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                currentAnim.SetFloat("MoveSpeed", 1);
            }
            else if ((transform.position.x - desiredPos.x) < -0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.left);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                currentAnim.SetFloat("MoveSpeed", 1);
            }
            else if ((transform.position.x - desiredPos.x) > -0.1 && (transform.position.x - desiredPos.x) < 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                if(Mathf.Approximately(transform.position.x - desiredPos.x,0))
                currentAnim.SetFloat("MoveSpeed", 0);
            }
            transform.position = Vector3.MoveTowards(transform.position, desiredPos, 10 * Time.deltaTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            stopped = true;
        }

        if (stopped)
        {
            if (transform.rotation.eulerAngles != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
                currentAnim.SetFloat("MoveSpeed", 0);
            }
            else
            {
                stopped = false;
            }
        }

        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(downRay, out hit))
        {
            
        }

    }
}
