using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class Programmer : MonoBehaviour, IEventDisposable, IHurtable
{
    public event Action OnActionFinished = delegate { };

    public event Action<Vector3> OnMovingStarted = delegate { };
    public event Action<Vector3> OnMovingEnded = delegate { };

    public event Action OnSkillStarted = delegate { };
    public event Action OnSkillEnded = delegate { };

    public event Action<GameObject> OnMouseClicked = delegate { };

    public event Action<int> OnDamaged = delegate { };
    public event Action OnDeath = delegate { };

    public ProgrammerStatus Status
    {
        get; private set;
    }

    public ProgrammerAbility Ability
    {
        get; private set;
    }

    public void SetHealth(int newHealth)
    {
        Status.Health = newHealth;
    }

    public void Heal(int increasingHealth)
    {
        Status.Health += increasingHealth;
        Status.Health = Mathf.Clamp(Status.Health, 0, Status.FullHealth);

        CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 {1}의 체력을 회복함. 현재 체력은 {2}.", name, increasingHealth, Status.Health);
    }

    public void Hurt(int damage)
    {
        if (damage < 0)
        {
            DebugLogger.LogWarningFormat("Programmer::Hurt => {0} 프로그래머가 음수의 데미지를 입음.", name);
        }

        Status.Health -= damage;
        OnDamaged(damage);

        CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 {1}의 데미지를 입음. 남은 체력은 {2}.", name, damage, Status.Health);

        if (Status.Health <= 0)
        {
            CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 사망함!", name);
            OnDeath();
        }
    }

    private void Awake()
    {
        Status = new ProgrammerStatus
        {
            FullHealth = 100,
            Health = 4
        };

        Ability = new ProgrammerAbility();

        OnMovingStarted += Rotate;
    }

    private void Rotate(Vector3 direction)
    {
        CommonLogger.LogFormat("Programmer::Rotate => 프로그래머가 회전 명령을 받음. RotateDirection = {0}", direction);

        var normalizedDirection = direction.normalized;
        var newRotation = Quaternion.LookRotation(normalizedDirection);
        
        transform.rotation = newRotation;

        CommonLogger.Log("Programmer::Rotate => 프로그래머가 회전이 완료됨.");
    }

    private void OnMouseDown()
    {
        OnMouseClicked(this.gameObject);
    }

    public void Move(Vector3 deltaPosition)
    {
        CommonLogger.LogFormat("Programmer::Move => 프로그래머가 이동 명령을 받음. DeltaPosition = {0}", deltaPosition);
        StopCoroutine("StartMove");

        StartCoroutine(StartMove(deltaPosition));
    }

    private IEnumerator StartMove(Vector3 deltaPosition)
    {
        var rotationBeforeMoving = transform.rotation;
        
        OnMovingStarted(deltaPosition);

        yield return Translate(deltaPosition);

        transform.rotation = rotationBeforeMoving;

        CommonLogger.Log("Programmer::Move => 프로그래머의 이동이 종료됨.");

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
        CommonLogger.Log("Programmer::UseSkill => 프로그래머의 스킬 사용이 시작됨.");
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

        CommonLogger.Log("Programmer::UseSkill => 프로그래머의 스킬 사용이 끝남.");

        OnSkillEnded();
        OnActionFinished();
    }

    public void DisposeRegisteredEvents()
    {
        OnActionFinished = delegate { };
        OnMovingEnded = delegate { };
        OnSkillStarted = delegate { };
        OnSkillEnded = delegate { };
        OnMouseClicked = delegate { };
        OnDeath = delegate { };
        OnDamaged = delegate { };

        Status.DisposeRegisteredEvents();

        OnMovingStarted = Rotate;
    }
}