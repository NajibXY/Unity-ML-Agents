using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour {

    private List<CheckPoint> checkPoints;  
    private int nextCheckPointIndex;

    private void Awake() {
        nextCheckPointIndex = 0;
        checkPoints = new List<CheckPoint>();
        Transform checkpointsTransform = transform.Find("CheckPoints");
        Debug.Log(checkpointsTransform);
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckPoint cp = checkpointSingleTransform.GetComponent<CheckPoint>();
            cp.SetTrackCheckPoints(this);
            checkPoints.Add(cp);
        }
    }

    public void CheckpointReached(CheckPoint cp) {
        if (checkPoints.IndexOf(cp) == nextCheckPointIndex) {
            Debug.Log("Correct checkpoint reached");
            nextCheckPointIndex++;
        } else {
            //todo ? Penalize reward
            Debug.Log("Incorrect checkpoint reached");
        }
        // Reset
        if (nextCheckPointIndex.CompareTo(checkPoints.Count) == 0) {
            Debug.Log("Lap completed");
            nextCheckPointIndex = 0;
        }
    }
}
