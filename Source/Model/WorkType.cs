using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataWorkType : EData {
        public override string Category => "WorkType";
        public override Type DefType => typeof(WorkTypeDef);

        public int naturalPriority;
        public List<SkillDef> relevantSkills = new List<SkillDef>();
        public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

        [NoCrawl]
        public List<WorkTags> workTags;

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as WorkTypeDef;

            this.workTags = Helper.Serialization.DataExtension.GetUniqueFlags(def.workTags).ToList();
        }

        // Aggregation

        class AggrWorkType : EAggr {
            public List<WorkTypeDef> workTypsByPriority;

            public AggrWorkType() {
                var list = DefDatabase<WorkTypeDef>.AllDefs.ToList();
                list.Sort((a, b) => b.naturalPriority - a.naturalPriority);
                this.workTypsByPriority = list;
            }
        }

        public override EAggr GetAggregation() {
            return new AggrWorkType();
        }
    }

    class LangWorkType : ELang {
        public override string Category => "WorkType";
        public override Type DefType => typeof(WorkTypeDef);
        
        public string labelShort;
        public string pawnLabel;
    }
}
