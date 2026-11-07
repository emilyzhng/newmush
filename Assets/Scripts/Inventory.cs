using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public MushroomScriptable data;
    public int numberof;

    public InventoryItem(MushroomScriptable data)
    {
        this.data = data;
        numberof = 1;
    }
}

public class Inventory : MonoBehaviour
{
    private int poisoned;
    private int nonpoisoned;

    public List<InventoryItem> items = new List<InventoryItem>();

    public LootSpawner spawner;

    public static bool goal1Met = false;
    public static bool goal2Met = false;
    public static bool goal3Met = false;

    // safe and poisonous mushrooms share a name, so match on the asset itself
    public void addItem(MushroomScriptable mush)
    {
        foreach (InventoryItem item in items)
        {
            if (item.data == mush)
            {
                item.numberof++;
                return;
            }
        }

        items.Add(new InventoryItem(mush));
    }


    public int returnNumberof(MushroomScriptable mush)
    {
        foreach (InventoryItem item in items)
        {
            if (item.data == mush)
            {
                return item.numberof;
            }
        }

        return 0;
    }


    public void CheckPoison()
    {
        poisoned = 0;
        nonpoisoned = 0;

        foreach (InventoryItem mush in items)
        {
            for (int i = 0; i < mush.numberof; i++)
            {
                if (mush.data.isPoisonous)
                {
                    poisoned++;
                }
                else
                {
                    nonpoisoned++;
                }
            }
        }

        Debug.Log("Poisonous mushrooms: " + poisoned);
        Debug.Log("Non-poisonous mushrooms: " + nonpoisoned);
    }


    public int ReturnPoisoned()
    {
        CheckPoison();
        return poisoned;
    }


    public int ReturnNonpoisoned()
    {
        CheckPoison();
        return nonpoisoned;
    }


    public void ResetInventory()
    {
        items.Clear();

        if (items.Count == 0)
        {
            Debug.Log("Inv is empty");
        }
    }


    public String DayEndDisplay()
    {
        String display = "";

        if (items.Count == 0)
        {
            display = "Nothing!";
            return display;
        }

        foreach (InventoryItem item in items)
        {
            display += item.data.mushroomName + " x" + item.numberof + "\n";
        }

        return display;
    }

    public bool HasRequiredIngredients()
    {
        // Goal 1
        goal1Met = returnNumberof(spawner.mushroomPrefabs[spawner.mushindex1].GetComponent<MushCategorization>().mushData) >= spawner.numgoal1;
        Debug.Log("Goal 1 met: " + goal1Met);

        // Goal 2
        if (spawner.numgoal2 > 0)
        {
            goal2Met = returnNumberof(spawner.mushroomPrefabs[spawner.mushindex2].GetComponent<MushCategorization>().mushData) >= spawner.numgoal2;
            Debug.Log("Goal 2 met: " + goal2Met);
        }
        else
        {
            goal2Met = true;
        }

        // Goal 3
        if (spawner.numgoal3 > 0)
        {
            goal3Met = returnNumberof(spawner.mushroomPrefabs[spawner.mushindex3].GetComponent<MushCategorization>().mushData) >= spawner.numgoal3;
            Debug.Log("Goal 3 met: " + goal3Met);
        }
        else
        {
            goal3Met = true;
        }

        return goal1Met && goal2Met && goal3Met;
    }
}