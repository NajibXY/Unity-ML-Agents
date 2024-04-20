using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class NewBehaviourScript : Agent {

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        //Vector3 oldLocalPosition = transform.localPosition;
        transform.localPosition += 10f * Time.deltaTime * new Vector3(moveX, 0, moveZ);
        
        //if ((targetTransform.localPosition - transform.localPosition).sqrMagnitude < (targetTransform.localPosition - oldLocalPosition).sqrMagnitude) {
        //    AddReward(0.1f);
        //} else {
        //    AddReward(-0.1f);
        //}
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(Random.Range(-2.5f, 0f), 0, Random.Range(-2f, 2f));
        targetTransform.localPosition = new Vector3(Random.Range(1.5f, 5f), 0, Random.Range(-2f, 2f));
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Goal> (out _)) {
            SetReward(1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        } else if (other.TryGetComponent<Wall> (out _)) {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
    }

}

