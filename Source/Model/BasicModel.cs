using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimWorldDataExporter.Model {
    
    class FloatDefValue: Helper.Serialization.ITypeable {
        string defName;
        float value;
        public FloatDefValue(string defName, float value) {
            this.defName = defName;
            this.value = value;
        }
    }
}
