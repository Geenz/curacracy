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
    public class Authentication {
        public static async Task<LoginResponse> LoginRequest(string email, string password, bool rememberMe) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("email", email));
                formValues.Add(new KeyValuePair<string, string>("password", password));
                formValues.Add(new KeyValuePair<string, string>("rememberMe", rememberMe.ToString()));
                
                var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/login", new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    LoginResponse u = JsonConvert.DeserializeObject<LoginResponse>(readStream.ReadToEnd());
                    return u;
                }
            }
            return null;
        }
        
        public static async Task<ValidationResponse> ValidateTokenRequest(string token, int userid) {
            if (userid > 0) {
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var formValues = new List<KeyValuePair<string, string>>();
                    formValues.Add(new KeyValuePair<string, string>("userId", userid.ToString()));
                    formValues.Add(new KeyValuePair<string, string>("token", token));
                    
                    
                    var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/validateToken", new FormUrlEncodedContent(formValues));
                    
                    if (response.IsSuccessStatusCode) {
                        Stream receiveStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                        ValidationResponse t = JsonConvert.DeserializeObject<ValidationResponse>(readStream.ReadToEnd());
                        return t;
                    }
                }
            }
            return null;
        }
                
        public static async Task<ValidationResponse> InvalidateTokenRequest(string token, int userid) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("userId", userid.ToString()));
                formValues.Add(new KeyValuePair<string, string>("token", token));
                
                var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/invalidateToken", new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    ValidationResponse t = JsonConvert.DeserializeObject<ValidationResponse>(readStream.ReadToEnd());
                    return t;
                }
            }
            return null;
        }
        
        public static async Task<GenericReponse> RegisterUserRequest(string username, string email, string password, DateTime birthdate) {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("username", username));
                formValues.Add(new KeyValuePair<string, string>("email", email));
                formValues.Add(new KeyValuePair<string, string>("password", password));
                formValues.Add(new KeyValuePair<string, string>("birthdate", birthdate.ToString()));
                
                var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/register", new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    GenericReponse t = JsonConvert.DeserializeObject<GenericReponse>(readStream.ReadToEnd());
                    return t;
                }
            }
            return null;
        }
	}
}
