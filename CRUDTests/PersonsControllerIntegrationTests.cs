using FluentAssertions;
using Xunit;

namespace CRUDTests;

public class PersonsControllerIntegrationTests
{
    [Fact]
    public void Index_ToReturnView()
    {
       HttpResponseMessage responseMessage = _client.GetAsync("/Persons/Index");


        responseMessage.IsSuccessStatusCode.Should().BeTrue();
    }
}
