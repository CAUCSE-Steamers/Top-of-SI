using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class NormalAttackDamageBurf : IBurf, ISkillModificationCommand
    {
        public NormalAttackDamageBurf(double burfRatio)
        {
            DamageBurfRatio = burfRatio;
        }

        public double DamageBurfRatio
        {
            get; private set;
        }

        public bool IsPositiveBurf
        {
            get
            {
                return DamageBurfRatio >= 0.0;
            }
        }

        public bool IsPersistent
        {
            get
            {
                return false;
            }
        }

        public string Description
        {
            get
            {
                return string.Format("일반 공격 데미지가 {0}배 추가 적용됩니다.", DamageBurfRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Coffee";
            }
        }

        public int RemainingTurn
        {
            get; set;
        }

        public void Modify(ActiveSkill activeSkill)
        {
            if (activeSkill.Information.Type == SkillType.None)
            {
                activeSkill.AdditionalDamageRatio += DamageBurfRatio;

                CommonLogger.LogFormat("NormalAttackDamageBurf::Modify => 스킬 '{0}'에 데미지 버프를 받음. 데미지 비율이 {1} 증가함. 현재 : {2}", activeSkill.Information.Name, DamageBurfRatio, activeSkill.AdditionalDamageRatio);
            }
        }

        public void Unmodify(ActiveSkill activeSkill)
        {
            if (activeSkill.Information.Type == SkillType.None)
            {
                activeSkill.AdditionalDamageRatio -= DamageBurfRatio;

                CommonLogger.LogFormat("NormalAttackDamageBurf::Unmodify => 스킬 '{0}'의 데미지 버프가 해제됨. 데미지 비율이 {1} 감소함. 현재 : {2}", activeSkill.Information.Name, DamageBurfRatio, activeSkill.AdditionalDamageRatio);
            }
        }
    }
}
