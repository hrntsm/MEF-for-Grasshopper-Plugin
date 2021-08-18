using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace GHPluginLoader
{
    public class GHPluginLoaderInfo : GH_AssemblyInfo
    {
        public override string Name => "GHPluginLoader Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("3A44A8B8-4E0D-4F9F-B400-AF23C41EC4A5");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}
