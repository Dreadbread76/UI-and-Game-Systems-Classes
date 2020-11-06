using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public LinearInventory inventory;


    [SerializeField]
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuActive();
        }
    }
    public void PauseMenuActive()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pausePanel.SetActive(true);
        }
        else
        {
           if (!LinearInventory.showInv)
            {
                Time.timeScale = 1;
            }
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pausePanel.SetActive(false);
        }
    }



}
