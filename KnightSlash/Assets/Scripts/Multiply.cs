using System;
using UnityEngine;

public class Multiply : MonoBehaviour
{
    public GameObject ballistaObject;
    public GameObject disableObject;
    public Collider collider;

    private void Start()
    {
        ballistaObject.SetActive(false);
    }

    public void BuyBallista()
    {
        ballistaObject.SetActive(true);
        disableObject.SetActive(false);
        collider.enabled = false;
    }
}
