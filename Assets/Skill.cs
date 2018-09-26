using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    interface BossSkill
    {
        void Do(Animator anim);       // 프로그래머 객체 배열
    }

    interface ProgrammerSkill
    {
        void Do();
    }
}
