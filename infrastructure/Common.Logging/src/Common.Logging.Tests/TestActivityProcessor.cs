using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Logging.Tests;

public class TestActivityProcessor : BaseExportProcessor<Activity>
{
    private readonly List<Activity> exportedItems = new List<Activity>();

    public TestActivityProcessor(BaseExporter<Activity>? exporter)
        : base(exporter ?? new DummyExporter<Activity>())
    {
    }

    protected override void OnExport(Activity activity)
    {
        this.exportedItems.Add(activity);
    }

    public List<Activity> GetExportedItems()
    {
        return this.exportedItems;
    }
}

public class DummyExporter<T> : BaseExporter<T> where T : class
{
    public override ExportResult Export(in Batch<T> batch)
    {
        return ExportResult.Success;
    }
}