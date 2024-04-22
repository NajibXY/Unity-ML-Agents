using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour {
    private void Awake() {
        Transform checkpointsTransform = transform.Find("CheckPoints");
        Debug.Log(checkpointsTransform);
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckPoint cp = checkpointSingleTransform.GetComponent<CheckPoint>();
            cp.SetTrackCheckPoints(this);
        }
    }

    public void CheckpointReached(CheckPoint cp) {
        Debug.Log(cp.transform.name);
    }
}
