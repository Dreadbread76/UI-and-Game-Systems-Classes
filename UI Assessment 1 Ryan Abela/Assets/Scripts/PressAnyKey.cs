using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pressAnyKeyPanel, menuPanel;
    void Start()
    {
        menuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            menuPanel.SetActive(true);
            pressAnyKeyPanel.SetActive(false);
        }
    }
}
