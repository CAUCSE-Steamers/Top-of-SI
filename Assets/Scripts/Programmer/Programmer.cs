using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Model;
using System.Xml.Linq;

public class Programmer : MonoBehaviour, IEventDisposable, IHurtable, IDeburf, IXmlConvertible, IXmlStateRecoverable
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
        get; set;
    }

    public ProgrammerAbility Ability
    {
        get; set;
    }
    
    public void SetHealth(int newHealth)
    {
        Status.Health = newHealth;
    }

    public void Heal(int increasingHealth)
    {
        Status.Health += (int)(increasingHealth * (1 + Status.HealRate));
        Status.Health = Mathf.Clamp(Status.Health, 0, Status.FullHealth);

        CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 {1}의 체력을 회복함. 현재 체력은 {2}.", name, increasingHealth, Status.Health);
    }

    public void Hurt(int damage)
    {
        if (damage < 0)
        {
            DebugLogger.LogWarningFormat("Programmer::Hurt => {0} 프로그래머가 음수의 데미지를 입음.", name);
        }

        int totalDamage = (int) (damage * (1 + Status.AdditionalDamageRatio));

        Status.Health -= totalDamage;
        OnDamaged(damage);

        CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 {1}의 데미지를 입음. 남은 체력은 {2}.", name, totalDamage, Status.Health);

        if (IsAlive == false)
        {
            CommonLogger.LogFormat("Programmer::Hurt => {0} 프로그래머가 사망함!", name);
            OnDeath();
        }
    }

    public bool IsAlive
    {
        get
        {
            return Status.Health > 0;
        }
    }

    private void Awake()
    {
        Status = new ProgrammerStatus
        {
            PortraitName = "UnityChan",
            FullHealth = 1000,
            Health = 1000,
            Name = "테스트 보스"
        };
        
        Ability = new ProgrammerAbility();

        RegisterBurf(new HealBurf(10, 50) { RemainingTurn = 3 });
        RegisterBurf(new HurtDamageBurf(10.0) { RemainingTurn = 4 });
        RegisterBurf(new NormalAttackDamageBurf(29.0) { RemainingTurn = 5 });
        RegisterBurf(new SkillDamageBurf(59.0) { RemainingTurn = 5 });

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
        if (IsAlive)
        {
            OnMouseClicked(this.gameObject);
        }
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

    public void GoVacation(int elapsedDays)
    {
        if (Status.IsOnVacation)
        {
            DebugLogger.LogWarningFormat("Programmer::GoVacation => 프로그래머 '{0}'는 이미 휴가를 떠난 상태지만, 또 휴가를 떠나려고 합니다.", name);
        }

        Status.StartVacationDay = elapsedDays;
        CommonLogger.LogFormat("Programmer::GoVacation => 프로그래머 '{0}'가 {1}일 째에 휴가를 떠납니다.", name, elapsedDays);

        OnActionFinished();
    }

    public void ReturnFromVacation(int elapsedDays)
    {
        if (Status.IsOnVacation == false)
        {
            DebugLogger.LogWarningFormat("Programmer::GoVacation => 프로그래머 '{0}'는 휴가를 떠나지 않은 상태에서 복귀하려고 합니다.", name);
        }

        int deltaDays = (elapsedDays - Status.StartVacationDay).Value;
        int healQuantity = (int)((Status.FullHealth * (0.05 * deltaDays * deltaDays)));

        Heal(healQuantity);
        Status.StartVacationDay = null;

        CommonLogger.LogFormat("Programmer::GoVacation => 프로그래머 '{0}'가 {1}일 째에 휴가에서 복귀합니다.", name, elapsedDays);

        OnActionFinished();
    }
    
    public void ApplyPersistentStatusBurfs()
    {
        foreach (var statusBurf in Status.Burfs.Where(burf => burf.IsPersistent)
                                               .OfType<IStatusModificationCommand>())
        {
            statusBurf.Modify(Status);
        }
    }

    public void RegisterBurf(IBurf newBurf)
    {
        Status.AddBurf(newBurf);

        if (newBurf.IsPersistent == false)
        {
            ApplyStatusBurf(newBurf as IStatusModificationCommand);
        }

        ApplySkillBurf(newBurf as ISkillModificationCommand);
    }

    public void DecayBurfs()
    {
        var expiredBurfs = Status.DecayBurfAndFetchExpiredBurfs();
        foreach (var expiredBurf in expiredBurfs)
        {
            UnapplyStatusBurf(expiredBurf as IStatusModificationCommand);
            UnapplySkillBurf(expiredBurf as ISkillModificationCommand);
        }
    }

    private void ApplyStatusBurf(IStatusModificationCommand statusBurf)
    {
        if (statusBurf != null)
        {
            statusBurf.Modify(Status);
        }
    }

    private void ApplySkillBurf(ISkillModificationCommand skillBurf)
    {
        if (skillBurf != null)
        {
            foreach (var activeSkill in Ability.AcquiredActiveSkills)
            {
                skillBurf.Modify(activeSkill);
            }
        }
    }

    private void UnapplyStatusBurf(IStatusModificationCommand statusBurf)
    {
        if (statusBurf != null)
        {
            statusBurf.Unmodify(Status);
        }
    }

    private void UnapplySkillBurf(ISkillModificationCommand skillBurf)
    {
        if (skillBurf != null)
        {
            foreach (var activeSkill in Ability.AcquiredActiveSkills)
            {
                skillBurf.Unmodify(activeSkill);
            }
        }
    }

    public DeburfType Deburf(List<DeBurfStructure> deburf)
    {
        DeburfType ret = DeburfType.None;
        foreach (var iter in deburf)
        {
            if(OutOfProgrammer(iter.Type))
            {
                ret |= iter.Type;
            }
            else
            {
                Status.Deburf.Add(iter);
            }
        }
        return ret;
    }

    private bool OutOfProgrammer(DeburfType type)
    {
        //If new Deburf type which need to control out of programmer, add it to this.
        return ((type & DeburfType.ShortenDeadLine) != 0);
    }

    public XElement ToXmlElement()
    {
        return new XElement("Programmer", Status.ToXmlElement(),
                                          Ability.ToXmlElement());
    }

    public void RecoverStateFromXml(string rawXml)
    {
        var element = XElement.Parse(rawXml);

        Status.RecoverStateFromXml(element.Element("Status").ToString());
        Ability.RecoverStateFromXml(element.Element("Ability").ToString());
    }
}