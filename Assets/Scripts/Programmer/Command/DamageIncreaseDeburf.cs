using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class DamageIncreaseDeburf : IBurf, IStatusModificationCommand
    {
        private double xRatio;
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
                return string.Format("자신이 받은 데미지를 {0} 배 증가시킨다.", IncreaseRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "DamageIncrease";
            }

        }

        public bool IsPersistent
        {
            get
            {
                return true;
            }
        }

        public bool IsPositiveBurf
        {
            get
            {
                return false;
            }
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio += IncreaseRatio;
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.AdditionalDamageRatio -= IncreaseRatio;
        }
    }
}
