using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataTerrain : DataBuildable {
        public static readonly List<StatDef> allStatDefs = new List<StatDef> {
            StatDefOf.Beauty,
            StatDefOf.Cleanliness,
            StatDefOf.Flammability,
            StatDefOf.WorkToBuild,
            StatDefOf.MarketValue,
        };

        public override string Category => "Terrain";
        public override Type DefType => typeof(TerrainDef);
        
        public Traversability passability;
        public int pathCost;
        public bool pathCostIgnoreRepeat = true;
        public float fertility = -1f;
        public TerrainAffordance terrainAffordanceNeeded = TerrainAffordance.Light;
        public List<ResearchProjectDef> researchPrerequisites;
        public int constructionSkillPrerequisite;
        public float resourcesFractionWhenDeconstructed = 0.75f;
        public DesignationCategoryDef designationCategory;
        public KeyBindingDef designationHotKey;
        public TechLevel minTechLevelToBuild;
        public TechLevel maxTechLevelToBuild;

        [NoCrawl]
        public List<FloatDefValue> stats = new List<FloatDefValue>();

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as TerrainDef;

            foreach (StatDef statDef in allStatDefs) {
                stats.Add(new FloatDefValue(statDef, def.GetStatValueAbstract(statDef)));
            }
        }
    }
    
    class LangTerrain : ELang {
        public override string Category => "Terrain";
        public override Type DefType => typeof(TerrainDef);
    }
}
