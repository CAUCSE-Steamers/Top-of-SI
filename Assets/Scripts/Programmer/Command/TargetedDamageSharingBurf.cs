using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TargetedDamageSharingBurf : IBurf, ITupleAcceptableCommand<Programmer, double>
    {
        public TargetedDamageSharingBurf(double sharingRatio)
        {
            SharingRatio = sharingRatio;
        }

        public double SharingRatio
        {
            get; private set;
        }

        public Programmer TargetProgrammer
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
                return string.Format("피격 시 데미지의 {0}배를 프로그래머 '{1}'이/가 대신 받습니다.", SharingRatio, TargetProgrammer.Status.Name);
            }
        }

        public string IconName
        {
            get
            {
                return "Target";
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

        public void Accept(Programmer programmer, double hurtDamage)
        {
            double sharedDamage = hurtDamage * SharingRatio;
            double decreasedDamage = hurtDamage * (1 - SharingRatio);

            programmer.Hurt((int) decreasedDamage);
            TargetProgrammer.Hurt((int) sharedDamage);
        }

        public void Leave(Programmer programmer, double hurtDamage)
        {

        }
    }
}