using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    // public class DLPackage
    //{
    //    public string Name;
    //    public string PkgName;
    //    public string NameSpace;
    //    public string ClassName;
    //}
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> Dalpackeges;
        static DalConfig()
        {
            XElement dalconfig = XElement.Load(@"xml\dal-config.xml"/*"C:\Users\allon\source\repos\ShohamElias\dotNet5782_9338_6442\dal-config.xml"*/);
            DalName = dalconfig.Element("dal").Value;
            //Dalpackeges = (from pkg in dalconfig.Element("dl-packages").Elements()
            //              let tmp1 = pkg.Attribute("namespace")
            //              let nameSpace = tmp1 == null ? "Dal" : tmp1.Value
            //              let tmp2 = pkg.Attribute("class")
            //              let className = tmp2 == null ? pkg.Value : tmp2.Value
            //              select new DLPackage()
            //              {
            //                  Name = "" + pkg.Name,
            //                  PkgName = pkg.Value,
            //                  NameSpace = nameSpace,
            //                  ClassName = className
            //              })
            //            .ToDictionary(p => "" + p.Name, p => p);

            Dalpackeges = (from pkg in dalconfig.Element("dal-packages").Elements()
                           select pkg).ToDictionary(p => "" + p.Name, p => p.Value);
            //int x = 12;

            // x += x;
        }
    }


}