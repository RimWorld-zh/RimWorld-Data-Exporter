using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataSkill : EData {
        public override string Category => "Skill";
        public override Type DefType => typeof(SkillDef);

        [NoCrawl]
        public List<WorkTags> workTags;

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as SkillDef;

            this.workTags =
                Helper
                    .Serialization
                    .DataExtension
                    .GetUniqueFlags(def.disablingWorkTags)
                    .ToList();
        }

        // Aggregation

        class AggrSkill : EAggr {
            public readonly int intervalTicks = SkillRecord.IntervalTicks;

            public readonly int minLevel = SkillRecord.MinLevel;
            public readonly int maxLevel = SkillRecord.MaxLevel;

            public readonly int maxFullRateXpPerDay = SkillRecord.MaxFullRateXpPerDay;

            public readonly int masterSkillThreshold = SkillRecord.MasterSkillThreshold;

            public readonly float saturatedLearningFactor = SkillRecord.SaturatedLearningFactor;

            public readonly float learnFactorPassionNone = SkillRecord.LearnFactorPassionNone;
            public readonly float learnFactorPassionMinor = SkillRecord.LearnFactorPassionMinor;
            public readonly float learnFactorPassionMajor = SkillRecord.LearnFactorPassionMajor;

            public readonly double joyGainFactorPassionNone = 0.0;
            public readonly double joyGainFactorPassionMinor = 1.9999999494757503E-05;
            public readonly double joyGainFactorPassionMajor = 3.9999998989515007E-05;

            public readonly List<float> xpRequirements = new List<float>();
            public readonly List<float> xpLosses = new List<float>();

            public static float XpLossFrom(int level) {
                switch (level) {
                case 10:
                    return -0.1f;
                case 11:
                    return -0.2f;
                case 12:
                    return -0.4f;
                case 13:
                    return -0.6f;
                case 14:
                    return -1f;
                case 15:
                    return -1.8f;
                case 16:
                    return -2.8f;
                case 17:
                    return -4f;
                case 18:
                    return -6f;
                case 19:
                    return -8f;
                case 20:
                    return -12f;
                default:
                    return 0;
                }
            }

            public AggrSkill() {
                for (int level = 0; level <= 20; level++) {
                    xpRequirements.Add(SkillRecord.XpRequiredToLevelUpFrom(level));
                    xpLosses.Add(AggrSkill.XpLossFrom(level));
                }
            }
        }

        public override EAggr GetAggregation() {
            return new AggrSkill();
        }
    }
    class LangSkill : ELang {
        public override string Category => "Skill";
        public override Type DefType => typeof(SkillDef);

        public string skillLabel;
    }
}
