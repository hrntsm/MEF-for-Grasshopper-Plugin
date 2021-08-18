using System.ComponentModel.Composition;
using GHPluginContractInterface;
using Rhino.Geometry;

namespace GHPluginMakeLine
{
    [Export(typeof(IPlugin))]
    public class CodeEjector : IPlugin
    {
        public Line RunMakeLine(Point3d to, Point3d from)
        {
            return new Line(from, to);
        }

        public string RunMakeText()
        {
            return "MEF はとてもべんり！！！";
        }
    }
}
