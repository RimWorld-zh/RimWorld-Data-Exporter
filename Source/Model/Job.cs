using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;

namespace RimWorldDataExporter.Model {
    class DataJob: EData {
        public override string Category => "Job";
        public override Type DefType => typeof(JobDef);
        
        public bool playerInterruptible = true;
        public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;
        public bool alwaysShowWeapon;
        public bool neverShowWeapon;
        public bool suspendable = true;
        public bool casualInterruptible = true;
        public bool collideWithPawns;
        public bool isIdle;
        public TaleDef taleOnCompletion;
        public bool makeTargetPrisoner;
        public int joyDuration = 4000;
        public int joyMaxParticipants = 1;
        public float joyGainRate = 1f;
        public SkillDef joySkill;
        public float joyXpPerTick;
        public JoyKindDef joyKind;
    }

    class LangJob : ELang {
        public override string Category => "Job";
        public override Type DefType => typeof(JobDef);

        public string reportString = "Doing something.";
    }
}
