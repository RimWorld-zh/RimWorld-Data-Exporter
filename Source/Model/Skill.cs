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

            this.workTags = Helper.Serialization.DataExtension.GetUniqueFlags(def.disablingWorkTags).ToList();
        }
    }
    class LangSkill : ELang {
        public override string Category => "Skill";
        public override Type DefType => typeof(SkillDef);

        public string skillLabel;
    }
}
