using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataJoyKind : EData {
        public override string Category => "JoyKind";
        public override Type DefType => typeof(JoyKindDef);

        [NoCrawl]
        public List<JoyGiverDef> joyGivers;

        [NoCrawl]
        public List<ThingDef> ingestibleThings;

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as JoyKindDef;

            this.joyGivers = 
                DefDatabase<JoyGiverDef>
                    .AllDefs
                    .Where(g => g.joyKind == def)
                    .ToList();
            this.ingestibleThings = 
                DefDatabase<ThingDef>
                    .AllDefs
                    .Where(t => t.ingestible?.joyKind == def)
                    .ToList();
        }
    }

    class LangJoyKind : ELang {
        public override string Category => "JoyKind";
        public override Type DefType => typeof(JoyKindDef);
    }
}
