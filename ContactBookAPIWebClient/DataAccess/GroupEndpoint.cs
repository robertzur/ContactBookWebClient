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
    public class GroupEndpoint
    {
        private HttpClient client;

        public List<Group> GetGroups(int page, int pageSize, UserData userData)
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);

                var response = client.GetStringAsync(string.Format("contact/{0}/{1}/{2}", page, pageSize, "true")).Result;

                var result = JsonConvert.DeserializeObject<List<Group>>(response);

                return result;

            }
        }

        public List<Group> GetFilteredGroups(string searchScope, string searchQuery, int page, int pageSize, UserData userData)
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);

                var response = client.GetStringAsync(string.Format("contact/{0}/{1}/{2}/{3}/{4}", searchScope, searchQuery, page, pageSize, "true")).Result;

                var result = JsonConvert.DeserializeObject<List<Group>>(response);

                return result;

            }
        }

        public string CreateGroup(Group model, UserData userData)
        {
            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("name", model.name));
                postData.Add(new KeyValuePair<string, string>("description", model.description));
                postData.Add(new KeyValuePair<string, string>("isContactGroup", "true"));
                postData.Add(new KeyValuePair<string, string>("tags", model.tags[0]));
                if (!string.IsNullOrWhiteSpace(model.parentId))
                {
                    postData.Add(new KeyValuePair<string, string>("parentId", model.parentId));
                }


                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.Add("Timestamp", userData.Timestamp.ToString());
                content.Headers.Add("Digest", userData.AuthenticationHash);
                content.Headers.Add("Public-Key", userData.PublicKey);


                client.PostAsync("http://localhost:3000/contact", content)
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

        public string UpdateGroup(Group model, UserData userData)
        {
            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("name", model.name));
                postData.Add(new KeyValuePair<string, string>("description", model.description));
                postData.Add(new KeyValuePair<string, string>("isContactGroup", "true"));
                postData.Add(new KeyValuePair<string, string>("tags", model.tags[0]));

                if (!string.IsNullOrWhiteSpace(model.parentId))
                {
                    postData.Add(new KeyValuePair<string, string>("parentId", model.parentId));
                }


                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.Add("Timestamp", userData.Timestamp.ToString());
                content.Headers.Add("Digest", userData.AuthenticationHash);
                content.Headers.Add("Public-Key", userData.PublicKey);


                client.PostAsync("http://localhost:3000/contact/" + model._id, content)
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

        public string DeleteGroup(string id, UserData userData)
        {
            try
            {
                client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);


                client.DeleteAsync("http://localhost:3000/contact/" + id)
                    .ContinueWith(deleteTask =>
                    {
                        deleteTask.Result.EnsureSuccessStatusCode();
                        var result = deleteTask.Result.Content.ReadAsStringAsync();
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

        public GroupViewModel GetGroup(string id, UserData userData)
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);

                var response = client.GetStringAsync(string.Format("contact/{0}", id)).Result;

                var result = JsonConvert.DeserializeObject<GroupViewModel>(response);

                return result;

            }
        }
    }
}