using System;
using UnityEngine;

public class ChipShot : MonoBehaviour
{
    private void Awake()
    {
        MeshRenderer _renderer = GetComponent<MeshRenderer>();
        _renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        {
            HoleDetection._chipShotTime = Time.time;
            PlayerObject _player = other.GetComponentInParent<PlayerObject>();
            HoleDetection._chipShotID = _player._playerID;
        }
    }
}
