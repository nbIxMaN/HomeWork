using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        const string SiteUrl = "http://icreate.azurewebsites.net/";
        const string GetEventsUrl = "api/Events";
        const string TokenUrl = "Token";
        const string RegisterUrl = "api/Account/Register";
        const string AddEventUrl = "api/Events";
        const string AddCommentUrl = "api/eventComments";
        const string SubscribeUrl = "api/Friends";
        private string token = "";
        [TestMethod]
        public void GetEventsList()
        {
            try
            {
                var wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/json");
                wc.Headers.Add("Accept-Charset", "cp1251");
                var result = wc.DownloadString(SiteUrl + GetEventsUrl);
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
                return;
            }
        }
        //[TestMethod]
        //public void Register()
        //{
        //    var wc = new WebClient();
        //    wc.Headers.Add("Content-Type", "application/json");
        //    var body = JsonConvert.SerializeObject(new
        //    {
        //        UserName = "TestUserName1",
        //        Password = "TestPassword361",
        //        ConfirmPassword = "TestPassword361"
        //    });
        //    try
        //    {
        //        wc.UploadString(SiteUrl + RegisterUrl, body);
        //    }
        //    catch (WebException we)
        //    {
        //        Assert.Fail(we.Message);
        //    }
        //}
        [TestMethod]
        public void Login()
        {
            try
            {
                var wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var data = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
                var result = wc.UploadString(SiteUrl + TokenUrl, data);
                //JObject o1 = JObject.Parse(result);
                //token = o1["access_token"].Value<string>();
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }
        private void Check(WebClient FirstWc, string realationship1, WebClient SecondWc, string realationship2)
        {
            bool InTestUser = false;
            bool InnbIxMaN = false;
            var result = FirstWc.DownloadString(SiteUrl + "api/Friends/List/my/" + realationship1);
            dynamic s1 = JsonConvert.DeserializeObject(result);
            result = SecondWc.DownloadString(SiteUrl + "api/Friends/List/my/" + realationship2);
            dynamic s2 = JsonConvert.DeserializeObject(result);
            //System.Console.WriteLine(s.First);
            foreach (var i in s1)
            {
                if (i.UserName == "nbIxMaN")
                {
                    InTestUser = true;
                }
            }
            foreach (var i in s2)
            {
                if (i.UserName == "TestUserName")
                {
                    InnbIxMaN = true;
                }
            }
            if (!InTestUser)
            {
                Assert.Fail("In TestUser list");
            }
            if (!InnbIxMaN)
            {
                Assert.Fail("In nbIxMaN list");
            }
        }
        [TestMethod]
        public void Subscribe()
        {
            try
            {
                var Wc = new WebClient();
                Wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var firstdata = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
                var seconddata = "grant_type=password&username=" + "nbIxMaN" + "&password=" + "AlIeN1";
                bool s = false;
                bool f = false;
                var result1 = Wc.UploadString(SiteUrl + TokenUrl, firstdata);
                JObject o1 = JObject.Parse(result1);
                var token1 = o1["access_token"].Value<string>();
                var result2 = Wc.UploadString(SiteUrl + TokenUrl, seconddata);
                JObject o2 = JObject.Parse(result2);
                var token2 = o2["access_token"].Value<string>();
                var FirstWc = new WebClient();
                FirstWc.Headers.Add("Content-Type", "application/json");
                FirstWc.Headers.Add("Authorization", "Bearer " + token1);
                var SecondWc = new WebClient();
                SecondWc.Headers.Add("Content-Type", "application/json");
                SecondWc.Headers.Add("Authorization", "Bearer " + token2);
                var result = FirstWc.UploadString(SiteUrl + SubscribeUrl + "/Unfollow/" + 38, "");
                result = SecondWc.UploadString(SiteUrl + SubscribeUrl + "/Unfollow/" + 41, "");
                var st1 = SiteUrl + SubscribeUrl + "/Follow/" + 38;
                var st2 = SiteUrl + SubscribeUrl + "/Follow/" + 41;
                result = FirstWc.UploadString(st1, "");
                Check(FirstWc, "s", SecondWc, "f");
                //result = FirstWc.DownloadString(SiteUrl + "api/Friends/List/my/s");
                //dynamic s1 = JsonConvert.DeserializeObject(result);
                //result = FirstWc.DownloadString(SiteUrl + "api/Friends/List/38/f");
                //dynamic s2 = JsonConvert.DeserializeObject(result);
                ////System.Console.WriteLine(s.First);
                //foreach (var i in s1)
                //{
                //    if (i.UserName == "nbIxMaN")
                //    {
                //        s = true;
                //    }
                //}
                //foreach (var i in s2)
                //{
                //    if (i.UserName == "TestUserName")
                //    {
                //        f = true;
                //    }
                //}
                result = SecondWc.UploadString(st2, "");
                Check(FirstWc, "m", SecondWc, "m");
                result = FirstWc.UploadString(SiteUrl + SubscribeUrl + "/Unfollow/" + 38, "");
                Check(FirstWc, "f", SecondWc, "s");
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }

        [TestMethod]
        public void AddEvent()
        {
            try
            {
                bool s = false;
                dynamic ev;
                var Wc = new WebClient();
                Wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var firstdata = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
                var result1 = Wc.UploadString(SiteUrl + TokenUrl, firstdata);
                JObject o1 = JObject.Parse(result1);
                var token = o1["access_token"].Value<string>();
                Wc = new WebClient();
                Wc.Encoding = Encoding.UTF8;
                Wc.Headers.Add("Content-Type", "application/json");
                Wc.Headers.Add("Authorization", "Bearer " + token);
                var time = System.DateTime.Now.ToString("u");
                var data = JsonConvert.SerializeObject(new
                {
                    Latitude = 76,
                    Longitude = 92,
                    Description = "TestEvent",
                    EventDate = time,
                });
                var result = Wc.UploadString(SiteUrl + AddEventUrl, data);
                string events = Wc.DownloadString(SiteUrl + GetEventsUrl);
                dynamic eventsList = JsonConvert.DeserializeObject(events);
                foreach (var i in eventsList)
                {
                    if ((i.Latitude == "76") && (i.Longitude == "92") && (i.Description == "TestEvent") && (i.EventDate == time))
                    {
                        s = true;
                        ev = i;
                    }
                }
                if (!s)
                {
                    Assert.Fail("Event not added");
                }
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }

        [TestMethod]
        public void AddEventWithPhoto()
        {
            bool s = false;
            dynamic ev;
            var Wc = new WebClient();
            Wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var firstdata = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
            var result1 = Wc.UploadString(SiteUrl + TokenUrl, firstdata);
            JObject o1 = JObject.Parse(result1);
            var token = o1["access_token"].Value<string>();
            Wc = new WebClient();
            Wc.Encoding = Encoding.UTF8;
            Wc.Headers.Add("Content-Type", "application/json");
            Wc.Headers.Add("Authorization", "Bearer " + token);
            string sdata;
            var photoIds = new string[0];
            var url = new WebClient().DownloadString(new Uri(SiteUrl + "api/Endpoints/GetUploadUrl/AddEvent"));
            url = url.Trim(new char[] { '\"' });
            url = url.TrimStart(new char[] { '/' });
            using (var client = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                var fs = new FileStream("C:\\Users\\Александр\\Desktop\\kompot.jpg", FileMode.Open);
                form.Add(new StreamContent(fs), "file", "file.jpg");
                fs = new FileStream("C:\\Users\\Александр\\Desktop\\qRISsLk7j3E.jpg", FileMode.Open);
                form.Add(new StreamContent(fs), "file", "file.jpg");
                var response = client.PostAsync(SiteUrl + url, form).Result;
                sdata = response.Content.ReadAsStringAsync().Result;
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var content = new StringContent(sdata, Encoding.UTF8, "application/json");
                var response = client.PostAsync(SiteUrl
                                        + "api/Endpoints/SaveUploadedFile/AddEvent", content).Result;
                sdata = response.Content.ReadAsStringAsync().Result;
                photoIds = JArray.Parse(sdata).Select(tok => (tok as JObject)["Id"].Value<string>()).ToArray();
                /*EventsListView.Items.AddRange(JArray.Parse(result).Select(tok =>
                {
                    var descr = (tok as JObject)["Description"].Value<string>();
                    var id = (tok as JObject)["EventId"].Value<int>();
                    return new ListViewItem { Text = descr, Tag = id };
                }).ToArray());*/
            }
            try
            {
                var time = System.DateTime.Now.ToString("u");
                //var descr = MsgBox2.Text.Select(c => string.Format(@"\u{0:x4}", (int)c)).Aggregate("", (a, b) => a + b);
                var data = JsonConvert.SerializeObject(new
                {
                    Latitude = 76,
                    Longitude = 92,
                    Description = "TestEvent",
                    EventDate = time,
                    PhotoIds = photoIds
                });
                //data = data.Replace(@"\\", @"\");
                var result = Wc.UploadString(SiteUrl + AddEventUrl, data);
            }
            catch(WebException we)
            {
                Assert.Fail(we.Message);
            }
        }

        [TestMethod]
        public void AddComets()
        {
            try
            {
                bool s = false;
                var Wc = new WebClient();
                Wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                var firstdata = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
                var result1 = Wc.UploadString(SiteUrl + TokenUrl, firstdata);
                JObject o1 = JObject.Parse(result1);
                var token = o1["access_token"].Value<string>();
                Wc = new WebClient();
                Wc.Encoding = Encoding.UTF8;
                Wc.Headers.Add("Content-Type", "application/json");
                Wc.Headers.Add("Authorization", "Bearer " + token);
                string events = Wc.DownloadString(SiteUrl + GetEventsUrl);
                dynamic eventsList = JsonConvert.DeserializeObject(events);
                var Id = eventsList.First.EventId;
                var data = JsonConvert.SerializeObject(new
                {
                    Text = "TextComment",
                    EntityId = Id.ToString()
                });
                var result = Wc.UploadString(SiteUrl + AddCommentUrl, data);
                dynamic comment = JsonConvert.DeserializeObject(result);
                var CommentId = comment.CommentId;
                eventsList = JsonConvert.DeserializeObject(result);
                foreach (var i in eventsList)
                {
                    if (i.EntityId == Id)
                    {
                        foreach (var j in i.LastComments)
                        {
                            var a = j.CommentId;
                            var b = j.Text;
                            if ((j.Text == "TextComments") && (j.CommentId == CommentId))
                            {
                                s = true;
                            }
                        }
                    }
                }
                if (!s)
                {
                    Assert.Fail("Comments not added");
                }
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }
    }
}
