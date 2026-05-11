using GearrOnes.HOA.Template.Core.Attributes;

namespace GearrOnes.HOA.Template.Features.Calendar.ViewModels;

[HoaFeature("FullManagement", 2)]
public class CalendarMonthViewModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel { get; set; } = string.Empty;
    public IReadOnlyList<CalendarDayViewModel> Days { get; set; } = [];
}

[HoaFeature("FullManagement", 2)]
public class CalendarDayViewModel
{
    public DateTime Date { get; set; }
    public bool IsCurrentMonth { get; set; }
    public IReadOnlyList<CalendarListItemViewModel> Events { get; set; } = [];
}

[HoaFeature("FullManagement", 2)]
public class CalendarListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }
}