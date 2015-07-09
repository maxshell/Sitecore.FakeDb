﻿namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Security.AccessControl;
  using Xunit;

  public class DbItemTest
  {
    [Fact]
    public void ShouldGenerateNewIdsIfNotSet()
    {
      // arrange & act
      var item = new DbItem("my item");

      // assert
      item.ID.IsNull.Should().BeFalse();
    }

    [Fact]
    public void ShouldGenerateNameBasedOnIdIfNotSet()
    {
      // arrange
      var id = ID.NewID;
      var item = new DbItem(null, id);

      // act & assert
      item.Name.Should().Be(id.ToShortID().ToString());
    }

    [Fact]
    public void ShouldCreateNewDbItem()
    {
      // arrange & act
      var item = new DbItem("home");

      // assert
      item.TemplateID.IsNull.Should().BeTrue();
      item.Children.Should().BeEmpty();
      item.Fields.Should().BeEmpty();
      item.FullPath.Should().BeNull();
      item.ParentID.Should().BeNull();
    }

    [Fact]
    public void ShouldAddFieldByNameAndValue()
    {
      // arrange
      var item = new DbItem("home") { { "Title", "Welcome!" } };

      // act & assert
      item.Fields.Should().ContainSingle(f => f.Name == "Title" && f.Value == "Welcome!");
    }

    [Fact]
    public void ShouldAddFieldByIdAndValue()
    {
      // arrange
      var item = new DbItem("home") { { FieldIDs.Hidden, "1" } };

      // act & assert
      item.Fields.Should().ContainSingle(f => f.ID == FieldIDs.Hidden && f.Value == "1");
    }

    [Fact]
    public void ShouldCreateItemWithChildrenProvidingName()
    {
      // arrange
      var child = new DbItem("child");

      // act
      var item = new DbItem("home", child);

      // assert
      item.Children.Single().Should().BeEquivalentTo(child);
    }

    [Fact]
    public void ShouldCreateItemWithChildrenProvidingNameAndId()
    {
      // arrange
      var child = new DbItem("child");

      // act
      var item = new DbItem("home", ID.NewID, child);

      // assert
      item.Children.Single().Should().BeEquivalentTo(child);
    }

    [Fact]
    public void ShouldCreateItemWithChildrenProvidingNameIdAndTemplateId()
    {
      // arrange
      var child = new DbItem("child");

      // act
      var item = new DbItem("home", ID.NewID, ID.NewID, child);

      // assert
      item.Children.Single().Should().BeEquivalentTo(child);
    }

    [Fact]
    public void ShouldCreateItemButNotAddChildrenProvidingNameIdAndTemplateIdIfChildrenObjectIsNull()
    {
      // arrange

      // act
      var item = new DbItem("home", ID.NewID, ID.NewID, null);

      // assert
      item.Children.Count.Should().Be(0);
    }

    [Fact]
    public void ShouldCreateItemButNotAssignChildrenThatAreNotDbItems()
    {
      // arrange

      // act
      var item = new DbItem("home", ID.NewID, ID.NewID, null, new object());

      // assert
      item.Children.Count.Should().Be(0);
    }

    [Fact]
    public void ShouldAddChildItem()
    {
      // arrange
      var parent = new DbItem("parent");
      var child = new DbItem("child");

      // act
      parent.Add(child);

      // assert
      parent.Children.Single().Should().BeEquivalentTo(child);
    }

    [Theory, AutoData]
    public void ShouldAddVersion(DbItem item)
    {
      // arrange
      var version = new DbVersion("en", 1);
      version.Fields.Add(new DbField("my field") { { "en", "Hello" } });

      // act
      item.Add(version);

      // assert
      item.VersionsCount.Should().ContainKey("en");
      item.VersionsCount["en"].Should().Be(1);
      item.Fields.Count().Should().Be(1);
      var expectedField = item.Fields.Single();
      expectedField.GetValue("en", 1).Should().Be("Hello");
    }

    [Fact]
    public void ShouldCreateNewItemAccess()
    {
      // arrange
      var item = new DbItem("home");

      // act
      item.Access.Should().BeOfType<DbItemAccess>();
    }

    [Fact]
    public void ShouldSetItemAccess()
    {
      // arrange
      var item = new DbItem("home") { Access = new DbItemAccess { CanRead = false } };

      // act & assert
      item.Access.CanRead.Should().BeFalse();
    }

    [Theory, AutoData]
    public void ShouldThrowIfFieldNameIsNull(DbItem item, string value)
    {
      // act
      Action action = () => item.Add((string)null, value);

      // assert
      action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: fieldName");
    }

    [Theory, AutoData]
    public void ShouldThrowIfFieldIdIsNull(DbItem item, string value)
    {
      // act
      Action action = () => item.Add((ID)null, value);

      // assert
      action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: fieldId");
    }

    [Theory, AutoData]
    public void ShouldThrowIfFieldIsNull(DbItem item)
    {
      // act
      Action action = () => item.Add((DbField)null);

      // assert
      action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: field");
    }

    [Theory, AutoData]
    public void ShouldThrowIChildItemIsNull(DbItem item)
    {
      // act
      Action action = () => item.Add((DbItem)null);

      // assert
      action.ShouldThrow<ArgumentNullException>().WithMessage("Value cannot be null.\r\nParameter name: child");
    }
  }
}