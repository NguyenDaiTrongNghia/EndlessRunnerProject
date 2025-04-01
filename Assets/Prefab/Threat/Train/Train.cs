//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] TrainSegment segmentPrefab;
    [SerializeField] Vector2 SegmentCountRange;
    [SerializeField] Threat threat;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTrainBody();
    }

    private void GenerateTrainBody()
    {
        int BodyCount = Random.Range((int)SegmentCountRange.x, (int)SegmentCountRange.y);
        for(int i = 0; i < BodyCount; i++)
        {
            Vector3 spawnPos = transform.position + transform.forward * segmentPrefab.GetSegmentLength() * i;
            TrainSegment newSegment = Instantiate(segmentPrefab, spawnPos, Quaternion.identity);
            if(i == 0)
            {
                newSegment.SetHead();
            }
            newSegment.GetMovementComponent().CopyFrom(threat.GetMovementComponent());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
