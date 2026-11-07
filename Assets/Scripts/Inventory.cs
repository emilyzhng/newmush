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


    // total of one mushroom type, safe and poisonous together, for display
    public int returnNumberofType(string mushroomName)
    {
        int total = 0;

        foreach (InventoryItem item in items)
        {
            if (item.data.mushroomName == mushroomName)
            {
                total += item.numberof;
            }
        }

        return total;
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

        List<string> shown = new List<string>();

        foreach (InventoryItem item in items)
        {
            if (shown.Contains(item.data.mushroomName))
                continue;

            shown.Add(item.data.mushroomName);
            display += item.data.mushroomName + " x" + returnNumberofType(item.data.mushroomName) + "\n";
        }

        return display;
    }

    // goals count every mushroom of the type, poisonous or not;
    // poison is judged separately in the daily review
    public bool HasRequiredIngredients()
    {
        goal1Met = GoalMet(spawner.mushindex1, spawner.numgoal1);
        Debug.Log("Goal 1 met: " + goal1Met);

        goal2Met = GoalMet(spawner.mushindex2, spawner.numgoal2);
        Debug.Log("Goal 2 met: " + goal2Met);

        goal3Met = GoalMet(spawner.mushindex3, spawner.numgoal3);
        Debug.Log("Goal 3 met: " + goal3Met);

        return goal1Met && goal2Met && goal3Met;
    }

    bool GoalMet(int prefabIndex, int needed)
    {
        if (needed <= 0)
            return true;

        string type = spawner.mushroomPrefabs[prefabIndex].GetComponent<MushCategorization>().mushData.mushroomName;
        return returnNumberofType(type) >= needed;
    }
}