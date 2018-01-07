using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataResearchProject : EData {
        public override string Category => "ResearchProject";
        public override Type DefType => typeof(ResearchProjectDef);
        
        public TechLevel techLevel;
        public float baseCost = 100f;
        public List<ResearchProjectDef> prerequisites;
        public List<ResearchProjectDef> requiredByThis;
        public ThingDef requiredResearchBuilding;
        public List<ThingDef> requiredResearchFacilities;
        public List<string> tags;
        //public ResearchTabDef tab;
        public float researchViewX = 1f;
        public float researchViewY = 1f;
    }

    class LangResearchProject : ELang {
        public override string Category => "ResearchProject";
        public override Type DefType => typeof(ResearchProjectDef);
        
        private string descriptionDiscovered;
    }
}
