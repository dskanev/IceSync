namespace IceSync.UI.Models
{
    public record WorkflowOutputModel
    (
        int Id,
        string? Name,
        bool IsActive,
        bool IsRunning,
        string? MultiExecBehavior
    );
}
