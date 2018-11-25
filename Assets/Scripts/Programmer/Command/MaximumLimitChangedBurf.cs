using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MaximumLimitChangedBurf : IBurf, IStageModificationCommand
    {
        public MaximumLimitChangedBurf(double increasingRatio)
        {
            IncreasingRatio = increasingRatio;
        }

        public double IncreasingRatio
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
                return string.Format("프로젝트의 기한을 추가로 {0}배 연장합니다.", IncreasingRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Heal";
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
                return IncreasingRatio > 0.0;
            }
        }

        public IBurf Clone()
        {
            return new MaximumLimitChangedBurf(IncreasingRatio)
            {
                RemainingTurn = this.RemainingTurn
            };
        }

        public void Modify(GameStage stage)
        {
            var statusManager = StageManager.Instance.Status;
            statusManager.MaximumDayLimit = (int) (statusManager.MaximumDayLimit * (1 + IncreasingRatio));

            CommonLogger.LogFormat("MaximumLimitChangedBurf::Modify => 프로젝트의 기한이 {0}배 증가함. 현재 : {1}", IncreasingRatio, statusManager.MaximumDayLimit);
        }

        public void Unmodify(GameStage stage)
        {
            var statusManager = StageManager.Instance.Status;
            CommonLogger.LogFormat("MaximumLimitChangedBurf::Modify => 프로젝트 기한 변경 버프가 해제됨. 비율 : {0}, 현재 : {1}", IncreasingRatio, statusManager.MaximumDayLimit);
        }
    }
}
