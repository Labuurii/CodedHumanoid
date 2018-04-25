using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBuildDownloader
{
    [Serializable]
    public class Build
    {
        public int build;
        public BuildLinks links;
    }

    [Serializable]
    public class BuildLinks
    {
        public Link download_primary;
    }

    [Serializable]
    public class Link
    {
        public string method;
        public string href;
        public LinkMeta meta;
    }

    [Serializable]
    public class LinkMeta
    {
        public string type;
    }

    internal class UnityClient
    {
        RestClient client;

        public UnityClient(string token)
        {
            client = new RestClient("https://build-api.cloud.unity3d.com/api/v1/");
            client.AddDefaultHeader("Authorization", "Basic " + token);
        }

        public List<Build> GetLatestSuccessfulBuildOf(string org, string proj, string build)
        {
            var req = new RestRequest("orgs/{orgid}/projects/{projectid}/buildtargets/{buildtargetid}/builds", Method.GET);
            req.AddUrlSegment("orgid", org);
            req.AddUrlSegment("projectid", proj);
            req.AddUrlSegment("buildtargetid", build);
            req.AddParameter("buildStatus", "success");
            req.AddParameter("per_page", "0");
            req.AddParameter("page", 0);

            var res = client.Execute(req);
            return JsonConvert.DeserializeObject<List<Build>>(res.Content);
        }

        public byte[] DownloadGet(string url)
        {
            var req = new RestRequest(url, Method.GET);
            return client.DownloadData(req, true);
        }
    }
}
