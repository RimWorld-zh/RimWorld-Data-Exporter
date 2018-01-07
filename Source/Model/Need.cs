using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataNeed : EData {
        public override string Category => "Need";
        public override Type DefType => typeof(NeedDef);

        public Intelligence minIntelligence;
        public bool colonistAndPrisonersOnly;
        public bool colonistsOnly;
        public bool onlyIfCausedByHediff;
        public bool neverOnPrisoner;
        public bool showOnNeedList = true;
        public float baseLevel = 0.5f;
        public bool major;
        public int listPriority;
        public string tutorHighlightTag;
        public bool showForCaravanMembers;
        public bool scaleBar;
        public float fallPerDay = 0.5f;
        public float seekerRisePerHour;
        public float seekerFallPerHour;
        public bool freezeWhileSleeping;
    }
}
