using MIMHelper.Cmdlets;
using System.Management.Automation;
using Xunit;
using System.Linq;

namespace MIMHelperTests
{
    public class GetMetaverseObjectsCmdletTests
    {
        [Fact]
        public void ShouldThrowCommandNotFoundException()
        {
            var sut = new GetMetaverseObjectsCmdlet();
            Assert.Throws<CommandNotFoundException>(() => sut.Invoke().Cast<object>().ToList());
        }
    }
}
