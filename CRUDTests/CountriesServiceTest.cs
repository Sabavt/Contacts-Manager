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
        CountryAddRequest? request = new CountryAddRequest() { CountryName = null };
          
        //Assert
        Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_NullCountry()
    {
        //Arrange
        CountryAddRequest? request = null;
          
        //Assert
        Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(request));
    }

    [Fact]
    public void AddCountry_DublicatedCountryName()
    {
        //Arrange
        CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
        CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };

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
        CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

        //Act
        CountryResponse response = _countriesService.AddCountry(request);

        //Assert
        Assert.True(response.CountryID != Guid.Empty);
    }
}
