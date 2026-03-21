using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace SigBe_lærer_koding
{
    public class GH_BIM360 : GH_Component
    {
        /// <summary>
        /// Sender epost til predefinerte mottakere
        /// </summary>
        public GH_BIM360()
          : base("Bestill BIM 360", "BIM 360",
              "Bestiller BIM 360 til prosjektnummeret i input prosjektnummer. Trenger et prosjektnummer og en knapp for å virke.",
              "NO", "test")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("send epost", "send epost(button)", "sender epost om BIM 360", GH_ParamAccess.item);
            pManager.AddIntegerParameter("prosjektnummer", "prosjektnummer(int)", "nummer opå prosjektet det gjelder", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("status", "status", "status", GH_ParamAccess.item);
        }
        Counter counter = new Counter();
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool sendepost = false;
            DA.GetData(0, ref sendepost);
            int prosjektnummer=0;
            DA.GetData(1, ref prosjektnummer);
            string output = "Trykk på BUTTON for å sende bestilling.";


            if (sendepost == true ) 

            {

                if (counter.count < 1)

                {
                    try
                    {
                        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; //NORCONSULTAD\sigbe
                        userName = userName.Split('\\')[1]; //NORCONSULTAD , sigbe
                        string combinedFromAdress = userName + "@norconsult.no";

                        SmtpClient smtpClient = new SmtpClient();
                        //NetworkCredential basicCredential = new NetworkCredential("sigurd.gjesti.berge@norconsult.com", "snjfmq22020", "smtp.norconsult.com");
                        MailMessage message = new MailMessage();
                        MailAddress fromAddress = new MailAddress(combinedFromAdress);

                        // setup up the host, increase the timeout to 5 minutes
                        smtpClient.Host = "smtp.norconsult.no";
                        smtpClient.UseDefaultCredentials = true;
                        //smtpClient.Credentials = basicCredential;
                        smtpClient.Timeout = (60 * 5 * 1000);

                        message.From = fromAddress;
                        message.Subject = "bim360";
                        message.IsBodyHtml = true;
                        message.Body = "<h3>Test: Dette er en automatisk generert mail fra PI. Bestiller herved bim360 på prosjekt " + prosjektnummer + "</h3>";
                        message.To.Add("sigurd.gjesti.berge@norconsult.com");

                        smtpClient.Send(message);
                        counter.count++;

                        
                    }
                    catch (Exception ex)
                    {
                        output = ex.ToString();
                    }
                    output =  counter.count.ToString();
                }
                
            }
            else
            {
                output="Du har ikke sendt forespørsel.";
            }
            if (counter.count > 0) output = "Forespørsel er sendt.";
            DA.SetData(0, output);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.b360_sml;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9a03a3d2-f509-48cf-b8ec-d85950270648"); }
        }
    }
}