using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickupSpawner : MonoBehaviour
{
    //support spawn area
    float radius;
    float minSpawnX, maxSpawnX, minSpawnY, maxSpawnY;

    Dictionary<string, GameObject> pickupPrefabs = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Load Prefabs of pickup
        foreach (PickupType type in Enum.GetValues(typeof(PickupType)))
        {
            string typeName = type.ToString();
            pickupPrefabs.Add(typeName, Resources.Load<GameObject>(typeName+"Pickup"));
            
        }

        //get radius of pickup
        GameObject tempPickup = Instantiate(pickupPrefabs[PickupType.Bonus.ToString()]);
        radius = tempPickup.GetComponent<CircleCollider2D>().radius;
        Destroy(tempPickup);

        //calculate spawn area
        minSpawnX = ScreenUtils.ScreenLeft + radius*2;
        maxSpawnX = ScreenUtils.ScreenRight - radius*2;
        minSpawnY = ScreenUtils.ScreenBottom + radius*2;
        maxSpawnY = ScreenUtils.ScreenTop - radius*2;


        EventManager.AddListener(EventName.SpawnPickupEvent, SpawnPickup);


        //randPickType = UnityEngine.Random.Range(0, 3);
        //SpawnPickup(randPickType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// spawn a pickup of pickuptype
    /// </summary>
    /// <param name="unuse">unuse</param>
    void SpawnPickup(int unuse)
    {
        PickupType pickuptype;

        //decise spawned location
        Vector3 location = new Vector3();
        location.x = UnityEngine.Random.Range(minSpawnX, maxSpawnX);
        location.y = UnityEngine.Random.Range(minSpawnY, maxSpawnY);

        int randPickType = (int)(UnityEngine.Random.value * 100);
        if (randPickType < DifficultyUtilities.DistractionProbability)
        {
            pickuptype = PickupType.Distraction;
        }
        else if (randPickType < DifficultyUtilities.DistractionProbability + DifficultyUtilities.BonusProbability)
        {
            pickuptype = PickupType.Bonus;
        }
        else
        {
            pickuptype = PickupType.SpeedDown; 
        }

        //test for even appear three type of pickup
        randPickType = UnityEngine.Random.Range(0, 3);
        pickuptype = (PickupType)randPickType;
        //pickuptype= Enum.GetName(typeof(PickupType), randPickType));

        //pickuptype = PickupType.SpeedDown;
        string pickupTypeName = pickuptype.ToString();//Enum.GetName(typeof(PickupType), randPickType);
        Instantiate(pickupPrefabs[pickupTypeName], location, Quaternion.identity);
    }

}
