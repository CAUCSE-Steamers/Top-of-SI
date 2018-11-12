using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class HurtDamageBurf : IBurf, IStatusModificationCommand
    {
        public HurtDamageBurf(double additionalDamageRatio)
        {
            AdditionalDamageRatio = additionalDamageRatio;
        }

        public double AdditionalDamageRatio
        {
            get; private set;
        }

        public bool IsPositiveBurf
        {
            get
            {
                return AdditionalDamageRatio <= 1.0;
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
                return string.Format("피격 시 {0}배 피해를 받습니다.", AdditionalDamageRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Liquid";
            }
        }

        public int RemainingTurn
        {
            get; set;
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio += AdditionalDamageRatio;

            CommonLogger.LogFormat("HurtDamageBurf::Modify => 프로그래머 '{0}'가 피격 데미지 버프를 받음. 피격 데미지 비율이 {1} 증가함. 현재 : {2}", status.Name, AdditionalDamageRatio, status.AdditionalDamageRatio);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio -= AdditionalDamageRatio;

            CommonLogger.LogFormat("HurtDamageBurf::Modify => 프로그래머 '{0}'의 피격 데미지 버프가 해제됨. 피격 데미지 비율이 {1} 감소함. 현재 : {2}", status.Name, AdditionalDamageRatio, status.AdditionalDamageRatio);
        }
    }
}
