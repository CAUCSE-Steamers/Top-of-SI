using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public class HealBurf : IBurf, IStatusModificationCommand
    {
        public HealBurf(int healQuantity, int fullIncraseingQuantity)
        {
            HealQuantity = healQuantity;
            FullIncreasingQuantity = fullIncraseingQuantity;
        }

        public int HealQuantity
        {
            get; private set;
        }

        public int FullIncreasingQuantity
        {
            get; private set;
        }

        public bool IsPositiveBurf
        {
            get
            {
                return HealQuantity >= 0 && FullIncreasingQuantity >= 0;
            }
        }

        public bool IsPersistent
        {
            get
            {
                return true;
            }
        }

        public void Modify(ProgrammerStatus status)
        {
            status.FullHealth += FullIncreasingQuantity;
            status.Health = Mathf.Clamp(status.Health + HealQuantity, 0, status.FullHealth);

            CommonLogger.LogFormat("HealBurf::Modify => 프로그래머 '{0}'가 힐 버프를 받음. 체력이 {1}, 총 체력이 {2}만큼 증가함.", status.Name, HealQuantity, FullIncreasingQuantity);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.FullHealth -= FullIncreasingQuantity;
            status.Health = Mathf.Clamp(status.Health - HealQuantity, 0, status.FullHealth);

            CommonLogger.LogFormat("HealBurf::Modify => 프로그래머 '{0}'에 적용된 힐 버프가 해제됨. 체력이 {1}, 총 체력이 {2}만큼 감소함.", status.Name, HealQuantity, FullIncreasingQuantity);
        }
    }
}
