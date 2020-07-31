using MIMHelper.Cmdlets;
using Moq;
using System;
using System.Management.Automation;
using Xunit;
using System.Linq;

namespace MIMHelperTests
{
    public class GetEmptyOrganizationUnitsCmdletTests
    {
        [Fact]
        public void ShouldThrowCommandNotFoundException()
        {
            var sut = new GetEmptyOrganizationUnitsCmdlet();
            Assert.Throws<CommandNotFoundException>(() => sut.Invoke().Cast<object>().ToList());
        }
    }
}
