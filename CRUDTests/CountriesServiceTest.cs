using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using FluentAssertions;
using Moq;
using RepositoryContracts;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly ICountriesRepository _countriesRepository;
    private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

    public CountriesServiceTest()
    {  
        _countriesService = new CountriesService(null);
    }

    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

        Func<Task> act = async () => await _countriesService.AddCountry(request);

        //Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;

        Func<Task> act = async () => await _countriesService.AddCountry(request);

        //Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddCountry_DublicatedCountryName()
    {
        //Arrange
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

        Func<Task> act = async () => await _countriesService.AddCountry(request1);
        Func<Task> act2 = async () => await _countriesService.AddCountry(request2);

        await act2.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        //Act
        CountryResponse response = await _countriesService.AddCountry(request);
        List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

        response.CountryID.Should().NotBe(Guid.Empty);
        countries_from_GetAllCountries.Should().Contain(response); 
    }

    [Fact] 
    public async Task GetCountryList_EmptyList()
    {
        //Act
        List<CountryResponse> actualCountry = await _countriesService.GetAllCountries();

        //Assert
        actualCountry.Count.Should().Be(0);
    }

    [Fact]
    public async Task GetCountryList_AddFewCountries()
    {
        //Arrange
        List<CountryAddRequest> country_request_list = [new CountryAddRequest() { CountryName = "USA" }, new CountryAddRequest() { CountryName = "Germany" }, new CountryAddRequest() { CountryName = "Belgium" }];
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

        //Assert
        foreach (var country_request in country_request_list)
        {
            countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
        }

        List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

        //Assert
        actualCountryResponseList.Should().BeEquivalentTo(countries_list_from_add_country);
    }

    [Fact]
    public async Task GetCountryByCountryID_NullCountryID()
    {
        //Arrange
        Guid? guid = null;

        //Act
        Func<Task> country_response = async () => await _countriesService.GetCountryByCountryID(guid); 

        await country_response.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GetCountryByCountryID_ValidCountyID()
    {
        //Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "Egypt" };
        CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

        //Act
        CountryResponse? county_response_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryID);

        //Assert
        country_response_from_add.Should().BeEquivalentTo(county_response_from_get);
    }
}
