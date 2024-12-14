using System.ComponentModel;

namespace TinyUrl.DTOs;

public enum Option
{
    [Description("Create a new tiny URL")]
    Create = 1,

    [Description("Get long URL from tiny URL")]
    Read = 2,

    [Description("Delete a tiny URL")]
    Delete = 3,

    [Description("View statistics for a tiny URL")]
    Statistics = 4
}