using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var data = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
            try
            {
                var result = wc.UploadString(SiteUrl + TokenUrl, data);
                //JObject o1 = JObject.Parse(result);
                //token = o1["access_token"].Value<string>();
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }

        [TestMethod]
        public void Subscribe()
        {
            var Wc = new WebClient();
            Wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var firstdata = "grant_type=password&username=" + "TestUserName" + "&password=" + "TestPassword361";
            var seconddata = "grant_type=password&username=" + "nbIxMaN" + "&password=" + "AlIeN1";

            try
            {
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
                var st1 = SiteUrl + SubscribeUrl + "/Follow/" + 38;
                var st2 = SiteUrl + SubscribeUrl + "/Follow/" + 41;
                var result = FirstWc.UploadString(st1, "");
                result = FirstWc.DownloadString(SiteUrl + "api/Friends/List/my/s");
                dynamic s1 = JsonConvert.DeserializeObject(result);
                result = FirstWc.DownloadString(SiteUrl + "api/Friends/List/38/f");
                dynamic s2 = JsonConvert.DeserializeObject(result);
                //System.Console.WriteLine(s.First);
                foreach (var i in s1)
                {
                    if (i.UserName == "nbIxMaN")
                    {
                        s = true;
                    }
                }
                foreach (var i in s2)
                {
                    if (i.UserName == "TestUserName")
                    {
                        f = true;
                    }
                }
                if (!s)
                {
                    Assert.Fail("In s list");
                }
                if (!f)
                {
                    Assert.Fail("In f list");
                }
            }
            catch (WebException we)
            {
                Assert.Fail(we.Message);
            }
        }

        [TestMethod]
        public void AddEvent()
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
            var time = System.DateTime.Now.ToString("u");
            var data = JsonConvert.SerializeObject(new
            {
                Latitude = 76,
                Longitude = 92,
                Description = "TestEvent",
                EventDate = time,
            });
            try
            {
                var result = Wc.UploadString(SiteUrl + AddEventUrl, data);
                string events = Wc.DownloadString(SiteUrl + GetEventsUrl);
                dynamic eventsList = JsonConvert.DeserializeObject(events);
                foreach (var i in eventsList)
                {
                    if ((i.Latitude == "76") && (i.Longitude == "92") && (i.Description == "TestEvent") && (i.EventDate == time))
                    {
                        s = true;
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
