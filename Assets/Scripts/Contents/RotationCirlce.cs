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
    private bool isStop = false;


    void Start()
    {
        OnStartRotation();
    }

    void Update()
    {
        if(isRotation)
        {
            if(isStop)
            {
                currentRotationTime -= Time.deltaTime;
                var rotationSpeed = rotationCurve.Evaluate(currentRotationTime/ maxLerpTime) * maxRotationSpeed;
                roationTarget.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);

                if (currentRotationTime < 0f)
                    isRotation = false;
            }
            else
            {
                currentRotationTime += Time.deltaTime;
                var rotationSpeed = rotationCurve.Evaluate(currentRotationTime/ maxLerpTime) * maxRotationSpeed;
                roationTarget.rotation *= Quaternion.Euler(Vector3.forward * rotationSpeed);
            }

        }
    }

    public void OnStartRotation()
    {
        isRotation = true;
    }

    public void OnEndRotation()
    {
        isStop = true;
        currentRotationTime = maxLerpTime;
    }
}
