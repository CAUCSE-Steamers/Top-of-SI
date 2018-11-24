using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class DamageSplashBurf : IBurf, IStatusModificationCommand
    {
        public DamageSplashBurf(double decreaseRatio)
        {
            DecreaseRatio = decreaseRatio;
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
                return string.Format("자신이 받은 데미지를 다른 프로그래머들과 공유한다.");
            }
        }

        public string IconName
        {
            get
            {
                return "Splash";
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
