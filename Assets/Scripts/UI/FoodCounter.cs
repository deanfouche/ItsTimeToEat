using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FoodCounter : MonoBehaviour
{
    public FoodInventory inventory;
    public GameObject counterDisplay;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Text foodCounter = counterDisplay.GetComponent<Text>();
        foodCounter.text = $"{inventory.foodCount}";
    }
}