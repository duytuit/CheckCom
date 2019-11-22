using ConsoleApplication4;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

[assembly: OwinStartup(typeof(Program.Startup))]
namespace ConsoleApplication4
{
    class Program
    {
        static IDisposable SignalR;
       
        static void Main(string[] args)
        {
            string url = "http://192.84.100.201";
            SignalR = WebApp.Start<Startup>(url);
            var connection = new HubConnection(url);
            var myHub = connection.CreateHubProxy("MyHub");

            Console.WriteLine("Enter your name");
            string name = Console.ReadLine();

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");

                    myHub.On<string, string>("addMessage", (s1, s2) =>
                    {
                        Console.WriteLine(s1 + ": " + s2);
                    });

                    while (true)
                    {
                        string message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            break;
                        }

                        myHub.Invoke<string>("Send", name, message).ContinueWith(task1 =>
                        {
                            if (task1.IsFaulted)
                            {
                                Console.WriteLine("There was an error calling send: {0}", task1.Exception.GetBaseException());
                            }
                            else
                            {
                                Console.WriteLine(task1.Result);
                            }
                        });
                    }
                }

            }).Wait();

            Console.Read();

        }
        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseCors(CorsOptions.AllowAll);

                /*  CAMEL CASE & JSON DATE FORMATTING
                 use SignalRContractResolver from
                https://stackoverflow.com/questions/30005575/signalr-use-camel-case
                    var hubConfiguration = new HubConfiguration();
          hubConfiguration.EnableDetailedErrors = true;
                var settings = new JsonSerializerSettings()
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };

                settings.ContractResolver = new SignalRContractResolver();
                var serializer = JsonSerializer.Create(settings);

               GlobalHost.DependencyResolver.Register(typeof(JsonSerializer),  () => serializer);                
                */
              //  var hubConfiguration = new HConfiguration();
                //hubConfiguration.EnableDetailedErrors = true;
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }
        [HubName("MyHub")]
        public class MyHub : Hub
        {
            public void Send(string name, string message)
            {
                Clients.All.addMessage(name, message+DateTime.Now.ToShortTimeString());
            }
          
        }
    }
}
