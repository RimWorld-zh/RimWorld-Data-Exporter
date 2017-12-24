using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataTerrain : EData {
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

        public string texturePath;
        public Color color = Color.white;

        public List<TerrainAffordance> affordances = new List<TerrainAffordance>();
        public bool takeFootprints;
        public bool takeSplashes;
        public bool avoidWander;
        public bool holdSnow = true;
        public bool extinguishesFire;

        public bool changeable = true;
        public TerrainDef smoothedTerrain;
        public TerrainDef driesTo;
        public TerrainDef burnedDef;

        public ThingDef terrainFilthDef;
        public bool acceptTerrainSourceFilth;
        public bool acceptFilth = true;

        [NoCrawl]
        public List<FloatDefValue> stats = new List<FloatDefValue>();

        [NoCrawl]
        public List<IntDefValue> costList;

        [NoCrawl]
        public float walkSpeed;

        [NoCrawl]
        public bool removable;
        [NoCrawl]
        public bool isCarpet;
        
        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as TerrainDef;

            foreach (StatDef statDef in allStatDefs) {
                stats.Add(new FloatDefValue(statDef, def.GetStatValueAbstract(statDef)));
            }
            this.costList = def.costList?.Select(thingCount => {
                return new IntDefValue(thingCount.thingDef, thingCount.count);
            }).ToList();
            this.walkSpeed = 13.0f / (def.pathCost + 13.0f);
            this.removable = def.Removable;
            this.isCarpet = def.IsCarpet;
        }
    }
    
    class LangTerrain : ELang {
        public override string Category => "Terrain";
        public override Type DefType => typeof(TerrainDef);
    }
}
