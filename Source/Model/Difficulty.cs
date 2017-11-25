using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataDifficulty : EData {
        public override string Category => "Difficulty";
        public override Type DefType => typeof(DifficultyDef);

        public int difficulty = -1;
        public float threatScale;
        public bool allowBigThreats = true;
        public bool allowIntroThreats = true;
        public bool allowCaveHives = true;
        public bool peacefulTemples;
        public float colonistMoodOffset;
        public float tradePriceFactorLoss;
        public float cropYieldFactor = 1f;
        public float diseaseIntervalFactor = 1f;
        public float enemyReproductionRateFactor = 1f;
        public float playerPawnInfectionChanceFactor = 1f;
        public float manhunterChanceOnDamageFactor = 1f;
    }

    class LangDifficulty : ELang {
        public override string Category => "Difficulty";
        public override Type DefType => typeof(DifficultyDef);
    }
}
