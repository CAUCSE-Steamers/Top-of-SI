using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ElapsedDayObjective : IStageObjective
    {
        private const string DescriptionFormat = "{0}일 내에 프로젝트 완수";

        private GameStage stage;

        public ElapsedDayObjective(GameStage stage)
        {
            this.stage = stage;
        }

        public string Description
        {
            get
            {
                return string.Format(DescriptionFormat, stage.ElapsedDayLimit);
            }
        }
    }
}
