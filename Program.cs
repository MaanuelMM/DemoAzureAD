using Azure.Core;
using Azure.Identity;
using Microsoft.Graph;
// using Microsoft.Graph.Auth;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DemoAzureAD
{
    public class Program
    {
        private static readonly string tenantId = "00000000-0000-0000-0000-000000000000";
        private static readonly string clientId = "00000000-0000-0000-0000-000000000000";
        private static readonly string clientSecret = "aaaaa~aaa.aaaaaa_aaaaaaaaaaaaaaaaaaaaaaa";
        private static readonly string authorityHost = "https://login.microsoftonline.com";

        public static void Main(string[] args)
        {
            Console.Clear();
            // OptionOne();
            // Console.WriteLine();
            OptionTwo();
        }

        public static void OptionOne()
        {
            /*
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(clientId)  // ClientID
                .WithClientSecret(clientSecret)  // Secret value of a client
                // .WithAuthority($"https://login.microsoftonline.com/{tenantId}/")  // TenantID as prefix
                .WithTenantId(tenantId)  // defaults to login.microsoftonline.com
                .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(app);

            string[] endpoints = new string[] { "https://graph.microsoft.com/.default" };

            AuthenticationResult token = app.AcquireTokenForClient(endpoints).ExecuteAsync().Result;

            Console.WriteLine(token.AccessToken);

            /////////////////////////////////////////////////////////////////////////////////////////

            string endpoint = "https://graph.microsoft.com/v1.0/users";

            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var response = http.GetAsync(endpoint).Result;

            // string result = http.GetStringAsync(endpoint).Result;

            Console.WriteLine();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"\nResponse: {response}");
                Console.WriteLine();

                OData oData = JsonConvert.DeserializeObject<OData>(data);
                List<User> items = JsonConvert.DeserializeObject<List<User>>(oData.Value.ToString());

                foreach (var item in items)
                {
                    Console.WriteLine($"User: {item.DisplayName}");
                }
            }
            else
            {
                Console.WriteLine($"\nError: {response.StatusCode}");
            }

            Console.WriteLine();

            /////////////////////////////////////////////////////////////////////////////////////////

            var graphClient = new GraphServiceClient(authProvider);

            // var users = graphClient.Users.GetAsync().Result.Value;  // SDK 5.0
            var users = graphClient.Users.Request().GetAsync().Result;

            foreach (var user in users)
            {
                    Console.WriteLine($"User: {user.DisplayName}");
            }

            Console.WriteLine();

            /////////////////////////////////////////////////////////////////////////////////////////

            var ana = graphClient.Users["ana.garcia@formacionesazure.onmicrosoft.com"].Request().GetAsync().Result;
            Console.WriteLine($"User: {ana.DisplayName}");

            ana.City = "Madrid";
            ana.Country = "España";
            ana.PostalCode = "28024";

            var anaUpdate = graphClient.Users["ana.garcia@formacionesazure.onmicrosoft.com"].Request().UpdateAsync(ana).Result;

            /////////////////////////////////////////////////////////////////////////////////////////

            var newUser = new User()
            {
                DisplayName = "Manu Tenorio",
                UserPrincipalName = "manu.tenorio@formacionesazure.onmicrosoft.com",
                AccountEnabled = true,
                PasswordProfile = new PasswordProfile()
                {
                    Password = "M4nuT3n0r10!",
                    ForceChangePasswordNextSignIn = false
                },
                Department = "OT"
            };
             */
        }

        public static void OptionTwo()
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = new Uri(authorityHost)
            };

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var requestContext = new TokenRequestContext(scopes);
            var token = credential.GetToken(requestContext);

            Console.WriteLine(token.Token);

            /////////////////////////////////////////////////////////////////////////////////////////

            string endpoint = "https://graph.microsoft.com/v1.0/users";

            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var response = http.GetAsync(endpoint).Result;

            // string result = http.GetStringAsync(endpoint).Result;

            Console.WriteLine();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"\nResponse: {response}");
                Console.WriteLine();

                OData oData = JsonConvert.DeserializeObject<OData>(data);
                List<User> items = JsonConvert.DeserializeObject<List<User>>(oData.Value.ToString());

                foreach (var item in items)
                {
                    Console.WriteLine($"User: {item.DisplayName}");
                }
            }
            else
            {
                Console.WriteLine($"\nError: {response.StatusCode}");
            }

            Console.WriteLine();

            ////////////////////////////////////////////////////////////////////////////////////////

            var graphClient = new GraphServiceClient(credential);

            var users = graphClient.Users.GetAsync().Result.Value;  // SDK 5.0
            // var users = graphClient.Users.Request().GetAsync().Result;

            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.DisplayName}");
            }

            Console.WriteLine();

            /////////////////////////////////////////////////////////////////////////////////////////

            var ana = graphClient.Users["ana.garcia@formacionesazure.onmicrosoft.com"].GetAsync().Result;
            Console.WriteLine($"User: {ana.DisplayName}");

            ana.City = "Madrid";
            ana.Country = "España";
            ana.PostalCode = "28024";

            var anaUpdate = graphClient.Users["ana.garcia@formacionesazure.onmicrosoft.com"].PatchAsync(ana).Result;

            /////////////////////////////////////////////////////////////////////////////////////////

            /*
            var newUser = new User()
            {
                DisplayName = "Manu Tenorio",
                UserPrincipalName = "manu.tenorio@formacionesazure.onmicrosoft.com",
                AccountEnabled = true,
                PasswordProfile = new PasswordProfile()
                {
                    Password = "M4nuT3n0r10!",
                    ForceChangePasswordNextSignIn = false
                },
                Department = "OT"
            };
             */
        }
    }
}