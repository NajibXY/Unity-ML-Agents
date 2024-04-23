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

    //// Learning data
    [SerializeField] private TrackCheckPoints trackCheckpoints;

    private Transform initialPosition;

    private void Awake() {
        initialPosition = transform;
        Debug.Log(initialPosition.position.x);
        Debug.Log(initialPosition.position.y);
    }

    private void FixedUpdate() {
        //GetInput();
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
            currentbreakForce = breakForce * 2;
        } else {
            currentbreakForce = 0f;
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        //todo enhance breaking
        currentbreakForce = isBreaking ? (breakForce * 3) : currentbreakForce;
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

    //private void OnTriggerEnter(Collider other) {
    //    if (other.TryGetComponent<FencePanel>(out _)) {
    //        //        //SetReward(1f);
    //        //        //floorMeshRenderer.material = winMaterial;
    //        //        //EndEpisode();

    //        //        //transform.localPosition = new Vector3(768.30f, 4.99f, 397.386f);
    //        //        //transform.localRotation = new Quaternion(0, 175.566f, 0, 0);

    //        //        //currentSteerAngle = 0;
    //        //        //currentbreakForce = 0;
    //        //        //frontRightWheelCollider.brakeTorque = 20000;
    //        //        //frontLeftWheelCollider.brakeTorque = 20000;
    //        //        //rearLeftWheelCollider.brakeTorque = 20000;
    //        //        //rearRightWheelCollider.brakeTorque = 20000;
    //        //        //frontLeftWheelCollider.motorTorque = 0;
    //        //        //frontRightWheelCollider.motorTorque = 0;
    //        //        //HandleMotor();
    //        //        //HandleSteering();
    //        //        //UpdateWheels();
    //        //        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //        //        AddReward(-1f);
    //        //        //todo commentate ?
    //        //        EndEpisode();
    //        Debug.Log("Enter trigger fence");
    //        AddReward(-0.5f);
    //    }
    //}


    // Learning Functions

    public override void OnEpisodeBegin() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //transform.position = initialPosition.position + new Vector3(UnityEngine.Random.Range(-2f, +2f), 0, UnityEngine.Random.Range(-2f, 0f));
        transform.forward = initialPosition.forward;
        transform.rotation = initialPosition.rotation;
        transform.position = new Vector3(768.3075f, 4.999092f, 397.386f);
        Debug.Log("Episode Begin");
        Debug.Log(transform.position.x);
        Debug.Log(transform.position.y);
        Debug.Log(transform.position.z);
        trackCheckpoints.ResetNextCheckpoint(transform);
        //stop the car
        StopCar();
    }

    public void StopCar() {
        // Steel Angle reset
        currentSteerAngle = 0;
        frontLeftWheelCollider.steerAngle = 0;
        frontRightWheelCollider.steerAngle = 0;
        // Break Force reset
        currentbreakForce = 0;
        ApplyBreaking();
        // Motor Force reset
        frontLeftWheelCollider.motorTorque = 0;
        frontRightWheelCollider.motorTorque = 0;
        rearLeftWheelCollider.motorTorque = 0;
        rearRightWheelCollider.motorTorque = 0;
        // Rotation Speed reset
        frontRightWheelCollider.rotationSpeed = 0;
        frontLeftWheelCollider.rotationSpeed = 0;
        rearLeftWheelCollider.rotationSpeed = 0;
        rearRightWheelCollider.rotationSpeed = 0;
        UpdateWheels();
    }

    public override void CollectObservations(VectorSensor sensor) {
        Vector3 checkPointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkPointForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch (actions.DiscreteActions[0]) {
            case 0:
                forwardAmount = 0f;
                break;
            case 1:
                forwardAmount = +1f;
                break;
            case 2:
                forwardAmount = -1f;
                break;
        }

        switch (actions.DiscreteActions[1]) {
            case 0:
                turnAmount = 0f;
                break;
            case 1:
                turnAmount = +1f;
                break;
            case 2:
                turnAmount = -1f;
                break;
        }

        SetInputs(forwardAmount, turnAmount);

        //bool isBreaking = false;
        //switch (actions.DiscreteActions[2]) {
        //    case 0:
        //        isBreaking = false;
        //        break;
        //    case 1:
        //        isBreaking = true;
        //        break;
        //}
        //SetInputs(forwardAmount, turnAmount, isBreaking);
    }

    public void SetInputs(float forwardAmount, float turnAmount) {
        horizontalInput = turnAmount;

        // Acceleration Input
        verticalInput = forwardAmount;

        //// Breaking Input
        //isBreaking = isItBreaking;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {

        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow)) {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            forwardAction = 2;
        }

        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) {
            turnAction = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            turnAction = 2;
        }

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
        //discreteActions[2] = 0;
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.TryGetComponent<FencePanel>(out _)) {
            AddReward(-1f);
            Debug.Log("Enter fence");
            //EndEpisode();
        }
        else if (collision.gameObject.TryGetComponent<Terrain>(out _)) {
            transform.forward = initialPosition.forward;
            transform.rotation = initialPosition.rotation;
            transform.position = new Vector3(768.3075f, 4.999092f, 397.386f);
            EndEpisode();
        }
    }

    private void OnCollisionStay(Collision collision) {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.TryGetComponent<FencePanel>(out _)) {
            Debug.Log("Stay fence");
            AddReward(-0.2f);
        }
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