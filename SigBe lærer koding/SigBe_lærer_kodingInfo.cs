using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace SigBe_lærer_koding
{
    public class SigBe_lærer_kodingInfo : GH_AssemblyInfo
  {
    public override string Name
    {
        get
        {
            return "SigBelrerkoding";
        }
    }
    public override Bitmap Icon
    {
        get
        {
            //Return a 24x24 pixel bitmap to represent this GHA library.
            return null;
        }
    }
    public override string Description
    {
        get
        {
            //Return a short string describing the purpose of this GHA library.
            return "";
        }
    }
    public override Guid Id
    {
        get
        {
            return new Guid("e7fb9d6e-8874-4f76-9d1d-91b5024ad0c1");
        }
    }

    public override string AuthorName
    {
        get
        {
            //Return a string identifying you or your company.
            return "";
        }
    }
    public override string AuthorContact
    {
        get
        {
            //Return a string representing your preferred contact details.
            return "";
        }
    }
}
}
