using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService();
    }

    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        //Arrange
        CountryRequest? request = new CountryRequest() { CountryName = null };

        //Assert
        Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryRequest? request = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_DublicatedCountryName()
    {
        //Arrange
        CountryRequest? request1 = new CountryRequest() { CountryName = "USA" };
        CountryRequest? request2 = new CountryRequest() { CountryName = "USA" };

        //Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);

        });
    }

    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        //Arrange
        CountryRequest? request = new CountryRequest() { CountryName = "Japan" };

        //Act
        CountryResponse response = _countriesService.AddCountry(request);
        List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

        //Assert
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Contains(response, countries_from_GetAllCountries);
    }

    [Fact]
    //The list of countries should be empty by default (before adding any countries) 
    public void GetCountryList_EmptyList()
    {   
        //Act
        List<CountryResponse> actualCountry = _countriesService.GetAllCountries();

        //Assert
        Assert.Empty(actualCountry);
    }
    
    [Fact] 
    public void GetCountryList_AddFewCountries()
    {   
        //Arrange
        List<CountryRequest> country_request_list =[ new CountryRequest() { CountryName = "USA"},new CountryRequest() { CountryName = "Germany"},new CountryRequest() { CountryName = "Belgium"}]; 
        List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

        //Assert
        foreach (var country_request in country_request_list)
        {
            countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
        } 

        List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

        //Assert
        foreach(var expected_country in countries_list_from_add_country)
        {
            Assert.Contains(expected_country, actualCountryResponseList);
        }
    }
}
