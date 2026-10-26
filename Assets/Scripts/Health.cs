
using UnityEngine;


public class Health : MonoBehaviour
{
    public HealthUI playerUI;
    public int maxHealth = 1;
    public static int currentHealth = 3;
    public static Health instance;
    public DailyEffects effects;
    public static bool poisoned;
    public Inventory inventory;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this one
        } else {
            Destroy(gameObject); // Destroy the duplicate
        }
    } 

    void Start()
    {
        if (currentHealth == 0) {
            currentHealth = maxHealth;
        }
        Debug.Log("Current Health: " + currentHealth);
        playerUI.UpdateHearts(currentHealth);
        Debug.Log("Poisoned Status: " + poisoned);
        if (poisoned == true)
        {
            Debug.Log("Huzzah");
            effects.RandomEffect();

            poisoned = false;
        }
    }

    // Daily review: not enough mushrooms costs 2 hearts (poisoned or not),
    // enough mushrooms but poisoned costs 1 heart. Returns the hearts lost.
    public int DailyReview(bool enoughMushrooms, bool atePoison)
    {
        int heartsLost = 0;
        if (!enoughMushrooms)
        {
            heartsLost = 2;
        }
        else if (atePoison)
        {
            heartsLost = 1;
        }

        currentHealth -= heartsLost;
        Debug.Log("Current Health: " + currentHealth);
        playerUI.UpdateHearts(currentHealth);

        if (atePoison)
        {
            poisoned = true;
            Debug.Log("Poisoned Status " + poisoned);
        }
        return heartsLost;
    }


    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}