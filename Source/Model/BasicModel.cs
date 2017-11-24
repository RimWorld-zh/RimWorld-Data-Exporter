using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RimWorldDataExporter.Model {

    [Helper.Serialization.Serializable]
    class NameValueFloat {
        string defName;
        float value;
        public NameValueFloat(string defName, float value) {
            this.defName = defName;
            this.value = value;
        }
    }
}
