using System.Collections.ObjectModel;
using System.Reflection;
using Elsa.Activities;
using Elsa.Contracts;
using Elsa.Signals;

namespace Elsa.Models;

public class ActivityExecutionContext
{
    private readonly List<Bookmark> _bookmarks = new();

    public ActivityExecutionContext(
        WorkflowExecutionContext workflowExecutionContext,
        ActivityExecutionContext? parentActivityExecutionContext,
        ExpressionExecutionContext expressionExecutionContext,
        IActivity activity,
        CancellationToken cancellationToken)
    {
        WorkflowExecutionContext = workflowExecutionContext;
        ParentActivityExecutionContext = parentActivityExecutionContext;
        ExpressionExecutionContext = expressionExecutionContext;
        Activity = activity;
        CancellationToken = cancellationToken;
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public WorkflowExecutionContext WorkflowExecutionContext { get; }
    public ActivityExecutionContext? ParentActivityExecutionContext { get; internal set; }
    public ExpressionExecutionContext ExpressionExecutionContext { get; }

    /// <summary>
    /// The currently executing activity.
    /// </summary>
    public IActivity Activity { get; set; }

    /// <summary>
    /// A cancellation token to use when invoking asynchronous operations.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    /// A dictionary of values that can be associated with the activity. 
    /// </summary>
    public IDictionary<string, object> ApplicationProperties { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Returns the <see cref="ActivityNode"/> metadata about the current activity.
    /// </summary>
    public ActivityNode ActivityNode => WorkflowExecutionContext.FindNodeByActivity(Activity);

    /// <summary>
    /// A list of bookmarks created by the current activity.
    /// </summary>
    public IReadOnlyCollection<Bookmark> Bookmarks => new ReadOnlyCollection<Bookmark>(_bookmarks);

    /// <summary>
    /// Gets or sets a value that indicates if the workflow should continue executing or not.
    /// </summary>
    public bool Continue { get; private set; } = true;

    /// <summary>
    /// A dictionary of received inputs.
    /// </summary>
    public IDictionary<string, object> Input => WorkflowExecutionContext.Input;

    /// <summary>
    /// Journal data will be added to the workflow execution log for the "Executed" event.  
    /// </summary>
    public IDictionary<string, object?> JournalData { get; private set; } = new Dictionary<string, object?>();

    public void ScheduleActivity(IActivity? activity, ActivityCompletionCallback? completionCallback = default, IEnumerable<RegisterLocationReference>? locationReferences = default, object? tag = default)
    {
        if (activity == null)
            return;

        WorkflowExecutionContext.Schedule(activity, this, completionCallback, locationReferences, tag);
    }

    public void ScheduleActivity(IActivity? activity, ActivityExecutionContext owner, ActivityCompletionCallback? completionCallback = default, IEnumerable<RegisterLocationReference>? locationReferences = default, object? tag = default)
    {
        if (activity == null)
            return;

        WorkflowExecutionContext.Schedule(activity, owner, completionCallback, locationReferences, tag);
    }

    public void PostActivities(params IActivity?[] activities) => PostActivities((IEnumerable<IActivity?>)activities);

    public void PostActivities(IEnumerable<IActivity?> activities, ActivityCompletionCallback? completionCallback = default)
    {
        foreach (var activity in activities)
            ScheduleActivity(activity, completionCallback);
    }

    public void CreateBookmarks(IEnumerable<object> bookmarkData, ExecuteActivityDelegate? callback = default)
    {
        foreach (var bookmarkDatum in bookmarkData)
            CreateBookmark(bookmarkDatum, callback);
    }

    public void AddBookmarks(IEnumerable<Bookmark> bookmarks) => _bookmarks.AddRange(bookmarks);
    public void AddBookmark(Bookmark bookmark) => _bookmarks.Add(bookmark);

    public Bookmark CreateBookmark(ExecuteActivityDelegate callback) => CreateBookmark(default, callback);
    
    public Bookmark CreateBookmark(object? bookmarkDatum = default, ExecuteActivityDelegate? callback = default)
    {
        var hasher = GetRequiredService<IHasher>();
        var identityGenerator = GetRequiredService<IIdentityGenerator>();
        var bookmarkDataSerializer = GetRequiredService<IBookmarkDataSerializer>();
        var bookmarkDatumJson = bookmarkDatum != null ? bookmarkDataSerializer.Serialize(bookmarkDatum) : default;
        var hash = bookmarkDatumJson != null ? hasher.Hash(bookmarkDatumJson) : default;

        var bookmark = new Bookmark(
            identityGenerator.GenerateId(),
            Activity.TypeName,
            hash,
            bookmarkDatumJson,
            Activity.Id,
            Id,
            callback?.Method.Name);

        AddBookmark(bookmark);
        return bookmark;
    }

    public T? GetProperty<T>(string key) => ApplicationProperties!.TryGetValue<T?>(key, out var value) ? value : default;
    public void SetProperty<T>(string key, T value) where T : notnull => ApplicationProperties[key] = value;

    public T UpdateProperty<T>(string key, Func<T?, T> updater) where T : notnull
    {
        var value = GetProperty<T?>(key);
        value = updater(value);
        ApplicationProperties[key] = value;
        return value;
    }

    public T GetRequiredService<T>() where T : notnull => WorkflowExecutionContext.GetRequiredService<T>();
    public object GetRequiredService(Type serviceType) => WorkflowExecutionContext.GetRequiredService(serviceType);
    public T? Get<T>(Input<T>? input) => input == null ? default : Get<T>(input.LocationReference);

    public object? Get(RegisterLocationReference locationReference)
    {
        var location = GetLocation(locationReference) ?? throw new InvalidOperationException($"No location found with ID {locationReference.Id}. Did you forget to declare a variable with a container?");
        return location.Value;
    }

    public T? Get<T>(RegisterLocationReference locationReference)
    {
        var value = Get(locationReference);
        return value != default ? (T?)(value) : default;
    }

    public void Set(RegisterLocationReference locationReference, object? value) => ExpressionExecutionContext.Set(locationReference, value);
    public void Set(Output output, object? value) => ExpressionExecutionContext.Set(output, value);
    public void Set<T>(Output output, T value) => ExpressionExecutionContext.Set(output, value);

    public async Task<T?> EvaluateAsync<T>(Input<T> input)
    {
        var evaluator = GetRequiredService<IExpressionEvaluator>();
        var locationReference = input.LocationReference;
        var value = await evaluator.EvaluateAsync(input, ExpressionExecutionContext);
        locationReference.Set(this, value);
        return value;
    }
    
    /// <summary>
    /// Stops further execution of the workflow.
    /// </summary>
    public void PreventContinuation() => Continue = false;
    
    /// <summary>
    /// Send a signal up the current branch.
    /// </summary>
    public async ValueTask SignalAsync(object signal)
    {
        var ancestorContexts = GetAncestorActivityExecutionContexts();
        
        foreach (var ancestorContext in ancestorContexts)
        {
            var signalContext = new SignalContext(ancestorContext, this, CancellationToken);

            if (ancestorContext.Activity is not ISignalHandler handler) 
                continue;
            
            await handler.HandleSignalAsync(signal, signalContext);

            if (signalContext.StopPropagationRequested)
                return;
        }
    }
    
    /// <summary>
    /// Explicitly complete the current activity. This should only be called by activities that explicitly suppress automatic-completion.
    /// </summary>
    public async ValueTask CompleteActivityAsync()
    {
        await SignalAsync(new ActivityCompleted());
    }

    /// <summary>
    /// Returns a flattened list of the current context's ancestors.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ActivityExecutionContext> GetAncestorActivityExecutionContexts()
    {
        var current = ParentActivityExecutionContext;

        while (current != null)
        {
            yield return current;
            current = current.ParentActivityExecutionContext;
        }
    }

    private RegisterLocation? GetLocation(RegisterLocationReference locationReference) =>
        ExpressionExecutionContext.Register.TryGetLocation(locationReference.Id, out var location)
            ? location
            : ParentActivityExecutionContext?.GetLocation(locationReference);
}