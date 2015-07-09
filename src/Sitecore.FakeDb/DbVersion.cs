namespace Sitecore.FakeDb
{
  using Sitecore.Diagnostics;

  public class DbVersion
  {
    public DbVersion([NotNull] string language, int version = 0)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");
      this.Language = language;
      this.Version = version;
      this.Fields = new DbFieldCollection();
    }

    public string Language { get; set; }

    public int Version { get; set; }

    public DbFieldCollection Fields { get; set; }

    public void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      if (this.Version == 0)
      {
        field.SetValue(this.Language, field.Value);
      }
      else
      {
        field.SetValue(this.Language, this.Version, field.Value);
      }

      this.Fields.Add(field);
    }
  }
}
