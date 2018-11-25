using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IStageModificationCommand
    {
        void Modify(GameStage stage);
        void Unmodify(GameStage stage);
    }
}
