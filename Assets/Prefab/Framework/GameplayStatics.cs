using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayStatics
{
    static GameMode gameMode;
    public static bool IsPositionOccupied(Vector3 position, Vector3 DetectionHalfExtend, string OccupationcheckTag)
    {
        Collider[] cols = Physics.OverlapBox(position,DetectionHalfExtend, Quaternion.identity);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == OccupationcheckTag || col.gameObject.tag == "NoSpawn")
            {
                return true;
            }
        }
        return false;
    }

    public static GameMode GetGameMode()
    {
        if(gameMode == null)
        {
            gameMode = GameObject.FindObjectOfType<GameMode>();
        }
        return gameMode;
    }
}
