using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class SocialityBurf : IBurf, IStatusModificationCommand
    {
        public SocialityBurf(int sociality)
        {
            Sociality = sociality;
        }

        public int Sociality
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
                return string.Format("자신의 리더십을 {0} 증가시킨다.", Sociality);
            }
        }

        public string IconName
        {
            get
            {
                return "Sociality";
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
                return true;
            }
        }

        public void Modify(ProgrammerStatus status)
        {
            status.AddSociality(Sociality);
            CommonLogger.LogFormat("LeadershipBurf::Modify => 프로그래머 '{0}'의 리더십이 {1}만큼 증가함.", status.Name, Sociality);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            CommonLogger.LogFormat("LeadershipBurf::Modify => 프로그래머 '{0}'에 적용된 리더십 버프가 해제됨.", status.Name);
        }
    }
}
