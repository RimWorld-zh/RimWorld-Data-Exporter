using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using RimWorldDataExporter.Helper;
using RimWorldDataExporter.Helper.Serialization;

namespace RimWorldDataExporter.Model {
    [RimWorldDataExporter.Helper.Serialization.Serializable]
    abstract class EAggr { }

    [RimWorldDataExporter.Helper.Serialization.Serializable]
    abstract class EObj : IEquatable<EObj>, IComparable<EObj> {
        public abstract string Category { get; }
        public abstract Type DefType { get; }

        public virtual bool Filter(Def def) {
            return true;
        }

        public string defName;

        public virtual void Crawl(Def baseDef) {
            Crawler.Crawl(this, baseDef);
        }

        public void Save(string path) {
            //File.WriteAllText(path, JsonUtility.ToJson(this, true));
            StringBuilder sb = new StringBuilder();
            sb.Append($"const {this.defName}: {this.GetType().Name} = ");
            sb.AppendLine(this.ToTypeSrcipt());
            sb.AppendLine();
            sb.AppendLine($"export default {this.defName};");
            File.WriteAllText(path, sb.ToString());
        }

        public bool Equals(EObj other) {
            return this.DefType == other.DefType && this.defName == other.defName;
        }

        public int CompareTo(EObj other) {
            return this.defName.CompareTo(other.defName);
        }
    }

    abstract class EData : EObj {
        public virtual EAggr GetAggregation() {
            return null;
        }
    }

    abstract class ELang : EObj {

        public string label;
        public string description;

        public override void Crawl(Def def) {
            base.Crawl(def);
        }
    }
}
