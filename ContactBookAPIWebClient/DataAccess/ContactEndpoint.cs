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
    public class ContactEndpoint
    {
        private HttpClient client;

        public List<Contact> GetContacts(int page, int pageSize, UserData userData)
        {
            using (client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Timestamp", userData.Timestamp.ToString());
                client.DefaultRequestHeaders.Add("Digest", userData.AuthenticationHash);
                client.DefaultRequestHeaders.Add("Public-Key", userData.PublicKey);

                var response = client.GetStringAsync(string.Format("contact/{0}/{1}",page,pageSize)).Result;

                var result = JsonConvert.DeserializeObject<List<Contact>>(response);

                return result;
               
            }
        }
        
        public string CreateContact(Contact model, UserData userData)
        {
            string tagsString = string.Empty;

            if (model.tags != null && model.tags.Count() > 0)
            {
                for(int i = 0; i< model.tags.Count(); i++)
                {
                    tagsString = string.Concat(tagsString, model.tags[i], (i == model.tags.Count()-1 ? "" : ";"));
                }
            }

            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("name", model.name));
                postData.Add(new KeyValuePair<string, string>("description", model.description));
                postData.Add(new KeyValuePair<string, string>("addressLine1", model.addressLine1));
                postData.Add(new KeyValuePair<string, string>("addressLine2", model.addressLine2));
                postData.Add(new KeyValuePair<string, string>("phoneNumber", model.phoneNumber));
                postData.Add(new KeyValuePair<string, string>("cellNumber", model.cellNumber));
                postData.Add(new KeyValuePair<string, string>("email", model.email));
                postData.Add(new KeyValuePair<string, string>("skypeId", model.skypeId));
                postData.Add(new KeyValuePair<string, string>("twitter", model.twitter));
                postData.Add(new KeyValuePair<string, string>("facebook", model.facebook));
                postData.Add(new KeyValuePair<string, string>("isContactGroup", (model.isContactGroup ? "true" : "false")));
                postData.Add(new KeyValuePair<string, string>("tags", tagsString));
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

        public string UpdateContact(Contact model, UserData userData)
        {
            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("name", model.name));
                postData.Add(new KeyValuePair<string, string>("description", model.description));
                postData.Add(new KeyValuePair<string, string>("addressLine1", model.addressLine1));
                postData.Add(new KeyValuePair<string, string>("addressLine2", model.addressLine2));
                postData.Add(new KeyValuePair<string, string>("phoneNumber", model.phoneNumber));
                postData.Add(new KeyValuePair<string, string>("cellNumber", model.cellNumber));
                postData.Add(new KeyValuePair<string, string>("email", model.email));
                postData.Add(new KeyValuePair<string, string>("skypeId", model.skypeId));
                postData.Add(new KeyValuePair<string, string>("twitter", model.twitter));
                postData.Add(new KeyValuePair<string, string>("facebook", model.facebook));
                postData.Add(new KeyValuePair<string, string>("isContactGroup", (model.isContactGroup ? "true" : "false")));
                if (!string.IsNullOrWhiteSpace(model.parentId))
                {
                    postData.Add(new KeyValuePair<string, string>("parentId", model.parentId));
                }


                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                content.Headers.Add("Timestamp", userData.Timestamp.ToString());
                content.Headers.Add("Digest", userData.AuthenticationHash);
                content.Headers.Add("Public-Key", userData.PublicKey);


                client.PostAsync("http://localhost:3000/contact/"+model._id, content)
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

        public string DeleteContact(string id, UserData userData)
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

        public Contact GetContact(string id, UserData userData)
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

                var result = JsonConvert.DeserializeObject<Contact>(response);

                return result;

            }
        }
    }
}