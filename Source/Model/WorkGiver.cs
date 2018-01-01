using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataWorkGiver : EData {
        public override string Category => "WorkGiver";
        public override Type DefType => typeof(WorkGiverDef);

        public WorkTypeDef workType;

        [NoCrawl]
        public List<WorkTags> workTags;

        public int priorityInType;
        public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();
        public JobTag tagToGive = JobTag.MiscWork;
        public List<ThingDef> fixedBillGiverDefs;

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as WorkGiverDef;

            this.workTags = Helper.Serialization.DataExtension.GetUniqueFlags(def.workTags).ToList();
        }
    }

    class LangWorkGiver : ELang {
        public override string Category => "WorkGiver";
        public override Type DefType => typeof(WorkGiver);
    }
}
