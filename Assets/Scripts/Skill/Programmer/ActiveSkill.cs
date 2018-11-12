using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using UnityEngine;

namespace Model
{
    public abstract class ActiveSkill : ICooldownRequired, ILevelUp, IXmlConvertible
    {
        private double baseDamage = 1.0;
        private double defaultCooldown;

        public ActiveSkill(SkillBasicInformation information, IEnumerable<PassiveSkill> passiveSkills, double defaultCooldown)
        {
            Information = information;
            AuxiliaryPassiveSkills = passiveSkills;
            BaseDamage = baseDamage;
            DefaultCooldown = defaultCooldown;

            RemainingCooldown = 0.0;
            AdditionalDamageRatio = 0.0;
            AdditionlCooldownRatio = 0.0;
        }

        public double RemainingCooldown
        {
            get; private set;
        }

        public void ApplySkill(IHurtable hurtable, ProjectType projectType, RequiredTechType techType)
        {
            System.Random random = new System.Random();
            if(random.NextDouble() > Accuracy)
            {
                //TODO : Write Script that skill is not correct.
                DebugLogger.LogWarningFormat("스킬이 빗나감.");
            }
            else
            {
                if (IsTriggerable == false)
                {
                    DebugLogger.LogWarningFormat("ActiveSkill::ApplySkill => Skill '{0}'의 쿨타임이 {1} 남아있는 상태에서 발동되려고 합니다.", Information.Name, RemainingCooldown);
                }

                hurtable.Hurt((int)CalculateDamage(projectType, techType));

                RemainingCooldown = DefaultCooldown;
            }
        }

        public bool IsTriggerable
        {
            get
            {
                return RemainingCooldown < double.Epsilon;
            }
        }

        public void DecreaseCooldown()
        {
            if (RemainingCooldown >= 1.0)
            {
                RemainingCooldown -= 1.0;
            }
        }

        public SkillBasicInformation Information
        {
            get; private set;
        }

        public IEnumerable<PassiveSkill> AuxiliaryPassiveSkills
        {
            get; private set;
        }

        public double AdditionalDamageRatio
        {
            get; set;
        }

        public double BaseDamage
        {
            get
            {
                return baseDamage * (1.0 + AdditionalDamageRatio);
            }
            private set
            {
                baseDamage = value;
            }
        }

        public double AdditionlCooldownRatio
        {
            get; set;
        }

        public double DefaultCooldown
        {
            get
            {
                return defaultCooldown * (1.0 + AdditionlCooldownRatio);
            }
            private set
            {
                defaultCooldown = value;
            }
        }

        // if random double number is smaller than accuracy, skill correct.
        public double Accuracy
        {
            get; set;
        }

        public IEnumerable<PassiveSkill> FlattenContainingPassiveSkills()
        {
            var passiveSkills = new List<PassiveSkill>();
            if (AuxiliaryPassiveSkills != null)
            {
                passiveSkills.AddRange(AuxiliaryPassiveSkills);

                var passiveQueue = new Queue<PassiveSkill>(AuxiliaryPassiveSkills);

                while (passiveQueue.Count > 0)
                {
                    var lowerPassiveSkill = passiveQueue.Dequeue();
                    if (lowerPassiveSkill.AuxiliaryPassiveSkills != null)
                    {
                        passiveSkills.AddRange(lowerPassiveSkill.AuxiliaryPassiveSkills);
                    }
                }
            }

            return passiveSkills.AsReadOnly();
        }

        private double CalculatePassiveAppliedCooldown()
        {
            double additionCooldown = AdditionalValueFromPassive<ICooldownConvertible>(DefaultCooldown, cooldownPassive =>
            {
                return cooldownPassive.CalculateAppliedCooldown(DefaultCooldown);
            });

            return DefaultCooldown + additionCooldown;
        }

        private double AdditionalValueFromPassive<T>(double baseValue, Func<T, double> valueApplyingFunction)
        {
            double additionalValue = 0.0;

            if (AuxiliaryPassiveSkills != null)
            {
                foreach (var passiveSkill in AuxiliaryPassiveSkills.OfType<T>()
                                                                   .Where(passiveSkill => (passiveSkill as PassiveSkill).Information.AcquisitionLevel > 0))
                {
                    additionalValue += (baseValue - valueApplyingFunction(passiveSkill));
                }
            }

            return additionalValue;
        }

        private double CalculateDamage(ProjectType projectType, RequiredTechType techType)
        {
            double typeAppliedDamage = CalculateProjectTypeAppliedDamage(projectType);
            double techAppliedDamage = CalculatetechTypeAppliedDamage(typeAppliedDamage, techType);
            double additionalDamage = AdditionalValueFromPassive<IDamageConvertible>(techAppliedDamage, damagePassive =>
            {
                return damagePassive.CalculateAppliedDamage(techAppliedDamage, projectType, techType);
            });

            return CalculateSkillLevelDamage(techAppliedDamage) + additionalDamage;
        }

        protected double CalculateProjectTypeAppliedDamage(ProjectType projectType)
        {
            return BaseDamage * (1 + SynastryCache.LanguageToProjectSynastry.GetValue(Information.Type)(projectType));
        }

        protected double CalculatetechTypeAppliedDamage(double projectTypeAppliedDamage, RequiredTechType techType)
        {
            return projectTypeAppliedDamage * (1 + SynastryCache.LanguageToTechSynastry.GetValue(Information.Type)(techType));
        }

        protected abstract double CalculateSkillLevelDamage(double projectTypeAppliedDamage);

        public abstract void LevelUP();

        public static ActiveSkill ParseXml(XElement skillElement)
        {
            var skillType = typeof(ActiveSkill).Assembly
                                               .GetTypes()
                                               .Where(type => type.FullName == skillElement.AttributeValue("Type"))
                                               .Single();

            var recoveredSkill = skillType.GetConstructor(new Type[] { })
                                         .Invoke(new object[] { }) as ActiveSkill;

            recoveredSkill.Information.RecoverStateFromXml(skillElement.Element("SkillInfo").ToString());

            var passiveSkills = new List<PassiveSkill>();
            foreach (var passiveElement in skillElement.Element("Auxiliary").Elements())
            {
                passiveSkills.Add(PassiveSkill.ParseXml(passiveElement));
            }

            recoveredSkill.AuxiliaryPassiveSkills = passiveSkills;
            return recoveredSkill;
        }

        public XElement ToXmlElement()
        {
            var activeRoot = new XElement("Skill",
                                          new XAttribute("Type", GetType().FullName),
                                          Information.ToXmlElement());

            activeRoot.Add(new XElement("Auxiliary",
                AuxiliaryPassiveSkills.Select(passiveSkill => passiveSkill.ToXmlElement())));

            return activeRoot;
        }
    }
}
