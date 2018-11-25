using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SkillDamageBurf : IBurf, ISkillModificationCommand
    {
        public SkillDamageBurf(double burfRatio)
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
                return string.Format("모든 프로그래밍 공격 데미지가 {0}배 추가 적용됩니다.", DamageBurfRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "DoubleSword";
            }
        }

        public int RemainingTurn
        {
            get; set;
        }

        public IBurf Clone()
        {
            return new SkillDamageBurf(DamageBurfRatio)
            {
                RemainingTurn = this.RemainingTurn
            };
        }

        public void Modify(ActiveSkill activeSkill)
        {
            if (activeSkill.Information.Type != SkillType.None)
            {
                activeSkill.AdditionalDamageRatio += DamageBurfRatio;

                CommonLogger.LogFormat("SkillDamageBurf::Modify => 스킬 '{0}'에 데미지 버프를 받음. 데미지 비율이 {1} 증가함. 현재 : {2}", activeSkill.Information.Name, DamageBurfRatio, activeSkill.AdditionalDamageRatio);
            }
        }

        public void Unmodify(ActiveSkill activeSkill)
        {
            if (activeSkill.Information.Type != SkillType.None)
            {
                activeSkill.AdditionalDamageRatio -= DamageBurfRatio;

                CommonLogger.LogFormat("SkillDamageBurf::Unmodify => 스킬 '{0}'의 데미지 버프가 해제됨. 데미지 비율이 {1} 감소함. 현재 : {2}", activeSkill.Information.Name, DamageBurfRatio, activeSkill.AdditionalDamageRatio);
            }
        }
    }
}
