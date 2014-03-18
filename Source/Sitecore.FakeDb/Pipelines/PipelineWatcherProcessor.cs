namespace Sitecore.FakeDb.Pipelines
{
  using System.Xml;
  using Sitecore.Configuration;
  using Sitecore.Pipelines;
  using Sitecore.Xml;
  using System;

  public class PipelineWatcherProcessor
  {
    public static event EventHandler<PipelineRunEventArgs> PipelineRun;

    private readonly string pipelineName;

    public PipelineWatcherProcessor(string pipelineName)
    {
      Diagnostics.Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");

      this.pipelineName = pipelineName;
    }

    public string PipelineName
    {
      get { return this.pipelineName; }
    }

    public virtual void Process(PipelineArgs args)
    {
      if (PipelineRun != null)
      {
        PipelineRun(this, new PipelineRunEventArgs(this.PipelineName, args));
      }
    }
  }
}