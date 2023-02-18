using System.Text.Json;
using RestSharp;
using System.Net;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://taskboardjs.krismikov.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl); 
        }

        [Test]
        public void Test_Get_Task_Check_Title()
        {
            var request = new RestRequest("tasks/board/done", Method.Get);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));



            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            Assert.That(tasks[0].title, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_GetSearchByKeywordValidResult()
        {
            var request = new RestRequest("tasks/search/home", Method.Get);
            var response = this.client.Execute(request);
           


            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));
        }

        [Test]
        public void Test_GetSearchByKeywordInvalidResult()
        {
            var request = new RestRequest("tasks/search/missing" + DateTime.Now.Ticks, Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.Content, Is.EqualTo("[]"));
        }

        [Test]
        public void Test_Create_TaskMissinTitle()
        {
            var request = new RestRequest("tasks", Method.Post);
            var reqBody = new
            {
                description = "some description",
                board = "Open"
            };
            request.AddBody(reqBody);  
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
        }

        [Test]
        public void Test_Create_TaskValidBody()
        {
            var request = new RestRequest("tasks", Method.Post);
            var reqBody = new
            {
                title = "some title" + DateTime.Now.Ticks,
                description = "some description",
                board = "Open"
            };
            request.AddBody(reqBody);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
           
        }
    }
}