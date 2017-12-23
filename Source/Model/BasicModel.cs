using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace RimWorldDataExporter.Model {

    class FloatDefValue : Helper.Serialization.ITypeable {
        string defName;
        float value;

        public FloatDefValue(string defName, float value) {
            this.defName = defName;
            this.value = value;
        }

        public FloatDefValue(Def def, float value) : this(def.defName, value) { }
    }

    class IntDefValue : Helper.Serialization.ITypeable {
        string defName;
        int value;

        public IntDefValue(string defName, int value) {
            this.defName = defName;
            this.value = value;
        }

        public IntDefValue(Def def, int value) : this(def.defName, value) { }
    }
}
