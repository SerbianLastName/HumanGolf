using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming) return;
        if (GameManager.activePlayerState == ActivePlayerState.InHole) return;
        if (collision.collider.CompareTag("Player")) return;
        GameManager.UpdateActivePlayerState(ActivePlayerState.OnGround);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming) return;
        if (GameManager.activePlayerState == ActivePlayerState.InHole) return;
        if (collision.collider.CompareTag("Player")) return;
        GameManager.UpdateActivePlayerState(ActivePlayerState.OnGround);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (GameManager.activePlayerState == ActivePlayerState.Aiming) return;
        if (GameManager.activePlayerState == ActivePlayerState.InHole) return;
        if (collision.collider.CompareTag("Player")) return;
        GameManager.UpdateActivePlayerState(ActivePlayerState.MidAir);
    }
}
