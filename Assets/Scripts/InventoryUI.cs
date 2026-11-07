using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory; 
    public GameObject itemPrefab;     
    public Transform Grid;   

    // one slot per mushroom type, so safe and poisonous of the same type share it
    public Dictionary<string, GameObject> UITracker = new Dictionary<string, GameObject>();

    public void RefreshUI()
    {
        foreach (var item in playerInventory.items)
        {
            UpdateItemUI(item.data);
        }
    }

    private void UpdateItemUI(MushroomScriptable mush)
    {
        string type = mush.mushroomName;
        int quantity = playerInventory.returnNumberofType(type);

        if (UITracker.ContainsKey(type))
        {
            TextMeshProUGUI quantityText = UITracker[type].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            quantityText.text = "x" + quantity;
        }
        else
        {
            GameObject go = Instantiate(itemPrefab, Grid);
            
            Image icon = go.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = mush.icon;

            TextMeshProUGUI quantityText = go.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            quantityText.text = "x" + quantity;

            UITracker[type] = go;
        }
    }

    public void DestoryUITracker()
    {
        foreach (var mush in UITracker)
        {
            Destroy(mush.Value);
        }

        UITracker.Clear();

        Debug.Log("Destroyed");
    }
}