using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ReadableScript : PassiveSkill, IAccuracyConvertible
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

        public ReadableScript() : base(information.Clone(), Enumerable.Empty<PassiveSkill>())
        {

        }

        public double CalculateAppliedAccuracy(double accuracy)
        {
            return accuracy * (1 + (Information.AcquisitionLevel * 0.01));
        }
    }
}
