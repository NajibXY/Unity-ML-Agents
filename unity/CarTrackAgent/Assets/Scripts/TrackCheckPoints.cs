using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{
    private void Awake() {
        Transform checkpointsTransform = transform.Find("CheckPoints");
        Debug.Log(checkpointsTransform);
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            Debug.Log(checkpointSingleTransform);
        }
    }
}
