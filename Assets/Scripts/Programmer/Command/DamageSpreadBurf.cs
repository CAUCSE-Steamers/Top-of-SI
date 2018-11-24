using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DamageSpreadBurf : IBurf, ITupleAcceptableCommand<Programmer, double>
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
                return string.Format("피격 데미지의 {0}배를 다른 프로그래머가 대신 나누어 받습니다.", SpreadRatio);
            }
        }

        public string IconName
        {
            get
            {
                return "Delegate";
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
                return true;
            }
        }

        public void Accept(Programmer programmer, double damage)
        {
            double spreadedDamage = damage * SpreadRatio;
            double decreasedDamage = damage * (1 - SpreadRatio);

            var programmers = StageManager.Instance.Unit.Programmers;

            if (programmers.Count() == 1)
            {
                programmers.Single().Hurt((int) damage);
            }
            else
            {
                foreach (var stageProgrammer in programmers)
                {
                    if (stageProgrammer.Equals(programmer))
                    {
                        stageProgrammer.Hurt((int) decreasedDamage);
                    }
                    else
                    {
                        stageProgrammer.Hurt((int) spreadedDamage);
                    }
                }
            }
        }

        public void Leave(Programmer first, double second)
        {

        }
    }
}
