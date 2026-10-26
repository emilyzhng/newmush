using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float startingTime = 240f;
    public static float timeInSeconds;
    public static float penaltySeconds;

    public GameObject homeback;
    public GameObject damper;
    public GameObject timesup;
    public Slider timeBar;
    public GameObject player;
    public TMP_Text timerText;

    void Start()
    {
        timeInSeconds = startingTime;
        timeBar.value = 1f;
    }

    void Update()
    {
        // time taken off by a daily effect (e.g. being poisoned the day before). Applied here rather than
        // in the effect itself so Start can't overwrite it, whichever script starts first.
        if (penaltySeconds > 0)
        {
            timeInSeconds -= penaltySeconds;
            penaltySeconds = 0;
        }

        timeInSeconds -= Time.deltaTime;

        if (timeInSeconds < 0)
            timeInSeconds = 0;

        timeBar.value = timeInSeconds / startingTime;

        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        timerText.text = $"{minutes}:{seconds:00}";

        if (timeInSeconds <= 0)
        {
            // no extra penalty here: the day review decides hearts lost, same as submitting
            timesup.SetActive(true);
            StartCoroutine(WaitFiveSeconds());
            enabled = false;
        }
    }

    public void stopworking()
    {
        enabled = false;
    }

    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(5f);

        player.transform.position = new Vector3(29, 476, 147);
        timesup.SetActive(false);

        // open the day review the same way the Submit button does, so the collected list is filled in
        FindAnyObjectByType<ButtonManager>().ClickSubmit();
    }

    public static void ReduceRoundTime()
    {
        startingTime -= 60f;

        if (startingTime < 60f)
            startingTime = 60f;
    }

    public static void ResetRoundTime()
    {
        startingTime = 240f;
    }
}