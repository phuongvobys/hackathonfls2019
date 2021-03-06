// Alice.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AliceInventory
{
    // Alice.cs
    public class Alice : Controller 
    {
        static void Main(string[] args) => CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(srv => srv.AddMvc())
            .Configure(app => app.UseMvc());

        [HttpPost("/alice")]
        public AliceResponse WebHook([FromBody] AliceRequest req) =>
        req.Reply(GetAliceReply(req.Request.OriginalUtterance, req.Session.UserId));

        private string GetAliceReply(string input, string clientId)
        {
            ChatResponse response = sessionManager.GetSession(clientId).ProcessInput(input);
           return $"{response.TextResponse}\n[VOICE:] {response.VoiceResponse}";
        }

        private static SessionManager sessionManager=new SessionManager();

        [HttpGet("/alice/hello")]

        //public AliceResponse HelloHook([FromBody] AliceRequest req) => req.Reply("Привет");
        public ActionResult<string> Get()
        {
           var input="add 3 cats";
            var dice=System.DateTime.Now.Second % 5;
            switch(dice)
            {
                case 0:input="add 2 tigers";
                    break;
                case 1:input="add 5 кг свинца";
                    break;
                case 2:input="добавить 10 грамм свинца";
                    break;
                case 3:input="add 5 метров кабеля";
                    break;
                case 4:input="add 50 см кабеля";
                    break;
                default:input="add cats";
                    break;
            }
            sessionManager.GetSession("").ProcessInput(input);
            var response=sessionManager.GetSession("").ProcessInput("list").TextResponse;
            return $"{System.DateTime.Now.ToLongTimeString()} {response}!";


            return $"Hello! {response}";
        }
        
        public class GoogleQuery
        {
            public GoogleQueryResult queryResult {get;set;}
        }
        public class GoogleQueryResult
        {
            public string queryText {get;set;}
        }

        [HttpPost("/google")]
        public JsonResult GetGoogleResponse([FromBody] GoogleQuery json) 
        { 
            var req=json.queryResult.queryText;
            var response= new GoolgeResponse();
            var resp=sessionManager.GetSession("clientId").ProcessInput(req).TextResponse;
            response.fulfillmentText = $"{resp}!";
            return new JsonResult(response);
        }
    }
}
