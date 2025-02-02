using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCirlce : MonoBehaviour
{
    [SerializeField]
    private Transform roationTarget;

    [SerializeField]
    private AnimationCurve rotationCurve;

    [SerializeField]
    private float maxRotationSpeed;

    [SerializeField]
    private float maxLerpTime;

    private float currentRotationTime = 0f;

    private bool isRotation = false;


    void Start()
    {
        OnStartRotation();
    }

    void Update()
    {
        if(isRotation)
        {
            currentRotationTime += Time.deltaTime;
            var rotationSpeed = rotationCurve.Evaluate(currentRotationTime/ maxLerpTime) * maxRotationSpeed;

            roationTarget.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
        }
    }

    public void OnStartRotation()
    {
        isRotation = true;
    }

    public void OnEndRotation()
    {
        isRotation = false;
    }
}
