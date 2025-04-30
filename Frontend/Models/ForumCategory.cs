using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Frontend.Models;

public enum ForumCategory
{
    [Display(Name = "Announcements")]
    Announcements,
    [Display(Name = "Arcade Games")]
    ArcadeGames,
    [Display(Name = "Support")]
    Support,
    [Display(Name = "General Discussion")]
    GeneralDiscussion,
    [Display(Name = "Off-Topic")]
    OffTopic
}

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? enumValue.ToString();
    }
}