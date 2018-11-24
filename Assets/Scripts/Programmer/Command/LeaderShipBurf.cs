using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class LeadershipBurf : IBurf, IStatusModificationCommand
    {
        public LeadershipBurf(int leadership)
        {
            Leadership = leadership;
        }

        public int Leadership
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
                return string.Format("매 턴마다 리더쉽이 {0}만큼 증가합니다.", Leadership);
            }
        }

        public string IconName
        {
            get
            {
                return "Leader";
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
                return Leadership > 0;
            }
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AddLeadership(Leadership);
            CommonLogger.LogFormat("LeadershipBurf::Modify => 프로그래머 '{0}'의 리더십이 {1}만큼 증가함.", status.Name, Leadership);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            CommonLogger.LogFormat("LeadershipBurf::Unmodify => 프로그래머 '{0}'에 적용된 리더십 버프가 해제됨.", status.Name);
        }
    }
}
