using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataJoyGiver : EData {
        public override string Category => "JoyGiver";
        public override Type DefType => typeof(JoyGiverDef);

        public float baseChance;
        public List<ThingDef> thingDefs;
        public JobDef jobDef;
        public JoyKindDef joyKind;
        public bool desireSit = true;
        public float pctPawnsEverDo = 1f;
        public bool unroofedOnly;
        public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();
        public bool canDoWhileInBed;
    }
}
