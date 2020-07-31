using MIMHelper.Cmdlets;
using System.Management.Automation;
using Xunit;
using System.Linq;

namespace MIMHelperTests
{
    public class GetExpiredADUsersCmdletTests
    {
        [Fact]
        public void ShouldThrowCommandNotFoundException()
        {
            var sut = new GetExpiredADUsersCmdlet();
            Assert.Throws<CommandNotFoundException>(() => sut.Invoke().Cast<object>().ToList());
        }
    }
}
