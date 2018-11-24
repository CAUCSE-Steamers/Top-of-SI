using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DamageSplashBurf : IBurf, IConstantAcceptableCommand<double>
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
                return string.Format("피격 시 다른 프로그래머들과 나눠진 데미지를 공유합니다.");
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

        public void Accept(double hurtDamage)
        {
            var programmers = StageManager.Instance.Unit.Programmers;

            double dividedDamage = hurtDamage / programmers.Count();

            foreach (var programmer in programmers)
            {
                programmer.Hurt((int) dividedDamage);
            }
        }

        public void Leave(double value)
        {

        }
    }
}
