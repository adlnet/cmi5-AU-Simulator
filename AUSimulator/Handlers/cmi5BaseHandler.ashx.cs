﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using AUSimulator.Classes;
using Newtonsoft.Json.Linq;
using TinCan;

namespace AUSimulator.Handlers
{
    public class cmi5BaseHandler : IHttpHandler
    {
        protected string endPoint { get; set; }
        protected Activity activity { get; set; }
        protected string actorJSON { get; set; }
        protected string authToken { get; set; }
        protected Guid registration { get; set; }
        protected Agent actor { get; set; }

        private static Activity cmi5Object(string ActivityID)
        {
            return new Activity
            {
                id = new Uri(ActivityID)
            };
        }

        protected static Activity cmi5ContextActivity()
        {
            return new Activity
            {
                id = new Uri("http://purl.org/xapi/cmi5/context/categories/cmi5")
            };
        }

        protected RemoteLRS GetLRS()
        {
            return  new RemoteLRS
            {
                auth = authToken,
                endpoint = new Uri(endPoint),
                version = TCAPIVersion.V100
            };
        }

        public virtual void ProcessRequest(HttpContext context)
        {
        }

        protected void SetCommonParms(NameValueCollection queryString)
        {
            endPoint = queryString["endpoint"];
            activity = ( queryString["activityId"] != null ? cmi5Object(queryString["activityId"]) : new Activity());
            actorJSON = queryString["actor"];
            authToken = "Basic " + Utilities.Base64Encoder(queryString["token"] + ":CMI5");
            if (queryString["registration"] != null)
                registration = new Guid(queryString["registration"]);

            var actorObject = JObject.Parse(actorJSON);
            actor = new Agent(actorObject);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}