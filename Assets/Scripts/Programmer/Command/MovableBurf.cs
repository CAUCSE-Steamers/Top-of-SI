using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MovableBurf : IBurf, IStatusModificationCommand
    {
        public MovableBurf(bool moveState)
        {
            MoveState = moveState;
        }

        public bool MoveState
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
                return string.Format("이동이 제한됩니다.");
            }
        }

        public string IconName
        {
            get
            {
                return "Stone";
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
                return MoveState == true;
            }
        }

        public IBurf Clone()
        {
            return new MovableBurf(MoveState)
            {
                RemainingTurn = this.RemainingTurn
            };
        }

        public void Modify(ProgrammerStatus status)
        {
            status.IsMovable = MoveState;
            CommonLogger.LogFormat("MovableBurf::Modify => 프로그래머 '{0}'의 이동 여부가 재설정됨. 값 = {1}", status.Name, MoveState);
        }

        public void Unmodify(ProgrammerStatus status)
        {
            status.IsMovable = !MoveState;
            CommonLogger.LogFormat("MovableBurf::Unmodify => 프로그래머 '{0}'의 이동 여부가 재설정됨. 값 = {1}", status.Name, !MoveState);
        }
    }
}
