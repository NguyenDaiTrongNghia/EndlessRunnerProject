using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArraySpawner : MonoBehaviour
{
    [SerializeField] int Amount = 10;
    [SerializeField] float Gap = 1f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i <= Amount; i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z) + transform.forward * Gap * i;
            GameObject nextCoin = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            nextCoin.GetComponent<ArraySpawner>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
