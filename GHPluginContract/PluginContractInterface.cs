using Rhino.Geometry;

namespace GHPluginContractInterface
{
    public interface IPlugin
    {
        Line RunMakeLine(Point3d to, Point3d from);
        string RunMakeText();
    }
}
