using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataMainButton : EData {
        public override string Category => "MainButton";
        public override Type DefType => typeof(MainButtonDef);
        
        public bool buttonVisible = true;
        public int order;
        public KeyCode defaultHotKey;
        public bool validWithoutMap;
        public KeyBindingDef hotKey;
    }

    class LangMainButton : ELang {
        public override string Category => "MainButton";
        public override Type DefType => typeof(MainButtonDef);
    }
}
