using OpenTelemetry;
using System.Diagnostics;

namespace Geev.Api.Framrwork.OpenTelemetry;

internal class CustomActivityFilterProcessor : BatchExportProcessor<Activity>
{
    private readonly OpenTelemetryOptions.FilterActivityOptions _filterActivities;
    public CustomActivityFilterProcessor(
        BaseExporter<Activity> exporter, OpenTelemetryOptions.FilterActivityOptions filterActivities,
        int maxQueueSize = 2048,
        int scheduledDelayMilliseconds = 5000,
        int exporterTimeoutMilliseconds = 30000,
        int maxExportBatchSize = 512)
        : base(
            exporter,
            maxQueueSize,
            scheduledDelayMilliseconds,
            exporterTimeoutMilliseconds,
            maxExportBatchSize)
    {
        _filterActivities = filterActivities;
    }
    public override void OnEnd(Activity activity)
    {
        try
        {
            if (FilterDisplayName(activity)) return;

            if (FilterTags(activity)) return;

            if (FilterTagGroups(activity)) return;

            if (FilterTagGroupsWithoutParent(activity)) return;
        }
        catch (Exception e)
        {
            // ignored
        }

        OnExport(activity);
    }

    private bool FilterDisplayName(Activity activity)
    {
        return _filterActivities.DisplayNames.Contains(activity.DisplayName);
    }

    private bool FilterTags(Activity activity)
    {
        return _filterActivities.Tags?.Any(tag => activity.Tags.Contains(tag)) ?? false;
    }

    private bool FilterTagGroups(Activity activity)
    {
        var hasFilterCondition = _filterActivities.TagGroups.Select(tagGroup =>
        {
            return tagGroup.All(identifier => activity?.Tags?.Contains(identifier) ?? false);
        });

        return hasFilterCondition.Any(x => x);
    }

    private bool FilterTagGroupsWithoutParent(Activity activity)
    {
        var hasFilterCondition = _filterActivities.TagGroupsWithoutParent.Select(tagGroup =>
        {
            return tagGroup.All(identifier => activity?.Tags?.Contains(identifier) ?? false)
                && activity.Parent is null;
        });

        return hasFilterCondition.Any(x => x);
    }
}
