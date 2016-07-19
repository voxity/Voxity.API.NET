using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Voxity.API.Tests
{
    [TestClass]
    public class EndpointsUsersTests
    {
        [TestMethod]
        public void Users_WhoAmI_Valid()
        {
            var session = new Fakes.StubIApiSession();
        }
    }

}
