using System;
using System.Net;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class SchemeController : Controller
    {
        // GET: Scheme
        //[Authorize]
        public ActionResult Index()
        {
            string name = Request.QueryString["name"];
            string plc = Request.QueryString["plc"];
            foreach (String key in Session.Keys) {
                if (key.Contains(name+plc)) {
                    ViewBag.url = Session[key];
                }
            }

            Session["SchemeURLImage"] = ViewBag.url;
            ViewBag.name = name;
            return View();
        }

        public void getImage() {
            if (Session["SchemeURLImage"] != null)
            {
                try {
                    string url = Session["SchemeURLImage"].ToString();

                    WebClient client = new WebClient();
                    byte[] data = client.DownloadData(url);
                    Response.BinaryWrite(data);
                    Response.ContentType = "image/png";
                }
                catch (Exception e){ }
            }
        }
    }
}