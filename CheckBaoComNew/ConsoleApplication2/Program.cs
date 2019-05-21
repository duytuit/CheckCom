using CheckBaoComNew.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //List<ThucDon> thucdon = new List<ThucDon>();
                // Start a task - calling an async function in this example
              //  List<ThucDon> td = new List<ThucDon>();
                Task<string> callTask = Task.Run(() => CallHttp());
                // Wait for it to finish
                callTask.Wait();
                // Get the result
                string astr = callTask.Result;
                List<ThucDon> thucdon = JsonConvert.DeserializeObject<List<ThucDon>>(astr);
                string td = thucdon.Where(t=>t.buaanid==1).ToString();
                // Write it our
                Console.WriteLine(td);
                Console.ReadLine();
            }
            catch (Exception ex)  //Exceptions here or in the function will be caught here
            {
                Console.WriteLine("Exception: " + ex.Message);
            }


        }

        // Simple async function returning a string...
        static private async Task<string> CallHttp()
        {
            // Just a demo.  Normally my HttpClient is global (see docs)
            HttpClient aClient = new HttpClient();
            // async function call we want to wait on, so wait
            string astr = await aClient.GetStringAsync("http://localhost:3000/ThucDon");
            // return the value
            return astr;
        }
    }
}
