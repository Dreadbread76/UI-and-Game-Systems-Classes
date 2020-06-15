using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text buttonText;
    public Text charNPCName;

    public string[] currentDialogue;
    public int index;
    public MouseLook playerMouseLook;
    

   

    private void OnGUI()
    {


    }
    public void DialogueScroll()
    {
        if (!(index >= currentDialogue.Length - 1))
        {
            index++;
            if(index >= currentDialogue.Length - 1)
            {
                buttonText.text = "Bye.";
            }
        }
        else
        {
            index = 0;
            Camera.main.GetComponent<MouseLook>().enabled = true;
            playerMouseLook.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            dialoguePanel.SetActive(false);
        }
        dialogueText.text = charNPCName + ": " + currentDialogue[index];
    }
}
