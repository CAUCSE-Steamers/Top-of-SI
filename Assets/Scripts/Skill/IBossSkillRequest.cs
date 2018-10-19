using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    interface IBossSkillRequest
    {
        void Invoke(Animator animator);
    }
}
