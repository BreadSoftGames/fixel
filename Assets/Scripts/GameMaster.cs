using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public GameObject plane;
    public GameObject firstAddon;
    public GameObject gameFinishedPanel;
    public GameObject gameFailedPanel;
    public GameObject canvas;
    public GameObject confetti;
    public GameObject[] levels;
    public Material[] backgrounds;
    public Material[] cubeMat;
    public Material[] playerMat;
    public Material[] standardMat;
    public Material[] surroundingMat;
    public Text stageText;
    public Image finger;

    bool restartable;
    bool GOFlag;

    void Awake()
    {
        restartable = false;
        GOFlag = false;

        PlayerPrefs.SetInt("Stage", 4);
        if (!PlayerPrefs.HasKey("Stage"))
        {
            PlayerPrefs.SetInt("Stage", 1);
        }
        GameObject go = Instantiate(levels[(PlayerPrefs.GetInt("Stage") - 1)% 5], Vector3.zero, Quaternion.identity);
        firstAddon.transform.position = new Vector3(firstAddon.transform.position.x, go.GetComponent<LevelInfo>().startHeight, firstAddon.transform.position.z);
        PlayerAddon.currendPlayerMat = playerMat[(PlayerPrefs.GetInt("Stage") - 1) % 5];
        stageText.text = "STAGE " + PlayerPrefs.GetInt("Stage");
    }
    private void Start()
    {
        GameObject[] priv = new GameObject[GameObject.FindGameObjectsWithTag("Row").Length];
        priv = GameObject.FindGameObjectsWithTag("Row");
        foreach (var item in priv)
        {
            item.GetComponent<Row>().addonMat = playerMat[(PlayerPrefs.GetInt("Stage") - 1) % 5];
        }

        GameObject[] priv2 = new GameObject[GameObject.FindGameObjectsWithTag("Backgrounds").Length];
        priv2 = GameObject.FindGameObjectsWithTag("Backgrounds");
        foreach (var item in priv2)
        {
            item.GetComponent<MeshRenderer>().material = backgrounds[(PlayerPrefs.GetInt("Stage") - 1) % 5];
        }


        plane.GetComponent<MeshRenderer>().material = standardMat[(PlayerPrefs.GetInt("Stage") - 1) % 5];

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && finger.gameObject.activeInHierarchy)
        {
            finger.gameObject.SetActive(false);
        }
        if(Input.GetMouseButtonUp(0) && restartable)
        {
            SceneManager.LoadScene(0);
        }

        if(!GOFlag && PlayerMover.gameOver)
        {
            GOFlag = true;
            Instantiate(gameFinishedPanel, canvas.transform);
            Invoke("Confetti", 0.5f);
            Invoke("Restartable",1f);
        }
        if(!GOFlag && PlayerMover.gameFailed)
        {
            GOFlag = true;
            Instantiate(gameFailedPanel, canvas.transform);
            Invoke("Restartable", 1f);
        }
    }

    void Confetti()
    {
        confetti.SetActive(true);
    }

    void Restartable()
    {
        restartable = true;
    }
}
