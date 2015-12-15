using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using CuracracyAPI.Models;

namespace CuracracyAPI.Client {
    public class User : ClientBase {
        public static async Task<ICollection<UserMeta>> GetUsers(int page, int count) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("count", count.ToString()));
                
                var response = await client.PostAsync(BASE_URI + "api/v1/user/page/" + page.ToString(), new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    var u = JsonConvert.DeserializeObject<ICollection<UserMeta>>(readStream.ReadToEnd());
                    return u;
                }
            }
            return null;
        }
		
		public static async Task<ICollection<UserMeta>> GetUsers(int page) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var response = await client.GetAsync(BASE_URI + "api/v1/user/page/" + page.ToString());
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    var u = JsonConvert.DeserializeObject<ICollection<UserMeta>>(readStream.ReadToEnd());
                    return u;
                }
            }
            return null;
        }
		
		public static async Task<UserResponse> GetUser(int userid, LoginResponse token) {
			using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("userid", token.userid.ToString()));
                formValues.Add(new KeyValuePair<string, string>("token", token.token));
                
                var response = await client.PostAsync(BASE_URI + "api/v1/user/" + userid.ToString(), new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    string responseBody = await readStream.ReadToEndAsync();
                    var u = JsonConvert.DeserializeObject<UserResponse>(responseBody);
                    return u;
                }
            }
            return null;
		}
        
        public static async Task<UserResponse> GetUser(int userid) {
			using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var response = await client.GetAsync(BASE_URI + "api/v1/user/" + userid.ToString());
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    string responseBody = await readStream.ReadToEndAsync();
                    var u = JsonConvert.DeserializeObject<UserResponse>(responseBody);
                    return u;
                }
            }
            return null;
		}
	}
}
