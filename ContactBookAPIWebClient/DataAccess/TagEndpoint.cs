using ContactBookAPIWebClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ContactBookAPIWebClient.DataAccess
{
    public class TagEndpoint
    {
        private HttpClient client;

        public Tag GetTagByName(string name, UserData userData)
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);

                var response = client.GetStringAsync(string.Format("tag/{0}", name)).Result;

                var result = JsonConvert.DeserializeObject<Tag>(response);

                return result;

            }
        }

        public string CreateTag(Tag model, UserData userData)
        {
            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("name", model.name));

                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.Add("Timestamp", userData.Timestamp.ToString());
                content.Headers.Add("Digest", userData.AuthenticationHash);
                content.Headers.Add("Public-Key", userData.PublicKey);


                client.PostAsync("http://localhost:3000/tag", content)
                    .ContinueWith(postTask =>
                    {
                        postTask.Result.EnsureSuccessStatusCode();
                        var result = postTask.Result.Content.ReadAsStringAsync();
                        client.Dispose();
                        return result;

                    });

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return null;
        }
    }
}