using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpServices.Models;

namespace RestSharpServices
{
    public class GitHubApiClient
    {
        private readonly RestClient client;

        public GitHubApiClient(string baseUrl, string username, string token)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(username, token)
            };

            this.client = new RestClient(options);
        }

        public List<Issue>? GetAllIssues(string repo)
        {
            var request = new RestRequest($"{repo}/issues");
            var response = client.Execute(request);

            return response.Content != null
                ? JsonSerializer.Deserialize<List<Issue>>(response.Content)
                : null;
        }

        public Issue? GetIssueByNumber(string repo, int issueNumber)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}", Method.Get);
            var responce = client.Execute(request);
            return responce.Content != null
                ? JsonSerializer.Deserialize<Issue>(responce.Content)
                : null;
        }

        public Issue? CreateIssue(string repo, string title, string body)
        {
            var request = new RestRequest($"{repo}/issues");
            request.AddJsonBody(new { title, body });
            var response = client.Execute(request, Method.Post);
            return response.Content != null
                ? JsonSerializer.Deserialize<Issue>(response.Content)
                : null;
        }

        public List<Label>? GetAllLabelsForIssue(string repo, int issueNumber)
        {

            var request = new RestRequest($"{repo}/issues/{issueNumber}/labels", Method.Get);
            var responce = client.Execute(request);
            return responce.Content != null
                ? JsonSerializer.Deserialize<List<Label>>(responce.Content)
                : null;

        }

        public List<Comment>? GetAllCommentsForIssue(string repo, int issueNumber)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}/comments", Method.Get);
            var responce = client.Execute(request);
            return responce.Content != null
                ? JsonSerializer.Deserialize<List<Comment>>(responce.Content)
                : null;
        }

        public Comment? CreateCommentOnGitHubIssue(string repo, int issueNumber, string body)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}/comments");
            request.AddJsonBody(new { repo, body });
            var response = client.Execute(request, Method.Post);
            return response.Content != null
                ? JsonSerializer.Deserialize<Comment>(response.Content)
                : null;
        }

        public Comment? GetCommentById(string repo, long commentId)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}", Method.Get);
            var response = client.Execute(request);
            return response.Content != null
               ? JsonSerializer.Deserialize<Comment>(response.Content)
               : null;
        }

        public Comment? EditCommentOnGitHubIssue(string repo, long commentId, string newBody)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}", Method.Patch);
            request.AddJsonBody(new { body = newBody });
            var response = client.Execute(request);
            return response.Content != null
               ? JsonSerializer.Deserialize<Comment>(response.Content)
               : null;
        }

        public bool? DeleteCommentOnGitHubIssue(string repo, long commentId)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}", Method.Delete);
            var response = client.Execute(request);
            return response.IsSuccessful;
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
