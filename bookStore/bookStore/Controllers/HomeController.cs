using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Net;
using System.IO;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string Index(ToTranslate toTranslate)
        {
            string text = toTranslate.Text;
            string lang = toTranslate.Lang;
            WebRequest req = WebRequest.Create("https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20150315T085331Z.3aae60473b16efea.8c82912cc2329d14afdfaf470f665ed90b736b6b&text="+text+"&lang="+lang);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            int beg = Out.IndexOf("[")+2;
            int end = Out.IndexOf("]")-2;
            Out = Out.Substring(beg, end-beg+1);
            return Out;
        }
    }

}
