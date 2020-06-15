
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearInventory : MonoBehaviour
{
    public Stats.BaseStats player;
    public static List<Item> inv = new List<Item>();
    public Item selectedItem;
    public static bool showInv;
    public GUIStyle style;
    public GUISkin skin;

    public Vector2 scr;
    public Vector2 scrollPos;
    public string sortType = "";
    public string[] enumTypesForItems;
    public static int money;

    public Image itemImage;
    public Text itemName;
    public Text itemValue;
    public Text itemDescription;
    public Text itemHealDmgArm;
    public ScrollView itemCarryList;
    public GameObject inventoryScreen;
    

    public Transform dropLocation;
    [System.Serializable]
    public struct Equipment
    {
        public string slotName;
        public Transform equipLocation;
        public GameObject currentItem;
    };
    public Equipment[] equipmentSlots;


    public static Chest currentChest;
    public static Shop currentShop;
    // Start is called before the first frame update
    void Start()
    {
        
        player = this.gameObject.GetComponent<Stats.BaseStats>();
        enumTypesForItems = new string[] { "All", "Food", "Weapon", "Apparel", "Crafting", "Ingredients", "Potions", "Scrolls", "Quest" };
        inv.Add(ItemData.CreateItem(0));
        inv.Add(ItemData.CreateItem(1));
        inv.Add(ItemData.CreateItem(30));

        
    }

    // Update is called once per frame
    void Update()
    {
        
        scr.x = Screen.width / 16;
        scr.y = Screen.height / 9;
        if (currentShop == null)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                showInv = !showInv;
                if (showInv)
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    return;
                }
                else
                {
                    Time.timeScale = 1;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    currentChest.showChestInv = false;
                    currentChest = null;
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInv = !showInv;
            if (showInv)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                currentChest.showChestInv = false;
                currentChest = null;
                return;
            }
        }
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.I))
        {
            inv.Add(ItemData.CreateItem(Random.Range(0, 3)));
                inv.Add(ItemData.CreateItem(Random.Range(100, 103)));
                inv.Add(ItemData.CreateItem(Random.Range(200, 203)));
        }
        if (Input.GetKey(KeyCode.N))
        {
            sortType = "Food";
        }
        if (Input.GetKey(KeyCode.M))
        {
            sortType = "All";
        }
