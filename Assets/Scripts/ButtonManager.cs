using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ComponentModel.Design;

public class ButtonManager : MonoBehaviour
{
    public InventoryUI inventoryui;
    public Inventory inventory;
    public GameObject damper;
    public GameObject homeback;
    public GameObject hometext;
    public GameObject InspectCam;
    public GameObject PlayerCam;
    public GameObject SubmitButton;
    public GameObject NextDayButton;
    public GameObject NextButton;
    public TMP_Text DayText;
    public TMP_Text ObjectiveText;
    public TMP_Text ObjectiveDisplayText;
    public static int Day = 1;
    public static int ObjectiveIndex = 0;
    public TMP_Text DayReviewText;
    public Health healthclass;
    public GameObject homebackdaydisplay;
    public GameObject homebackbook;
    public GameObject house1;
    public GameObject house2;
    public GameObject house3;
    public GameObject player;
    private Vector3 offset1 = new Vector3 (12f, 0, 4f);
    private Vector3 offset2 = new Vector3 (12f, 5f, -8f);
    private Vector3 offset3 = new Vector3 (-12f, 0, 4f);
    public GameObject objectiveshow;
    public Timer time;
    public SFX sfx;
    public GameObject DayReviewTextt;
    public GameObject Day1pg1;
    public GameObject Day1pg2;
    public GameObject Day1Left;
    public GameObject Day1Right;
    public GameObject Day2pg1;
    public GameObject Day2pg2;
    public GameObject Day3pg1;
    public GameObject Day3pg2;
    public GameObject Day3pg3;







    public string[] Objective = { "Collect 2 Cubes", "Collect 2 Spheres and 2 Cubes", "Collect 1 Cylinders, 2 Cubes, and 2 Spheres" };
    
