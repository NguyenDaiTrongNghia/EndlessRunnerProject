using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public void GameOver()
    {
        SetGamePause(true);
    }
    public void SetGamePause(bool bIsPaused)
    {
        if (bIsPaused)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
