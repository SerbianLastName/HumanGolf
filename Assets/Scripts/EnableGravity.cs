using UnityEngine;

public class EnableGravity : MonoBehaviour
{
   [SerializeField] private Rigidbody rb;
   [SerializeField] private Collider col;
   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         col.isTrigger = false;
         rb.isKinematic = false;
         rb.useGravity = true;
      }
   }
}
