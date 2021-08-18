using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using GHPluginContractInterface;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace GHPluginLoader
{
    public class GHPluginLoaderComponent : GH_Component
    {
        [Import(typeof(IPlugin))]
        public IPlugin plugin;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GHPluginLoaderComponent()
          : base("GHPluginLoader", "PluginLoader",
            "Description of component",
            "Category", "Subcategory")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("To", "To", "Line point To", GH_ParamAccess.item);
            pManager.AddPointParameter("From", "From", "Line point From", GH_ParamAccess.item);
            pManager.AddTextParameter("dll path", "path", "Set dll path", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "Line", "Line", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "Text", "Text", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d ptTo = new Point3d();
            Point3d ptFrom = new Point3d();
            string pluginName = string.Empty;

            if (!DA.GetData(0, ref ptTo)) { return; }
            if (!DA.GetData(1, ref ptFrom)) { return; }
            if (!DA.GetData(2, ref pluginName)) { return; }

            if (string.IsNullOrEmpty(pluginName))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please set the plugin name");
            }
            if (!System.IO.File.Exists(pluginName))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File not exist.");
            }

            // ----
            // MEF
            // ----
            var catalog = new AggregateCatalog(
              new AssemblyCatalog(Assembly.Load(System.IO.File.ReadAllBytes(pluginName)))
            );
            var container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                throw compositionException;
            }

            Line line = plugin.RunMakeLine(ptTo, ptFrom);
            string txt = plugin.RunMakeText();
            catalog.Dispose();
            container.Dispose();

            DA.SetData(0, line);
            DA.SetData(1, txt);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1bdc7d1a-f9a2-458b-a9b5-b5b8df0d64aa");
    }
}
