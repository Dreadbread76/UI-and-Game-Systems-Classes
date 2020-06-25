using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//this script can be found in the Component section under the option Intro RPG/Player/Interact
[AddComponentMenu("Intro PRG/RPG/Player/Interact")]
public class Interact : MonoBehaviour
{
    #region Variables
    public Stats.BaseStats player;
    public CanvasDialogue dlgMaster;
    public LinearInventory linearInventory;
    //[Header("Player and Camera connection")]
    //create two gameobject variables one called player and the other mainCam
    #endregion
    #region Start
    //connect our player to the player variable via tag
    //connect our Camera to the mainCam variable via tag
    #endregion
    #region Update  

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("KeyPressed");
            Ray interact;
            interact = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo;
            if (Physics.Raycast(interact, out hitInfo, 10))
            {
                #region NPC tag
                //and that hits info is tagged NPC
                //Debug that we hit a NPC             
                
                if (hitInfo.collider.CompareTag("NPC"))
                {
                    Debug.Log("Talk to the NPC");
                    
                }
                #endregion
                #region Item
                if (hitInfo.collider.CompareTag("Item"))
                {
                    Debug.Log("Pick Up Item");
                    ItemHandler handler = hitInfo.transform.GetComponent<ItemHandler>();
                    if(handler != null)
                    {
                        player.quest.goal.ItemCollected(handler.itemId);
                        handler.OnCollection();
                    }
                }
                #endregion
                #region Chest
                if (hitInfo.collider.CompareTag("Chest"))
                {
                    Chest chest = hitInfo.transform.GetComponent<Chest>();
                    if(chest != null)
                    {
                        chest.showChestInv = true;
                        LinearInventory.showInv = true;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        Time.timeScale = 0;
                        LinearInventory.currentChest = chest;
                    }
                }
                #endregion
                //and that hits info is tagged Item
                //Debug that we hit an Item               
                
            }
        }
    }
    //if our interact key is pressed
    //create a ray
    //this ray is shooting out from the main cameras screen point center of screen
    //create hit info
    //if this physics raycast hits something within 10 units
    
    

    //and that hits info is tagged Item
    //Debug that we hit an Item               
   
    #endregion
}






