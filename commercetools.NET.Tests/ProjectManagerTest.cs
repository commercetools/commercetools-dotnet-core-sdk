using System.Threading.Tasks;

using commercetools.Common;
using commercetools.Project;

using NUnit.Framework;

namespace commercetools.Tests
{
    /// <summary>
    /// Test the API methods in the ProjectManager class.
    /// </summary>
    [TestFixture]
    public class ProjectManagerTest
    {
        private Client _client;

        /// <summary>
        /// Test setup
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            _client = new Client(Helper.GetConfiguration());
        }

        /// <summary>
        /// Test teardown
        /// </summary>
        [OneTimeTearDown]
        public void Dispose()
        {
        }

        /// <summary>
        /// Tests the ProjectManager.GetProjectAsync method.
        /// </summary>
        /// <see cref="ProjectManager.GetProjectAsync"/>
        [Test]
        public async Task ShouldGetProjectAsync()
        {
            Response<Project.Project> response = await _client.Project().GetProjectAsync();
            Assert.IsTrue(response.Success);

            Project.Project project = response.Result;
            Assert.NotNull(project.Key);
            Assert.NotNull(project.Name);
            Assert.NotNull(project.CreatedAt);
        }
    }
}
