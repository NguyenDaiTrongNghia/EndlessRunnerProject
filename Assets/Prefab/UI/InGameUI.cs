using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ScoreKeeper scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(scoreKeeper != null)
        {
            scoreKeeper.onScoreChanged += UpdateScoreText;
        }
    }

    private void UpdateScoreText(int newVal)
    {
        ScoreText.SetText($"Coin: {newVal}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
