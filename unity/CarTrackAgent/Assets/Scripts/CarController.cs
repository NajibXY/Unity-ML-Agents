using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarController : Agent {
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private Transform spawnPosition;
    [SerializeField] private TrackCheckPoints trackCheckpoints;

    private void FixedUpdate() {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput() {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor() {
        if (verticalInput == 0) {
            currentbreakForce = breakForce*2;
        } else {
            currentbreakForce = 0f;
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        //todo enhance breaking
        currentbreakForce = isBreaking ? (breakForce*3) : currentbreakForce;
        ApplyBreaking();
    }

    private void ApplyBreaking() {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering() {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels() {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<FencePanel>(out _)) {
            //SetReward(1f);
            //floorMeshRenderer.material = winMaterial;
            //EndEpisode();

            //transform.localPosition = new Vector3(768.30f, 4.99f, 397.386f);
            //transform.localRotation = new Quaternion(0, 175.566f, 0, 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            //currentSteerAngle = 0;
            //currentbreakForce = 0;
            //frontRightWheelCollider.brakeTorque = 20000;
            //frontLeftWheelCollider.brakeTorque = 20000;
            //rearLeftWheelCollider.brakeTorque = 20000;
            //rearRightWheelCollider.brakeTorque = 20000;
            //frontLeftWheelCollider.motorTorque = 0;
            //frontRightWheelCollider.motorTorque = 0;
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        } 
    }

    public void SetInputs(float forwardAmount, float turnAmount) {
        currentSteerAngle = maxSteerAngle * turnAmount;
        frontLeftWheelCollider.motorTorque = forwardAmount * motorForce;
        frontRightWheelCollider.motorTorque = forwardAmount * motorForce;
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Unity.MLAgents;
//using Unity.MLAgents.Actuators;
//using Unity.MLAgents.Sensors;
//public class NewBehaviourScript : Agent {

//    [SerializeField] private Transform targetTransform;
//    [SerializeField] private Material winMaterial;
//    [SerializeField] private Material loseMaterial;
//    [SerializeField] private MeshRenderer floorMeshRenderer;
//    public override void OnActionReceived(ActionBuffers actions) {
//        float moveX = actions.ContinuousActions[0];
//        float moveZ = actions.ContinuousActions[1];

//        //Vector3 oldLocalPosition = transform.localPosition;
//        transform.localPosition += 10f * Time.deltaTime * new Vector3(moveX, 0, moveZ);

//        //if ((targetTransform.localPosition - transform.localPosition).sqrMagnitude < (targetTransform.localPosition - oldLocalPosition).sqrMagnitude) {
//        //    AddReward(0.1f);
//        //} else {
//        //    AddReward(-0.1f);
//        //}
//    }

//    public override void CollectObservations(VectorSensor sensor) {
//        sensor.AddObservation(transform.localPosition);
//        sensor.AddObservation(targetTransform.localPosition);
//    }

//    public override void OnEpisodeBegin() {
//        transform.localPosition = new Vector3(Random.Range(-2.5f, 0f), 0, Random.Range(-2f, 2f));
//        targetTransform.localPosition = new Vector3(Random.Range(1.5f, 5f), 0, Random.Range(-2f, 2f));
//    }

//    public override void Heuristic(in ActionBuffers actionsOut) {
//        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
//        continuousActions[0] = Input.GetAxisRaw("Horizontal");
//        continuousActions[1] = Input.GetAxisRaw("Vertical");
//    }

//    private void OnTriggerEnter(Collider other) {
//        if (other.TryGetComponent<Goal>(out _)) {
//            SetReward(1f);
//            floorMeshRenderer.material = winMaterial;
//            EndEpisode();
//        } else if (other.TryGetComponent<Wall>(out _)) {
//            SetReward(-1f);
//            floorMeshRenderer.material = loseMaterial;
//            EndEpisode();
//        }
//    }

//}