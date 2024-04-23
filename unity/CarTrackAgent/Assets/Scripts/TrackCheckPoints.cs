using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour {

    [SerializeField] private List<Transform> carTransformList;
    private List<CheckPoint> checkPoints;  
    private List<int> nextCheckPointIndexList;

    private void Awake() {
        checkPoints = new List<CheckPoint>();
        // Initialize CPs
        Transform checkpointsTransform = transform.Find("CheckPoints");
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckPoint cp = checkpointSingleTransform.GetComponent<CheckPoint>();
            cp.SetTrackCheckPoints(this);
            checkPoints.Add(cp);
        }
        // Initialize Cars
        nextCheckPointIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList) {
            nextCheckPointIndexList.Add(0);
        }
    }

    public void CarReachedCheckpoint(CheckPoint cp, Transform carTransform) {
        int nextCheckPointIndex = nextCheckPointIndexList[carTransformList.IndexOf(carTransform)];
        if (checkPoints.IndexOf(cp) == nextCheckPointIndex) {
            Debug.Log("Correct checkpoint reached");
            //todo Good reward
            carTransform.gameObject.GetComponent<CarController>().AddReward(2f);
            nextCheckPointIndexList[carTransformList.IndexOf(carTransform)] = nextCheckPointIndex + 1;
        } else {
            //todo Penalize reward
            carTransform.gameObject.GetComponent<CarController>().AddReward(-2f);
            Debug.Log("Incorrect checkpoint reached");
        }
        // Reset
        if (nextCheckPointIndex.CompareTo(checkPoints.Count) == 0) {
            Debug.Log("Lap completed");
            //todo Success episode
            carTransform.gameObject.GetComponent<CarController>().AddReward(20);
            nextCheckPointIndexList[carTransformList.IndexOf(carTransform)] =  0;
        }
    }

    public void ResetNextCheckpoint(Transform carTransform) {
        nextCheckPointIndexList[carTransformList.IndexOf(carTransform)] = 0;
    }

    public CheckPoint GetNextCheckpoint(Transform carTransform) {
        // Reset if necessary
        int nextCheckPointIndex = nextCheckPointIndexList[carTransformList.IndexOf(carTransform)];
        if (nextCheckPointIndex.CompareTo(checkPoints.Count) == 0) {
            nextCheckPointIndexList[carTransformList.IndexOf(carTransform)] = 0;
        }
        return checkPoints[nextCheckPointIndexList[carTransformList.IndexOf(carTransform)]];
    }
}
