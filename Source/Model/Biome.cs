using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataBiome : EData {
        public override string Category => "Biome";
        public override Type DefType => typeof(BiomeDef);
        
        public float animalDensity;
        public float plantDensity;
        public float diseaseMtbDays = 60f;
        public float factionBaseSelectionWeight = 1f;

        public bool impassable;
        public bool hasVirtualPlants = true;

        public int pathCost_spring = 1650;
        public int pathCost_summer = 1650;
        public int pathCost_fall = 1650;
        public int pathCost_winter = 27500;

        public string texture;

        [NoCrawl]
        public List<NameValueFloat> wildAnimals = new List<NameValueFloat>();

        [NoCrawl]
        public List<NameValueFloat> wildPlants = new List<NameValueFloat>();

        [NoCrawl]
        public List<NameValueFloat> diseases = new List<NameValueFloat>();

        public override void Crawl(Def baseDef) {
            base.Crawl(baseDef);

            var def = baseDef as BiomeDef;

            foreach (var animalPawnKind in DefDatabase<PawnKindDef>.AllDefs) {
                float value = def.CommonalityOfAnimal(animalPawnKind) / animalPawnKind.wildSpawn_GroupSizeRange.Average;
                if (value > 0) {
                    this.wildAnimals.Add(new NameValueFloat(animalPawnKind.defName, value));
                }
            }

            foreach (var plantThing in DefDatabase<ThingDef>.AllDefs) {
                float value = def.CommonalityOfPlant(plantThing);
                if (value > 0) {
                    this.wildPlants.Add(new NameValueFloat(plantThing.defName, value));
                }
            }

            foreach (var diseaseIncident in DefDatabase<IncidentDef>.AllDefs) {
                float value = def.CommonalityOfDisease(diseaseIncident);
                if (value > 0) {
                    this.diseases.Add(new NameValueFloat(diseaseIncident.diseaseIncident.defName, value));
                }
            }
        }
    }

    class LangBiome : ELang {
        public override string Category => "Biome";
        public override Type DefType => typeof(BiomeDef);
    }
}
