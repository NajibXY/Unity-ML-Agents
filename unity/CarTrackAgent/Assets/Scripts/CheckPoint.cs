using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    private TrackCheckPoints trackCheckpoints;
    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<CarController>(out CarController carController)) {
            trackCheckpoints.CheckpointReached(this);
        }
    }

    public void SetTrackCheckPoints(TrackCheckPoints trackCheckpoints) {
        this.trackCheckpoints = trackCheckpoints;
    }
}
