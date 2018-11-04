using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Model
{
    public interface ISoundProducible
    {
        AudioClip EffectSound
        {
            get;
        }
    }
}
