using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Programmer : MonoBehaviour
{
    public event Action<Vector3> OnMovingStarted = delegate { };
    public event Action<Vector3> OnMovingEnded = delegate { };

    private void Start()
    {
        OnMovingStarted += Rotate;
    }

    private void Rotate(Vector3 direction)
    {
        var normalizedDirection = direction.normalized;
        var newRotation = Quaternion.LookRotation(normalizedDirection);
        
        transform.rotation = newRotation;
    }

    public void Move(Vector3 deltaPosition)
    {
        StopCoroutine("StartMove");

        StartCoroutine(StartMove(deltaPosition));
    }

    private IEnumerator StartMove(Vector3 deltaPosition)
    {
        var rotationBeforeMoving = transform.rotation;

        var sourcePosition = transform.position;
        var destinationPosition = transform.position + deltaPosition;

        float interpolationValue = 0.0f;

        OnMovingStarted(deltaPosition);

        while (transform.position != destinationPosition)
        {
            yield return new WaitForEndOfFrame();
            interpolationValue += Time.deltaTime;

            var interpolatedPosition = Vector3.Lerp(sourcePosition, destinationPosition, interpolationValue);
            transform.position = interpolatedPosition;
        }

        OnMovingEnded(transform.position);

        transform.rotation = rotationBeforeMoving;
    }
}
