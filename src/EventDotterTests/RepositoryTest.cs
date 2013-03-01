using DotVoter;
using Xunit;

namespace DotVoterTests
{
    public class RepositoryTest
    {
        [Fact]
        public void MyTest()
        {
            var repo = new EventRepoTest();

            repo.Test();


            Assert.Equal(4, 2 + 2);
        }
    }
}
