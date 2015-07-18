using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;


namespace ContactBookAPIWebClient.DataAccess
{
    public class UserEndpoint
    {
        private HttpClient client;
        public void Register(string username)
        {
            try
            {
                client = new HttpClient();
                var postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("email", username));

                HttpContent content = new FormUrlEncodedContent(postData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                client.PostAsync("http://localhost:3000/User", content)
                    .ContinueWith(postTask =>
                    {
                        postTask.Result.EnsureSuccessStatusCode();
                        client.Dispose();
                    });

            }
            catch (Exception ex)
            {
                
 
            }


        }
    }
}