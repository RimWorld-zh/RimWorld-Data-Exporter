using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataKeyBindingCategory : EData {
        public override string Category => "KeyBindingCategory";
        public override Type DefType => typeof(KeyBindingCategoryDef);
    }

    class LangKeyBindingCategory : ELang {
        public override string Category => "KeyBindingCategory";
        public override Type DefType => typeof(KeyBindingCategoryDef);
    }
}
