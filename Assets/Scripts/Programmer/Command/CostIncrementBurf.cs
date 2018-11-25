using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class CostIncrementBurf : IBurf, IStatusModificationCommand
    {
        public CostIncrementBurf(double incrementRatio)
        {
            IncrementRatio = incrementRatio;
        }

        public double IncrementRatio
        {
            get; set;
        }

        public int RemainingTurn
        {
            get; set;
        }

        public string Description
        {
            get
            {
                return string.Format("스킬 사용 시 정신력 소모량이 추가로 {0}배 증가합니다.", IncrementRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Cost";
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
                return IncrementRatio < 0.0;
            }
        }

        public IBurf Clone()
        {
            return new CostIncrementBurf(IncrementRatio)
            {
                RemainingTurn = this.RemainingTurn
            };
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AdditionalSkillCostRatio += IncrementRatio;

            CommonLogger.LogFormat("CostIncrementBurf::Modify => 프로그래머 '{0}'가 정신력 소모 증가 버프를 받음. 정신력 소모 비율이 {1} 증가함. 현재 : {2}", status.Name, IncrementRatio, status.AdditionalDamageRatio);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.AdditionalSkillCostRatio -= IncrementRatio;

            CommonLogger.LogFormat("CostIncrementBurf::Unmodify => 프로그래머 '{0}'의 정신력 소모 증가 버프가 해제됨. 정신력 소모 비율이 {1} 감소함. 현재 : {2}", status.Name, IncrementRatio, status.AdditionalDamageRatio);
        }
    }
}
