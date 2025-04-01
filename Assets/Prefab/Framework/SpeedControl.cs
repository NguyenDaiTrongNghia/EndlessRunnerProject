using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    public delegate void OnGlobalSpeedChanged(float newSpeed);
    [SerializeField] float GlobalSpeed = 15f;

    public event OnGlobalSpeedChanged onGlobalSpeedChanged;
    public void ChangeGlobalSpeed(float SpeedChange, float duration)
    {
        GlobalSpeed += SpeedChange;
        InformSpeedChange();
        StartCoroutine(RemoveSpeedChange(SpeedChange, duration));
    }

    public float GetGlobalSpeed()
    {
        return GlobalSpeed;
    }

  IEnumerator RemoveSpeedChange(float SpeedChangeAmt, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GlobalSpeed -= SpeedChangeAmt;
        InformSpeedChange();
    }

    private void InformSpeedChange()
    {
        onGlobalSpeedChanged?.Invoke(GlobalSpeed);
    }
}