    public void Start()
    {

        TMP_Text DayTextWood = DayReviewTextt.GetComponent<TMP_Text>();
        String x = DayTextWood.text;
        x = "Day " + Day + " Review";
        DayTextWood.text = x;


        objectiveshow.GetComponent<TMP_Text>().text = "You have " + Mathf.FloorToInt(Timer.timeInSeconds / 60f) + " minutes to fulfill the objective...";

        homeback.SetActive(false);
        if (Day <= 3)
        {
            DayText.text = "Day " + Day + "...";
        } 
        Debug.Log("H " + Day);
        ObjectiveText.text = Objective[ObjectiveIndex];
        ObjectiveDisplayText.text = Objective[ObjectiveIndex];  

        if (Day == 1)
        {
            SpawnPlayer(house1.transform.position + offset1);
        }
        else if (Day == 2)
        {
            SpawnPlayer(house2.transform.position + offset2);
        }
        else if (Day == 3)
        {
            SpawnPlayer(house3.transform.position + offset3);
        }

        StartCoroutine(ShowObjective());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SubmitButton.activeSelf)
        {
            ClickSubmit();
        }
    }

    void SpawnPlayer(Vector3 spawn)
    {
        // some house spots are below the terrain surface, which can drop the player through the map,
        // so lift the spawn onto the ground of whichever terrain tile it is on
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            Vector3 local = spawn - terrain.transform.position;
            Vector3 size = terrain.terrainData.size;
            if (local.x >= 0 && local.z >= 0 && local.x <= size.x && local.z <= size.z)
            {
                float groundY = terrain.SampleHeight(spawn) + terrain.transform.position.y;
                spawn.y = Mathf.Max(spawn.y, groundY + 0.1f);
            }
        }

        player.transform.position = spawn;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.position = spawn;
        rb.linearVelocity = Vector3.zero;
    }

    IEnumerator ShowObjective()
        {
        yield return new WaitForSeconds(10f); // Wait 10 seconds

        objectiveshow.SetActive(true);

        yield return new WaitForSeconds(4f); // Stay visible for 4 seconds

        objectiveshow.SetActive(false);
        }

    public void ClickSubmit()
    {
        Debug.Log("helloo Inventory count = " + inventory.items.Count);
        Debug.Log("Collect Inventory ID: " + inventory.GetInstanceID());
        foreach (InventoryItem item in inventory.items)
        {
            Debug.Log("helloo" + item.data.mushroomName + " x" + item.numberof);

        }
        damper.SetActive(true);
        homeback.SetActive(true);
        InspectCam.SetActive(true);
        PlayerCam.SetActive(false);
        SubmitButton.SetActive(false);
        LeanTweenHandle.SlideInTB(homebackdaydisplay, 500, 2.5f);
        LeanTweenHandle.PopIn(homebackbook, 0.7f);
        sfx.OpenBook();

        TextMeshProUGUI tmpComponent = hometext.GetComponent<TextMeshProUGUI>();
        tmpComponent.text = inventory.DayEndDisplay();
        DayReviewText.text = "Day " + Day + " Review";

        inventoryui.DestoryUITracker();
    }

   public void ClickNext()
{
    sfx.OpenBook();
    bool enoughMushrooms = inventory.HasRequiredIngredients();
    bool atePoison = inventory.ReturnPoisoned() > 0;
    healthclass.DailyReview(enoughMushrooms, atePoison);

    TextMeshProUGUI tmpComponent = hometext.GetComponent<TextMeshProUGUI>();
    if (!enoughMushrooms)
    {
        tmpComponent.text = "You didn't collect enough mushrooms! You lost 2 hearts.";
        if (atePoison)
        {
            tmpComponent.text += "\nYou were also poisoned! Your time will be shortened by 1 minute the next day.";
        }
    }
    else if (atePoison)
    {
        tmpComponent.text = "You've Been Poisoned! You lost 1 heart, and your time will be shortened by 1 minute the next day.";
    }
    else
    {
        tmpComponent.text = "You collected enough mushrooms and were not poisoned!";
    }

    if (Health.currentHealth <= 0)
    {
        tmpComponent.text += "\nYou have no hearts left...";
    }

    inventory.ResetInventory();

    NextDayButton.SetActive(true);
    NextButton.SetActive(false);
}

    public void ClickNextDay()
    {
        // health can drop below 0 (poison and a failed day each cost 2), so treat anything at or below 0 as dead
        if (Health.currentHealth <= 0)
        {
            SceneManager.LoadScene("End");
            return;
        }

        if (Day == 3 && Health.currentHealth > 0)
        {
            SceneManager.LoadScene("End 1");
            return;
        }

        Day++;
        ObjectiveIndex++;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

    }

    public void Day1RightButtonClikc()
    {
        if (Day == 1)
        {
            Day1pg1.SetActive(false);
            Day1pg2.SetActive(true);
            Day2pg1.SetActive(false);
            Day2pg2.SetActive(false);
            Day1Right.SetActive(false);
            Day1Left.SetActive(true);
            sfx.OpenBook();

        } else if (Day == 2)
        {
            Day2pg1.SetActive(false);
            Day2pg2.SetActive(true);
            Day1pg1.SetActive(false);
            Day1pg2.SetActive(false);
            Day1Right.SetActive(false);
            Day1Left.SetActive(true);
            sfx.OpenBook();
        }
        if (Day == 3 && Day3pg2.activeSelf)
        {
            Day3pg3.SetActive(true);
            Day1Left.SetActive(true);
            Day1Right.SetActive(false);
            sfx.OpenBook();
        }
        if (Day == 3)
        {
            Day3pg2.SetActive(true);
            Day3pg1.SetActive(false);
            Day1Right.SetActive(true);
            Day1Left.SetActive(true);
            sfx.OpenBook();
        }
    }

    public void Day1LeftButtonClikc()
    {
        if(Day == 1)
        {
            Day1pg1.SetActive(true);
            Day1pg2.SetActive(false);
            Day2pg1.SetActive(false);
            Day2pg2.SetActive(false);
            Day1Left.SetActive(false);
            Day1Right.SetActive(true);
            sfx.OpenBook();
        } else if (Day == 2)
        {
            Day2pg1.SetActive(true);
            Day2pg2.SetActive(false);
            Day1pg1.SetActive(false);
            Day1pg2.SetActive(false);
            Day1Left.SetActive(false);
            Day1Right.SetActive(true);
            sfx.OpenBook();
        }

        if (Day == 3 & Day3pg3.activeSelf)
        {
            Day1Right.SetActive(true);
            Day3pg2.SetActive(true);
            Day3pg3.SetActive(false);
            sfx.OpenBook();
        }
        
        if (Day == 3)
        {
            Day3pg1.SetActive(true);
            Day3pg2.SetActive(false);
            Day1Left.SetActive(false);
            sfx.OpenBook();
        }
        
    }

    public int getDay()
    {
        return Day;
    }
}


