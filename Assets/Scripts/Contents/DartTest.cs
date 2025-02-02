using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DartTest : MonoBehaviour
{
    [SerializeField]
    private Transform dartTransform;

    [SerializeField]
    private float maxWaitPositionY;
    [SerializeField]
    private float minWaitPositionY;

    [SerializeField]
    private float maxPower;

    [SerializeField]
    private float minPower;

    [SerializeField]
    private float Destination;

    private Vector2 shootDirection;
    private Vector2 startPosition;

    [SerializeField]
    private Vector2 prevPosition;

    [SerializeField]
    private float maxScale = 3f;

    [SerializeField]
    private Transform end;
    [SerializeField]
    private RotationCirlce rotationCirlce;

    private float scale = 0f;

    private bool isShoot = false;

    private float currentTouchTime = 0f;
    private float currentPower = 0f;
    private float yawPower = 0f;

    private bool isDown = false;

    private Vector3 localScale;

    void Start()
    {
    }

    void Update()
    {
        if (isShoot)
        {
            Vector3 movePosition = shootDirection * (currentPower * Time.deltaTime);
            dartTransform.transform.position += movePosition;

            if (dartTransform.transform.position.y >= Destination)
            {
                dartTransform.transform.parent = end;
                rotationCirlce.OnEndRotation();
                enabled = false;
            }
        }
        else
        {
            if (MultiTouchManager.Instance.IsTouchPress)
            {
                if (prevPosition.y > dartTransform.position.y)
                    prevPosition = dartTransform.position;
                currentTouchTime += Time.deltaTime;

                var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                position.y = Mathf.Clamp(position.y, minWaitPositionY, maxWaitPositionY);
                position.z = 0f;
                dartTransform.position = position;
            }
        }

        if (MultiTouchManager.Instance.IsTouchEnd)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        isShoot = true;
        shootDirection = ((Vector2)dartTransform.position - prevPosition).normalized;

        if(shootDirection.y < 0f)
        {
            shootDirection.y = Mathf.Abs(shootDirection.y);
        }

        currentPower = Mathf.Clamp(currentTouchTime * 5f, minPower, maxPower);
        yawPower = currentPower;
    }
}
