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
    }

    class LangStuffCategory : ELang {
        public override string Category => "StuffCategory";
        public override Type DefType => typeof(StuffCategoryDef);
    }
}
