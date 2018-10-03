using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class Programmer : MonoBehaviour, IEventDisposable
{
    public event Action OnActionFinished = delegate { };

    public event Action<Vector3> OnMovingStarted = delegate { };
    public event Action<Vector3> OnMovingEnded = delegate { };

    public event Action OnSkillStarted = delegate { };
    public event Action OnSkillEnded = delegate { };

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
        
        OnMovingStarted(deltaPosition);

        yield return Translate(deltaPosition);

        transform.rotation = rotationBeforeMoving;

        OnMovingEnded(transform.position);
        OnActionFinished();
    }

    private IEnumerator Translate(Vector3 deltaPosition)
    {
        var sourcePosition = transform.position;
        var destinationPosition = transform.position + deltaPosition;
        float interpolationValue = 0.0f;

        while (transform.position != destinationPosition)
        {
            yield return new WaitForEndOfFrame();

            interpolationValue += Time.deltaTime;

            var interpolatedPosition = Vector3.Lerp(sourcePosition, destinationPosition, interpolationValue);
            transform.position = interpolatedPosition;
        }
    }

    public void UseSkill()
    {
        StartCoroutine(StartUseSkill());
    }

    [SerializeField]
    private ParticleSystem testEffect;
    [SerializeField]
    private GameObject skillSpellPositionObject;

    private IEnumerator StartUseSkill()
    {
        OnSkillStarted();

        yield return new WaitForSeconds(0.5f);

        var particle = Instantiate(testEffect, skillSpellPositionObject.transform);
        particle.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        Destroy(particle.gameObject);
        
        OnSkillEnded();
        OnActionFinished();
    }

    public void DisposeRegisteredEvents()
    {
        OnActionFinished = delegate { };
        OnMovingStarted = delegate { };
        OnMovingEnded = delegate { };
        OnSkillStarted = delegate { };
        OnSkillEnded = delegate { };
    }
}
