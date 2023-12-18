using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Objects;

public class Notification
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string CreatedAt { get; set; }

    public string ObjectType { get; set; }

    public int ObjectId { get; set; }

    public int? SourceUserId { get; set; }

    public bool IsRead { get; set; }

    public NotificationDetails Details { get; set; }
}

public class NotificationDetails 
{
    public string Title { get; set; }

    public int PostId { get; set; }

    public string Username { get; set; }

    public string CoverUrl { get; set; }
}