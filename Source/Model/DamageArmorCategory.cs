using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataDamageArmorCategory : EData {
        public override string Category => "DamageArmorCategory";
        public override Type DefType => typeof(DamageArmorCategoryDef);

        public StatDef multStat;
        public StatDef deflectionStat;
    }
}
