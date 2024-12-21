using System;
using UnityEngine;

public class GreenModifier : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerObject player = other.GetComponentInParent<PlayerObject>();
            player._inGreen = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerObject player = other.GetComponentInParent<PlayerObject>();
            player._inGreen = false;
        }
    }
}
