using RestSharpServices;
using NUnit.Framework.Internal;
using RestSharpServices.Models;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static int lastCreatedIssueNumber;
        private static long lastCreatedCommentId;

        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov", "StefRuseva88", "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");
            repo = "test-nakov-repo";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            var issues = client.GetAllIssues(repo);

            Assert.That(issues, Has.Count.GreaterThan(0), "There should be more than one issue.");

            foreach (var issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0.");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            int issueNumber = 1;
            var issue = client.GetIssueByNumber(repo, issueNumber);

            Assert.That(issue, Is.Not.Null, "The response should contain issue data.");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "The issue number should be as requested");
            Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty.");
        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            int issueNumber = 6;
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            Assert.That(labels.Count, Is.GreaterThan(0));

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be greater than 0.");
                Assert.That(label.Name, Is.Not.Empty, "Label name should not be empty.");

                Console.WriteLine("Label: " + label.Id + " - Name: " + label.Name);
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            int issueNumber = 6;
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);

            Assert.That(comments.Count, Is.GreaterThan(0));

            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be greater than 0.");
                Assert.That(comment.Body, Is.Not.Empty, "Comment body should not be empty.");

                Console.WriteLine("Comment: " + comment.Id + " - Body: " + comment.Body);
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            string expectedTitle = "Create Your Own Title";
            string body = "Give Me Some Description";

            var issue = client.CreateIssue(repo, expectedTitle, body);

            lastCreatedIssueNumber = issue.Number;

            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
                Assert.That(issue.Title, Is.EqualTo(expectedTitle));

            });
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            string body = "Body of the new Comment";
            var comment = client.CreateCommentOnGitHubIssue(repo, lastCreatedIssueNumber, body);

            lastCreatedCommentId = comment.Id;

            Assert.That(comment.Body, Is.EqualTo(body));

            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;  
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            Comment comment = client.GetCommentById(repo, lastCreatedCommentId);

            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Id, Is.EqualTo(lastCreatedCommentId), "The retreivet comment ID should match the requested comment ID");
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            string newBody = "This is the updated text of the comment";
            var comment = client.EditCommentOnGitHubIssue(repo, lastCreatedCommentId, newBody);

            Assert.That(comment, Is.Not.Null);
            Assert.That(comment.Id, Is.EqualTo(lastCreatedCommentId), "The updatet comment ID should match the original comment ID");
            Assert.That(comment.Body, Is.EqualTo(newBody), "The updated comment text should match the new body text.");
        }


        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            var result = client.DeleteCommentOnGitHubIssue(repo, lastCreatedCommentId);

            Assert.That(result, Is.True);
        }


    }
}

