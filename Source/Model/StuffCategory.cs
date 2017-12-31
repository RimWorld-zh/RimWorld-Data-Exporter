using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataStuffCategory : EData {
        public override string Category => "StuffCategory";
        public override Type DefType => typeof(StuffCategoryDef);

        [NoCrawl]
        public List<ThingDef> stuffs;

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as StuffCategoryDef;

            this.stuffs = DefDatabase<ThingDef>
                            .AllDefs
                            .Where(t => t.IsStuff && t.stuffProps.categories.Contains(def))
                            .ToList();
        }
    }

    class LangStuffCategory : ELang {
        public override string Category => "StuffCategory";
        public override Type DefType => typeof(StuffCategoryDef);
    }
}
