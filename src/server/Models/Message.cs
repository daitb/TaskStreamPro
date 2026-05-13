using System;
using System.Collections.Generic;

namespace server.Models;

public partial class Message
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int SenderId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
