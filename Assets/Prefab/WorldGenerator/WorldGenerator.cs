using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldGenerator : MonoBehaviour
{
    [Header("Road Blocks")]
    //[SerializeField] float EvnMoveSpeed = 4f;
    [SerializeField] Transform StartingPoint;
    [SerializeField] Transform EndPoint;
    [SerializeField] GameObject[] roadBlocks;

    [Header("Buildings")]
    [SerializeField] GameObject[] buildings;   
    [SerializeField] Transform[] buildingSpawnPoints;
    [SerializeField] Vector2 buildingSpawnScaleRange = new Vector2(0.6f, 0.8f);

    [Header("Street Lights")]
    [SerializeField] GameObject StreetLight;
    [SerializeField] Transform[] StreetLightSpawnPoints;

    [Header("Threats")]
    [SerializeField] Vector3 OccupationDetectionHalfExtend;
    [SerializeField] Threat[] Threats;
    [SerializeField] Transform[] Lanes;
    Vector3 MoveDirection;

    [Header("Pickups")]
    [SerializeField] Pickup[] pickups;
    bool GetRandomSpawnPoint(out Vector3 spawnPoint, string OccupationcheckTag)
    {
        Vector3[] spawnPoints = GetAvailableSpawnPoints(OccupationcheckTag);
        if(spawnPoints.Length == 0)
        {
            spawnPoint = new Vector3(0, 0, 0);
            return false;
        }

        int pick = Random.Range(0, spawnPoints.Length);
        spawnPoint = spawnPoints[pick];
        return true;

    }

    Vector3[] GetAvailableSpawnPoints(string OccupationcheckTag)
    {
        List<Vector3> AvailableSpawnPoints = new List<Vector3>();
        foreach(Transform spawnTrans in Lanes)
        {
            Vector3 spawnPoint = spawnTrans.position + new Vector3(0, 0, StartingPoint.position.z);
            if(!IsPositionOccupied(spawnPoint, OccupationcheckTag))
            {
                AvailableSpawnPoints.Add(spawnPoint);
            }
        }
        return AvailableSpawnPoints.ToArray();
    }

    bool IsPositionOccupied(Vector3 position, string OccupationcheckTag)
    {
        Collider[] cols = Physics.OverlapBox(position, OccupationDetectionHalfExtend, Quaternion.identity);
       foreach(Collider col in cols)
        {
            if(col.gameObject.tag == OccupationcheckTag)
            {
                return true;
            }
        }
        return false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 nextBlockPosition = StartingPoint.position;
        float EndPointDistance = Vector3.Distance(StartingPoint.position, EndPoint.position);
        MoveDirection = (EndPoint.position - StartingPoint.position).normalized;
        while (Vector3.Distance(StartingPoint.position, nextBlockPosition) < EndPointDistance)
        {
            GameObject newBlock = SpawnNewBlock(nextBlockPosition, MoveDirection);

            float blockLength = newBlock.GetComponent<Renderer>().bounds.size.z;
            nextBlockPosition += MoveDirection * blockLength;
        }

        StartSpawnElements();

        Pickup newPickup = Instantiate(pickups[0], StartingPoint.position, Quaternion.identity);
        newPickup.GetComponent<MovementComp>().SetDestination(EndPoint.position);
        newPickup.GetComponent<MovementComp>().SetMoveDirect(MoveDirection);
    }

    private void StartSpawnElements()
    {
        //loop through all the threats
            //start periodically spawn threat based on their spawn interval
            //this requires a coroutine
        foreach(Threat threat in Threats)
        {
            StartCoroutine(SpawnElement(threat));
        }
        foreach(Pickup pickup in pickups)
        {
            StartCoroutine(SpawnElement(pickup));
        }   
    }

    IEnumerator SpawnElement(Spawnable elementToSpawn)
    {
        while(true)
        {
            if(GetRandomSpawnPoint(out Vector3 spawnPoint, elementToSpawn.gameObject.tag))
            {
                Spawnable newThreat = Instantiate(elementToSpawn, spawnPoint, Quaternion.identity);

                newThreat.GetMovementComponent().SetDestination(EndPoint.position);
                newThreat.GetMovementComponent().SetMoveDirect(MoveDirection);
            }
         

            yield return new WaitForSeconds(elementToSpawn.SpawnInterval);
        }
    }

    GameObject SpawnNewBlock(Vector3 SpawnPosition, Vector3 MoveDir)
    {
        int pick = Random.Range(0, roadBlocks.Length);
        GameObject pickedBlock = roadBlocks[pick];
        GameObject newBlock = Instantiate(pickedBlock);
        newBlock.transform.position = SpawnPosition;
        MovementComp moveComp = newBlock.GetComponent<MovementComp>();
        if (moveComp != null)
        {
            //moveComp.SetMoveSpeed(EvnMoveSpeed);
            moveComp.SetDestination(EndPoint.position);
            moveComp.SetMoveDirect(MoveDir);
        }

        SpawnBuildings(newBlock);
        SpawnStreetLights(newBlock);

        return newBlock;
    }

    private void SpawnStreetLights(GameObject ParentBlock)
    {
        foreach (Transform StreetLightSpawnPoint in StreetLightSpawnPoints)
        {
            Vector3 SpawnLocation = ParentBlock.transform.position + (StreetLightSpawnPoint.position - StartingPoint.position);
            Quaternion SpawnRot = Quaternion.LookRotation((StartingPoint.position - StreetLightSpawnPoint.position).normalized, Vector3.up);
            Quaternion SpawnRotOffset = Quaternion.Euler(0, -90, 0);
            GameObject newStreetLight = Instantiate(StreetLight, SpawnLocation, SpawnRot * SpawnRotOffset, ParentBlock.transform);
        }
    }

    private void SpawnBuildings(GameObject ParentBlock)
    {
        foreach (Transform BuildingSpawnPoint in buildingSpawnPoints)
        {
            Vector3 BuildingSpawnLocation = ParentBlock.transform.position + (BuildingSpawnPoint.position - StartingPoint.position);
            int RotationOffsetBy90 = Random.Range(0, 3);
            Quaternion BuildingSpawnRotation = Quaternion.Euler(0, RotationOffsetBy90 * 90, 0);
            Vector3 BuildingSpawnSize = Vector3.one * Random.Range(buildingSpawnScaleRange.x, buildingSpawnScaleRange.y);
            int BuildingPick = Random.Range(0, buildings.Length);

            GameObject newBuilding = Instantiate(buildings[BuildingPick], BuildingSpawnLocation, BuildingSpawnRotation, ParentBlock.transform);
            newBuilding.transform.localScale = BuildingSpawnSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.gameObject!=null && other.gameObject.tag == "RoadBlock")
        {
            GameObject newBlock = SpawnNewBlock(other.transform.position, MoveDirection);
            float newBlockHalfLength = newBlock.GetComponent<Renderer>().bounds.size.z/2f;
            float previousBlockHalfLength = other.GetComponent<Renderer>().bounds.size.z/2f;

            Vector3 newBlockSpawnOffset = -(newBlockHalfLength + previousBlockHalfLength) * MoveDirection;
            newBlock.transform.position += newBlockSpawnOffset;
        }
    }
}
