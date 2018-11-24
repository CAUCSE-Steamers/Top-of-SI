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
                return IncreaseRatio > 0.0;
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
