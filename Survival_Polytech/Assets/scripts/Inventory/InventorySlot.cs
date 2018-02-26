﻿using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    public Image icon;

    Item item;

    public void AddItem (Item NewItem)
    {
        item = NewItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void UseItem()
    {
        Debug.Log("USING " + item.name);
        if (item !=null) item.Use();
    }    
}