#endif
    }
    void Display()
    {
        if(sortType == "All"  || sortType == "")
        {
            if (inv.Count <= 34)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + i * (0.25f * scr.y), 3f * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                        
                        
                    }
                }
            }
            else
            {
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, 8.5f * scr.y + (inv.Count - 34)), false, true);
                  #region EVERYTHING DISPLAYED INSIDE SCROLL VIEW

                  #endregion
                  
                 

                ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), sortType);
                int a = 0;
                int s = 0;
                for (int i = 0; i< inv.Count; i++)
                {
                    if(inv[i].Type == type)
                    {
                        a++;
                    }
                }
                if (a <= 34)
                {
                    for (int i = 0; i < inv.Count; i++)
                    {
                        if (inv[i].Type == type)
                        {
                            if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + s * (0.25f * scr.y), 3f * scr.x, 0.25f * scr.y),inv[i].Name))
                            {
                                selectedItem = inv[i];
                            }
                            s++;
                        }
                    }

                }
                else
                {
                    scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, 8.5f * scr.y + (inv.Count - 34)), false, true);
                    for (int i = 0; i < inv.Count; i++)
                    {
                        if (inv[i].Type == type)
                        {
                            if (GUI.Button(new Rect(0.5f * scr.x,  s * (0.25f * scr.y), 3f * scr.x, 0.25f * scr.y), inv[i].Name))
                            {
                                selectedItem = inv[i];
                            }
                            s++;
                        }
                    }

                    GUI.EndScrollView();
                }
            }
        }
        
    }
    private void OnGUI()
    {
        if (showInv)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            for (int i = 0; i < enumTypesForItems.Length; i++)
            {
                if(GUI.Button(new Rect(4f*scr.x + i * scr.x, 0, scr.x, 0.25f * scr.y), enumTypesForItems[i]))
                {
                    sortType = enumTypesForItems[i];
                }
            }
            Display();
            if(selectedItem != null)
            {
                UseItem();
            }
        }
    }
    void UseItem()
    {
        GUI.Box(new Rect(8f * scr.x, 0.25f * scr.y, 3.5f * scr.x, 7f * scr.y), "", style);
        //GUI.Box(new Rect(4.25f * scr.x, 0.5f * scr.y, 3f * scr.x, 3f * scr.y), selectedItem.Icon);
        GUI.skin = skin;
        GUI.Box(new Rect(8.25f * scr.x, 0.5f * scr.y, 3f * scr.x, 0.5f * scr.y), selectedItem.Name);
        if(currentChest != null)
        {
            if (GUI.Button(new Rect(8.25f * scr.x,6.75f * scr.y, scr.x, 0.25f * scr.y), "Move Item"))
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                    if (equipmentSlots[i].currentItem != null && selectedItem.Name == equipmentSlots[i].currentItem.name)
                    {
                        Destroy(equipmentSlots[i].currentItem);
                    }
            }
            currentChest.chestInv.Add(selectedItem);
            if(selectedItem.Amount > 1)
            {
                selectedItem.Amount--;
            }
            else
            {
                inv.Remove(selectedItem);
                selectedItem = null;
                return;
            }
        }
        GUI.skin = null;

        
        switch (selectedItem.Type)
        {

            #region Food
            case ItemType.Food:
                GUI.Box(new Rect(8.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);

                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "Heal: " + selectedItem.Heal.ToString();
                

                if (player.characterStatus[0].currentValue < player.characterStatus[0].maxValue)
                {
                    if (GUI.Button(new Rect(10.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Eat"))
                    {
                        player.characterStatus[0].currentValue += selectedItem.Heal;
                        if (selectedItem.Amount > 1)
                        {
                            selectedItem.Amount--;
                        }
                        else
                        {
                            inv.Remove(selectedItem);
                            selectedItem = null;
                            return;
                        }
                    }
                }

                
                break;
            #endregion
            #region Weaponry
            case ItemType.Weapon:
                GUI.Box(new Rect(8.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);

                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "Damage: " + selectedItem.Damage.ToString();

                if (equipmentSlots[2].currentItem == null || selectedItem.Name != equipmentSlots[2].currentItem.name) 
                {
                    if (GUI.Button(new Rect(10.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        if (equipmentSlots[2].currentItem != null)
                        {
                            Destroy(equipmentSlots[2].currentItem);
                        }
                        GameObject curItem = Instantiate(selectedItem.Mesh, equipmentSlots[2].equipLocation);
                        equipmentSlots[2].currentItem = curItem;
                        curItem.name = selectedItem.Name;

                    }
                }
                else
                {
                    if (GUI.Button(new Rect(8.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        Destroy(equipmentSlots[3].currentItem);
                    }
                }
                break;
            #endregion
            #region Apparel
            case ItemType.Apparel:
                GUI.Box(new Rect(8.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);

                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "Armour: " + selectedItem.Armour.ToString();

                if (equipmentSlots[3].currentItem == null || selectedItem.Name != equipmentSlots[3].currentItem.name)
                {
                    if (GUI.Button(new Rect(10.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        if (equipmentSlots[3].currentItem != null)
                        {
                            Destroy(equipmentSlots[3].currentItem);
                        }
                        GameObject curItem = Instantiate(selectedItem.Mesh, equipmentSlots[3].equipLocation);
                        equipmentSlots[3].currentItem = curItem;
                        curItem.name = selectedItem.Name;

                    }
                }
                else
                {
                    if (GUI.Button(new Rect(10.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        Destroy(equipmentSlots[3].currentItem);
                    }
                }
                break;
            #endregion
            #region Crafting
            case ItemType.Crafting:
                GUI.Box(new Rect(10.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "";
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            #endregion
            #region Ingredients
            case ItemType.Ingredients:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);

                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "";

                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            #endregion
            #region Potions
            case ItemType.Potions:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "Heal: " + selectedItem.Heal.ToString();
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Drink"))
                {

                }
                break;
            #endregion
            #region Scrolls
            case ItemType.Scrolls:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.ItemDescription + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                itemImage.sprite = selectedItem.Icon;
                itemName.text = selectedItem.Name + " x" + selectedItem.Amount;
                itemValue.text = "Value: " + selectedItem.Value.ToString();
                itemDescription.text = selectedItem.ItemDescription;
                itemHealDmgArm.text = "Heal: " + selectedItem.Heal.ToString();
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            #endregion
            #region Quest
            case ItemType.Quest:
                break;
            #endregion
            case ItemType.Money:
                break;
            default:
                break;
        }
        //DISCARD BUTTON
        if(GUI.Button(new Rect(8.25f * scr.x, 4f * scr.y, 3f *scr.x,1f* scr.y), "Discard"))
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if (equipmentSlots[i].currentItem != null && selectedItem.Name == equipmentSlots[i].currentItem.name)
                {
                    Destroy(equipmentSlots[i].currentItem);
                }
            }
            GameObject droppedItem = Instantiate(selectedItem.Mesh, dropLocation.position, Quaternion.identity);
            droppedItem.name = selectedItem.Name;
            droppedItem.AddComponent<Rigidbody>().useGravity = true;
            droppedItem.GetComponent<ItemHandler>().enabled = true;
            if (selectedItem.Amount > 1)
            {
                selectedItem.Amount--;
            }
            else
            {
                inv.Remove(selectedItem);
                selectedItem = null;
                return;
            }
        }
    }
}
