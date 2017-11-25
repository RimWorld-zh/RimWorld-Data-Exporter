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
            File.WriteAllText(path, this.ToJson());
        }

        public bool Equals(EObj other) {
            return this.DefType == other.DefType && this.defName == other.defName;
        }

        public int CompareTo(EObj other) {
            return this.defName.CompareTo(other.defName);
        }
    }

    abstract class EData : EObj {
    }

    abstract class ELang : EObj {

        public string label;
        public string description;

        public override void Crawl(Def def) {
            base.Crawl(def);
        }
    }
}
