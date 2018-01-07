using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataKeyBinding : EData {
        public override string Category => "KeyBinding";
        public override Type DefType => typeof(KeyBindingDef);

        public KeyBindingCategoryDef category;
        public KeyCode defaultKeyCodeA;
        public KeyCode defaultKeyCodeB;
    }

    class LangKeyBinding : ELang {
        public override string Category => "KeyBinding";
        public override Type DefType => typeof(KeyBindingDef);
    }
}
