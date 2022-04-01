using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;


public class RacerCharacterController : MonoBehaviour, ICharacterController
{
    [SerializeField] float maxSpeed;
    [SerializeField] float turningSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float brakingDeceleration;
    [SerializeField] float groundDeceleration;
    [SerializeField] float reverseMaxSpeed;
    [SerializeField] float airDeceleration;
    [SerializeField] float gravity;
    [SerializeField] float jumpSpeed;
    [SerializeField] float maxFallSpeed;

    KinematicCharacterMotor Motor;

    Vector3 newRelativeVelocity;

    Vector2 directionInput;
    bool braking;
    bool jump1Requested;
    bool jump2Requested;

    private void Awake()
    {
        Motor = GetComponent<KinematicCharacterMotor>();
    }

    void Start()
    {
        Motor.CharacterController = this;
    }

    public void GetInputs(RacerInputs inputs)
    {
        directionInput = inputs.directionInput;
        braking = inputs.braking;

        if (Motor.GroundingStatus.IsStableOnGround)
        {
            if (inputs.jump1Triggered) jump1Requested = true;
            if (inputs.jump2Triggered) jump2Requested = true;
        }
    }

    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        newRelativeVelocity = transform.InverseTransformDirection(currentVelocity);

        if (Motor.GroundingStatus.IsStableOnGround)
        {
            if (!braking)
            {
                // Get relative velocity vector
                newRelativeVelocity.z = transform.InverseTransformDirection(currentVelocity).z + directionInput.y * acceleration * deltaTime;
                newRelativeVelocity.z = Mathf.Clamp(newRelativeVelocity.z, -Mathf.Abs(reverseMaxSpeed), maxSpeed);

                // Apply deceleration on ground
                if (Mathf.Abs(directionInput.y) <= float.Epsilon)
                {
                    newRelativeVelocity.z = transform.InverseTransformDirection(currentVelocity).z - groundDeceleration * deltaTime * Mathf.Sign(transform.InverseTransformDirection(currentVelocity).z);
                    if (Mathf.Abs(newRelativeVelocity.z) < 1f) newRelativeVelocity.z = 0;
                }
            }            
            //braking
            else
            {
                newRelativeVelocity.z = transform.InverseTransformDirection(currentVelocity).z - brakingDeceleration * deltaTime * Mathf.Sign(transform.InverseTransformDirection(currentVelocity).z);
                if (Mathf.Abs(newRelativeVelocity.z) < 1f) newRelativeVelocity.z = 0;
                Debug.Log("Brake");
            }

            // Apply Jump1
            if (jump1Requested)
            {
                //Debug.Log("Jumped");
                Motor.ForceUnground(0.1f);
                newRelativeVelocity.y += jumpSpeed;
                jump1Requested = false;
            }
        }
        else
        {
            //Gravity
            newRelativeVelocity.y +=  -Mathf.Abs(gravity) * deltaTime;
        }

        newRelativeVelocity = new Vector3(0, newRelativeVelocity.y, newRelativeVelocity.z);
        currentVelocity = transform.TransformDirection(newRelativeVelocity);
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
        float currentYAngle = currentRotation.eulerAngles.y + directionInput.x * turningSpeed * deltaTime;
        currentRotation = Quaternion.Euler(currentRotation.x, currentYAngle, currentRotation.z);
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return true;
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
    }

    public void PostGroundingUpdate(float deltaTime)
    {
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
    }   
}
