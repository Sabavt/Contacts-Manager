using FluentAssertions;
using Xunit;

namespace CRUDTests;

public class PersonsControllerIntegrationTests : IClassFixture<MyWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PersonsControllerIntegrationTests(MyWebApplicationFactory myWeb)
    {
       _client = myWeb.CreateClient();
    }

    [Fact]
    public async Task Index_ToReturnView()
    {
       HttpResponseMessage responseMessage = await _client.GetAsync("/Persons/Index");
         
       responseMessage.IsSuccessStatusCode.Should().BeTrue();
    }
}
