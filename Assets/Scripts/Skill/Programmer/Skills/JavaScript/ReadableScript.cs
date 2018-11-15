using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class ReadableScript : PassiveSkill, IAccuracyConvertible
    {
        private static SkillBasicInformation information = new SkillBasicInformation()
        {
            Name = "ReadableScript",
            Type = SkillType.JavaScript,
            AcquisitionLevel = 0,
            MaximumLevel = 20,
            IconName = "JavaScript",
            RequiredUpgradeCost = 3,
            DescriptionFunc = level => string.Format("가독성 좋은 코드를 작성합니다. 스킬의 명중률이 {0}% 증가합니다.", level)
        };

        public ReadableScript() : base(information, Enumerable.Empty<PassiveSkill>())
        {

        }

        public double calculateAppliedAccuracy(double accuracy)
        {
            return accuracy * (1 + information.AcquisitionLevel / 100);
        }

        public override void LevelUP()
        {
            information.AcquisitionLevel++;
        }
    }
}
