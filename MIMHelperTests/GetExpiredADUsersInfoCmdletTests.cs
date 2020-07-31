using MIMHelper.Cmdlets;
using System.Management.Automation;
using Xunit;
using System.Linq;

namespace MIMHelperTests
{
    public class GetExpiredADUsersInfoCmdletTests
    {
        [Fact]
        public void ShouldThrowCommandNotFoundException()
        {
            var sut = new GetExpiredADUsersInfoCmdlet();
            Assert.Throws<CommandNotFoundException>(() => sut.Invoke().Cast<object>().ToList());
        }
    }
}
