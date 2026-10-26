using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{

    public Image TransitionScreen;
    private int Index;
    private float Incriments;
    private float Duration;

    private float LowerBound;
    private float UpperBound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Index = 0;
        Incriments = 10f;
        Duration = 0.8f;
        TransitionScene("In");
    }

    public void TransitionScene(string transitionType)
    {
        Index = 0;
        TransitionScreen.enabled = true;
        // bounds are fractions of the screen height (0 = covering, 1 = fully hidden above)
        if (transitionType == "In")
        {
            LowerBound = 0f;
            UpperBound = 1f;
        }
        else
        {
            LowerBound = 1f;
            UpperBound = 0f;
        }
        StartCoroutine("Transition");
    }

    IEnumerator Transition()
    {
        if (Incriments >= Index)
        {
            RectTransform rectTransform = TransitionScreen.GetComponent<RectTransform>();
            // read the height every step, the screen size can change (fullscreen, window resize)
            float screenHeight = ((RectTransform)rectTransform.parent).rect.height;
            float yPos = Mathf.Lerp(LowerBound, UpperBound, Index / Incriments) * screenHeight;
            Vector2 currentPos = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(currentPos.x, yPos);
            yield return new WaitForSeconds(Duration / Incriments);
            StartCoroutine("Transition");
        }
        else
        {
            // once slid away, turn the screen off so it can never peek in at a different resolution
            if (UpperBound > LowerBound)
                TransitionScreen.enabled = false;
            yield return new WaitForSeconds(0);
        }
        Index++;
    }

    public float getDuration()
    {
        return Duration;
    }


}
