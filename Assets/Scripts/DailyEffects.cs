using UnityEngine;

public class DailyEffects : MonoBehaviour
{
    private float movementX;
    private float movementY;
    private bool reversed = true;  
    public Timer time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SpeedUp()
    {
        PlayerMovement.moveSpeed = 50;
    }

    public void SlowDown()
    {
       PlayerMovement.moveSpeed = 3; 
    }

    public void Shrink()
    {
        Timer.penaltySeconds += 60;
    }
    public void Enlargen()
    {
        Timer.timeInSeconds = Timer.timeInSeconds - 60;
    }

    public void RandomEffect()
    {
        {
            int x = Random.Range(2,3);

            switch (x)
            {
                case 0:
                    SpeedUp();
                    Debug.Log("Effect: speedup");
                    break;

                case 1:
                    SlowDown();
                    Debug.Log("Effect: slowdonw");

                    break;

                case 2:
                    Shrink();
                    Debug.Log("Effect: shrink");
                    break;

                case 3: 
                    Enlargen();
                    Debug.Log("Effect: enlarge");
                    break;
            }

        }
    }

}
