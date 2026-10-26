using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ScenePreloader scene;
    public GameObject quitButton;

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (quitButton != null)
            quitButton.SetActive(false);
#endif
    }

    public void ClickStart()
    {
        scene.ActivatePreloadedScene();
    }

    public void Quit()
    {
        Application.Quit(); 
        Debug.Log("Exitting");  
    }
}
