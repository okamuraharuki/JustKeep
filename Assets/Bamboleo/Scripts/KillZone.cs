using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Placed")
        {
            Debug.Log("Kill Obj");
            Destroy(other.gameObject);
        }
    }
}
