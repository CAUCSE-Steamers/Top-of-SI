using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Model
{
    public abstract class ActiveSkill : ICooldownRequired, ILevelUp, IXmlConvertible
    {
        public event Action<ActiveSkill> OnSkillMissed = delegate { };

        private double baseDamage = 1.0;
        private double additionalCooldownRatio;
        private double initialCooldown;
        private double accuracy;
        private int cost;

        public ActiveSkill(SkillBasicInformation information, IEnumerable<PassiveSkill> passiveSkills, double defaultCooldown)
        {
            Information = information;
            AuxiliaryPassiveSkills = passiveSkills;
            BaseDamage = baseDamage;
            initialCooldown = defaultCooldown;

            Information.LearnEnabled = true;
            RemainingCooldown = 0.0;
            AdditionalDamageRatio = 0.0;
            AdditionlCooldownRatio = 0.0;
            Accuracy = 0.0;
            Information.CriticalRatio = 0.5;
            Information.CriticalProbability = 0.9;
        }

        public double RemainingCooldown
        {
            get; private set;
        }

        public void ApplySkill(IHurtable hurtable, ProjectType projectType, RequiredTechType techType)
        {
            if (Random.Range(0f, 1f) > Accuracy)
            {
                OnSkillMissed(this);
            }
            else
            {
                double critical = 1;
                if (IsTriggerable == false)
                {
                    DebugLogger.LogWarningFormat("ActiveSkill::ApplySkill => Skill '{0}'의 쿨타임이 {1} 남아있는 상태에서 발동되려고 합니다.", Information.Name, RemainingCooldown);
                }
                if(UnityEngine.Random.Range(0, 1) > Information.CriticalProbability)
                {
                    critical += Information.CriticalRatio;
                }

                hurtable.Hurt((int)(CalculateDamage(projectType, techType) * critical));

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
            if (RemainingCooldown > 0.0)
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
            get
            {
                return additionalCooldownRatio;
            }
            set
            {
                double currentProgressedCooldownRatio = 1.0 - (RemainingCooldown / DefaultCooldown);

                additionalCooldownRatio = value;

                RemainingCooldown = DefaultCooldown * (1.0 - currentProgressedCooldownRatio);
            }
        }

        public double DefaultCooldown
        {
            get
            {
                return CalculatePassiveAppliedCooldown() * (1.0 + AdditionlCooldownRatio);
            }
        }

        public double Accuracy
        {
            get
            {
                double additionalAccuracy = AdditionalValueFromPassive<IAccuracyConvertible>(accuracy, accuracyPassive =>
                {
                    return accuracyPassive.CalculateAppliedAccuracy(accuracy);
                });

                return accuracy + additionalAccuracy;
            }
            set
            {
                accuracy = value;
            }
        }

        public int Cost
        {
            get
            {
                return cost;
            }
            set
            {
                cost = value;
            }
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
            double additionCooldown = AdditionalValueFromPassive<ICooldownConvertible>(initialCooldown, cooldownPassive =>
            {
                return cooldownPassive.CalculateAppliedCooldown(initialCooldown);
            });

            return initialCooldown + additionCooldown;
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
            double levelAppliedDamage = CalculateSkillLevelDamage(BaseDamage);
            double additionalDamage = AdditionalValueFromPassive<IDamageConvertible>(levelAppliedDamage, damagePassive =>
            {
                return damagePassive.CalculateAppliedDamage(levelAppliedDamage, projectType, techType);
            });

            double passiveAppliedDamage = levelAppliedDamage + additionalDamage;

            double typeAppliedDamage = passiveAppliedDamage * (1 + SynastryCache.LanguageToProjectSynastry.GetValue(Information.Type)(projectType));
            double techAppliedDamage = typeAppliedDamage * (1 + SynastryCache.LanguageToTechSynastry.GetValue(Information.Type)(techType));

            return techAppliedDamage;
        }

        protected abstract double CalculateSkillLevelDamage(double baseDamage);

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
