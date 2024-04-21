using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<CarController>(out CarController carController)) {
            Debug.Log("CheckPoint !");
        }
    }
}
