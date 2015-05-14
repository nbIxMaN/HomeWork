using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Translate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ToTranslate toTranslate)
        {
            toTranslate.key = "trnsl.1.1.20150315T085331Z.3aae60473b16efea.8c82912cc2329d14afdfaf470f665ed90b736b6b";
            if (toTranslate.text == null)
            {
                ViewBag.Translated = "Пожалуйста введите текст для перевода";
                return View("Index");
            }
            string Url = "https://translate.yandex.net/api/v1.5/tr.json/translate";
            var obj = JsonConvert.SerializeObject(toTranslate);
            obj = obj.Replace("\":\"", "=");
            obj = obj.Replace("\",\"", "&");
            obj = obj.Substring(2, obj.Length - 4);
            WebRequest req = WebRequest.Create(Url + "?" + obj);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            dynamic translated = JsonConvert.DeserializeObject(Out);
            ViewBag.ToTranslate = toTranslate.text;
            string str = (translated.text).ToString();
            str = str.Replace("[\r\n  \"", "");
            str = str.Replace("\"\r\n]", "");
            ViewBag.Translated = str;
            return View("Index");
        }
    }

}
