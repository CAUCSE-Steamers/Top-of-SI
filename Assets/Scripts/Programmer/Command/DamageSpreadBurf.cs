using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class DamageSpreadBurf : IBurf, IStatusModificationCommand
    {
        public DamageSpreadBurf(double ratio, double decreaseRatio)
        {
            SpreadRatio = ratio;
            DecreaseRatio = decreaseRatio;
        }

        public double SpreadRatio
        {
            get; private set;
        }

        public double DecreaseRatio
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
                return string.Format("자신이 받은 데미지의 {0} 배를 다른 프로그래머들에게 전달한다.", SpreadRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Spread";
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
            
        }

        public void Unmodify(ProgrammerStatus status)
        {
            
        }
    }
}
