using IdentityModel.Client;
using System;

namespace ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var tokenClient = new TokenClient("http://localhost:5005/connect/token", "ro.client", "secret");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").GetAwaiter().GetResult();
            Console.WriteLine("Error : {0}", tokenResponse.Error);
            Console.WriteLine("Token : {0}", tokenResponse.AccessToken);
            Console.Read();
        }
    }
}
