namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Xunit;

  public class DbVersionTest
  {
    // TODO: Looks strange, but it was described in issue in such way
    [Fact]
    public void ShouldAddUnversionedField()
    {
      // arrage
      var version = new DbVersion("en");
      var field = new DbField("name") { { "en", "value" } };

      // act
      version.Add(field);

      // assert
      version.Fields.Count().Should().Be(1);
      var expectedField = version.Fields.Single();
      expectedField.GetValue("en", 0).Should().Be("value");
    }

    [Fact]
    public void ShouldAddVersionedField()
    {
      // arrage
      var version = new DbVersion("en", 1);
      var field = new DbField("name") { { "en", "value" } };

      // act
      version.Add(field);

      // assert
      version.Fields.Count().Should().Be(1);
      var expectedField = version.Fields.Single();
      expectedField.GetValue("en", 1).Should().Be("value");
    }
  }
}


