using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataDamage : EData {
        public override string Category => "Damage";
        public override Type DefType => typeof(DamageDef);
        
        public bool makesBlood = true;
        public DamageArmorCategoryDef armorCategory;
        public bool spreadOut;
        public bool isExplosive;
        public int explosionDamage = 10;
        public float explosionBuildingDamageFactor = 1f;
        public Color explosionColorCenter = Color.white;
        public Color explosionColorEdge = Color.white;
        public HediffDef hediff;
        public HediffDef hediffSkin;
        public HediffDef hediffSolid;
    }

    class LangDamage : ELang {
        public override string Category => "Damage";
        public override Type DefType => typeof(DamageDef);
    }
}
