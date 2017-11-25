using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataHair : EData {
        public override string Category => "Hair";
        public override Type DefType => typeof(HairDef);

        public string texPath;
        public HairGender hairGender;
        public List<string> hairTags = new List<string>();
    }

    class LangHair : ELang {
        public override string Category => "Hair";
        public override Type DefType => typeof(HairDef);
    }
}
