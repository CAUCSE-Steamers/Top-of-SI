using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DamageIncreaseDeburf : IBurf, IStatusModificationCommand
    {
        public DamageIncreaseDeburf(double ratio)
        {
            IncreaseRatio = ratio;
        }

        public double IncreaseRatio
        {
            get; private set;
        }

        public int RemainingTurn
        {
            get; set;
        }

        public string Description
        {
            get
            {
                return string.Format("피격 데미지가 {0}배 추가 적용됩니다.", IncreaseRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Blood";
            }

        }

        public bool IsPersistent
        {
            get
            {
                return false;
            }
        }

        public bool IsPositiveBurf
        {
            get
            {
                return IncreaseRatio < 0.0;
            }
        }

        public IBurf Clone()
        {
            return new DamageIncreaseDeburf(IncreaseRatio)
            {
                RemainingTurn = this.RemainingTurn
            };
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio += IncreaseRatio;

            CommonLogger.LogFormat("DamageIncreaseDeburf::Modify => 프로그래머 '{0}'가 피격 데미지 버프를 받음. 피격 데미지 비율이 {1} 증가함. 현재 : {2}", status.Name, IncreaseRatio, status.AdditionalDamageRatio);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio -= IncreaseRatio;

            CommonLogger.LogFormat("DamageIncreaseDeburf::Modify => 프로그래머 '{0}'의 피격 데미지 버프가 해제됨. 피격 데미지 비율이 {1} 감소함. 현재 : {2}", status.Name, IncreaseRatio, status.AdditionalDamageRatio);
        }
    }
}
