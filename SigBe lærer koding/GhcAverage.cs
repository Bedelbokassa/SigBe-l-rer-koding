using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace SigBe_lærer_koding
{
    public class GhcAverage : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GhcAverage class.
        /// </summary>
        public GhcAverage()
          : base("Average of 2 numbers", "Avg",
              "Regner ut gjennomsnittet av to tall",
              "NO", "VisualBasic")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("First Number", "First", "The first number", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Second Number", "Second", "The second number", GH_ParamAccess.item, 0.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Average", "Average", "Average of two inputs", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double a = double.NaN;
            double b = double.NaN;

            DA.GetData(0, ref a);
            DA.GetData(1, ref b);

            double average = 0.5 * (a + b);

            DA.SetData(0, average);
                }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.average_of_two_numbers;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("35c18619-a27a-4745-8cea-871388927172"); }
        }
    }
}