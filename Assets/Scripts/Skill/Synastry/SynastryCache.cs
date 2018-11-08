using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public static class SynastryCache
    {
        private const string LanguageProjectSynastryResource = "LanguageToProjectSynastry";
        private const string LanguageTechSynastryResource = "LanguageToTechniqueSynastry";
        private const string KnowledgeTechSynastryResource = "KnowledgeToTechniqueSynastry";

        static SynastryCache()
        {
            var synastryReader = new SynastryReader();

            var languageProjectSynastryAsset = ResourceLoadUtility.LoadData(LanguageProjectSynastryResource);
            var languageTechSynastryAsset = ResourceLoadUtility.LoadData(LanguageTechSynastryResource);
            var knowledgeTechSynastryAsset = ResourceLoadUtility.LoadData(KnowledgeTechSynastryResource);

            LanguageToProjectSynastry = synastryReader.ParseXml<SkillType, ProjectType>(languageProjectSynastryAsset.text);
            LanguageToTechSynastry = synastryReader.ParseXml<SkillType, RequiredTechType>(languageTechSynastryAsset.text);
            KnowledgeToTechSynastry = synastryReader.ParseXml<KnowledgeType, RequiredTechType>(knowledgeTechSynastryAsset.text);
        }

        public static Synastry<SkillType, ProjectType> LanguageToProjectSynastry
        {
            get; private set;
        }

        public static Synastry<SkillType, RequiredTechType> LanguageToTechSynastry
        {
            get; private set;
        }

        public static Synastry<KnowledgeType, RequiredTechType> KnowledgeToTechSynastry
        {
            get; private set;
        }
    }
}
