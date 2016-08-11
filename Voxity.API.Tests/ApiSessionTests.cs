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

    [TestClass]
    public class UtilsFilterTests
    {
        [TestMethod]
        public void Utils_ValidPhone_Valid()
        {
            Voxity.API.Utils.Filter.ValidPhone("+33612345678");
        }
    }
}
