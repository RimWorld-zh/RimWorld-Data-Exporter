using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test {
    class Test {
        static void Main(string[] args) {
            List<Type> typeList = new List<Type> {
                typeof(string),
                typeof(int),
                typeof(long),
                typeof(double),
                typeof(float),
                typeof(List<>),
                typeof(Dictionary<,>),
            };
            foreach (var type in typeList) {
                Console.WriteLine(type.Name);
            }
            Console.WriteLine(typeof(List<string>) == typeof(List<int>));
        }
    }
}
