
using Unity.XR.GoogleVr;
using UnityEngine;

public class MushInst : MonoBehaviour
{
    public GameObject InspectCam;
    public GameObject InspectBack;
    public GameObject PlayerCam;
    public GameObject InspectButton;
    public GameObject InteractButton;
    public GameObject XButton;
    public GameObject InventoryMush;
    public GameObject Button;
    public GameObject CollectButton;
    public MushroomScriptable mushData;
    public PlayerMovement playerMovement;

    public Inventory playerInventory; 
    public InventoryUI inventoryUI;    
    public GameObject inv;
    public Inspect inspect;
    public GameObject book;
    public GameObject ExitInteract;
    public bool inShade;
    public SFX sfx;
    public ParticleSystem collectPoof;
    public ButtonManager button;
    public GameObject pg2;
    public GameObject pg3;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInventory = FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InteractButton.activeSelf)
        {
            InteractBook();
        }
        if (Input.GetKeyDown(KeyCode.E) && CollectButton.activeSelf)
        {
            PhysicalCollect();
        }
        if (Input.GetKeyDown(KeyCode.E) && CollectButton.activeSelf)
        {
            PhysicalCollect();
        }
        if (Input.GetKeyDown(KeyCode.E) && InspectButton.activeSelf)
        {
            MakeMush();
        }
    }
    public GameObject getMushroom()
    {
        return PlayerMovement.currentmush;
    }

    public void InteractBook()
    {
        book.SetActive(true);
        InspectBack.SetActive(true);
        InteractButton.SetActive(false);
        ExitInteract.SetActive(true);
        PlayerCam.SetActive(false);
        InspectCam.SetActive(true);
        sfx.OpenBook();
        if (button.getDay() == 2)
        {
            pg2.SetActive(true);
        }

        if (button.getDay() == 3)
        {
            pg3.SetActive(true);
        }
    }

    public void MakeMush()
    {
        Debug.Log("works");
        if (getMushroom() == null) return;

        Vector3 BackPos = InspectBack.transform.position; 
        Vector3 Offset = new Vector3(0f, 1.7f, -5f);
        InventoryMush = Instantiate(getMushroom(), BackPos + Offset, Quaternion.identity);
        Debug.Log("x " + InventoryMush);

        sfx.ButtonClick();

        InspectCam.SetActive(true);
        PlayerCam.SetActive(false);
        InspectButton.SetActive(false);
        XButton.SetActive(true);
        CollectButton.SetActive(true);
        inv.SetActive(false);
        InspectBack.SetActive(true);

        inspect.objectInspect = InventoryMush.transform;   
    }
    public void ExitInspect()
    {
        PlayerCam.SetActive(true);
        InspectCam.SetActive(false);
        XButton.SetActive(false);
        Destroy(InventoryMush);
        CollectButton.SetActive(false);
        inv.SetActive(true);
        InspectBack.SetActive(false);
        book.SetActive(false);
    }     

    public void ExitInterac()
    {
        ExitInteract.SetActive(false);
        InspectBack.SetActive(false);
        book.SetActive(false);
        CollectButton.SetActive(false);
        PlayerCam.SetActive(true);
        InspectCam.SetActive(false);
        sfx.OpenBook();
    }     
    public void PhysicalCollect()
    {
        MushCategorization mushroom = InventoryMush.GetComponent<MushCategorization>();
        
        Debug.Log("1 " + mushroom.mushData);

        playerInventory.addItem(mushroom.mushData);

        
        sfx.CollectObject();

        // Get the original mushroom's position
        Vector3 mushroomPosition = PlayerMovement.currentmush.transform.position;

        // Move the poof to where the mushroom was
        collectPoof.transform.position = mushroomPosition;

        // Play the poof
        collectPoof.Play();

        // Now remove the mushroom
        Destroy(InventoryMush);
        playerMovement.DestroyMush();

        InspectBack.SetActive(false);
        CollectButton.SetActive(false);
        XButton.SetActive(false);
        PlayerCam.SetActive(true);
        InspectCam.SetActive(false);
        inv.SetActive(true);
        Debug.Log(
    "\n========== MUSHROOM COLLECTED ==========" +
    "\nCollected: " + mushroom.mushData.mushroomName +
    "\nIs Poisonous: " + mushroom.mushData.isPoisonous +
    "\nPoisonous Count: " + playerInventory.ReturnPoisoned() +
    "\nNon-Poisonous Count: " + playerInventory.ReturnNonpoisoned() +
    "\nTotal Mushrooms: " + 
        (playerInventory.ReturnPoisoned() + playerInventory.ReturnNonpoisoned()) +
    "\nInventory Contents:\n" + playerInventory.DayEndDisplay() +
    "\n========================================"
);


        inventoryUI.RefreshUI();
    }
}

