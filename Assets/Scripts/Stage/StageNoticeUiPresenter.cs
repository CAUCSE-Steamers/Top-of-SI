using UnityEngine;
using System.Collections;
using Model;
using UnityEngine.UI;
using System.Text;
using System.Linq;

public class StageNoticeUiPresenter : MonoBehaviour
{
    [SerializeField]
    private Text leftBossSkillText;
    [SerializeField]
    private Text rightBossSkillText;

    private Text BossSkillText
    {
        get
        {
            return StageManager.Instance.Status.StageDirection == Direction.Left ?
                   leftBossSkillText : rightBossSkillText;
        }
    }

    public void SetBossSkillNoticeActiveState(bool newState)
    {
        BossSkillText.gameObject.SetActive(newState);
    }

    public void RenderBossSkillNotice(ProjectSkill skill)
    {
        SetBossSkillNoticeActiveState(true);

        if (skill is ProjectBurfSkill)
        {
            RenderBossSkillNotice(skill as ProjectBurfSkill);
        }
        else if (skill is ProjectMultiAttackSkill)
        {
            RenderBossSkillNotice(skill as ProjectMultiAttackSkill);
        }

        StartCoroutine(AutomaticTurnOffBossSkillNotice());
    }

    private IEnumerator AutomaticTurnOffBossSkillNotice()
    {
        yield return new WaitForSeconds(3.0f);
        SetBossSkillNoticeActiveState(false);
    }

    private void RenderBossSkillNotice(ProjectMultiAttackSkill multiAttackSkill)
    {
        var boss = StageManager.Instance.Unit.Boss;
        var noticeBuilder = new StringBuilder();

        BossSkillText.text = string.Format("프로젝트가 데미지 {0}의 전역 공격을 수행합니다!", (int) multiAttackSkill.Damage);
    }

    private void RenderBossSkillNotice(ProjectBurfSkill burfSkill)
    {
        var boss = StageManager.Instance.Unit.Boss;
        var noticeBuilder = new StringBuilder();

        // TODO: Other notice?
        foreach (var burf in burfSkill.Burf)
        {
            switch (burf.Type)
            {
                case BurfType.None:
                    break;
                case BurfType.Cure:
                    int healQuantity = (int) (burf.Factor * boss.Status.FullHealth);
                    noticeBuilder.AppendFormat("프로젝트가 {0}의 체력을 회복합니다!", healQuantity);
                    break;
                case BurfType.DecreaseDamage:
                    break;
                case BurfType.Overwhelming:
                    break;
                default:
                    break;
            }
        }

        BossSkillText.text = noticeBuilder.ToString();
    }
}
