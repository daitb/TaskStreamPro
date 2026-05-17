using System;
using System.Collections.Generic;

namespace server.Models;

public partial class TaskProject
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly? Deadline { get; set; }

    public int? AssigneeId { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User? Assignee { get; set; }

    public virtual Project Project { get; set; } = null!;
}
