using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInventory : MonoBehaviour
{
    private GameObject[] foodItems;
    public int foodCount;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foodItems = GameObject.FindGameObjectsWithTag("Food");
        foodCount = foodItems.Length;
    }
}
