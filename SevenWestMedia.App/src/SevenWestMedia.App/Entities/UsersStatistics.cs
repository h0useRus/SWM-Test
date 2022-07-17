
namespace SevenWestMedia.App.Entities;

public record AgeStats(int Age)
{
    public List<GenderStats> GenderStats { get; } = new List<GenderStats>();

    public void Add(IEnumerable<GenderStats> stats)
    {
        foreach (var gender in stats)
            if(!GenderStats.Contains(gender))
                GenderStats.Add(gender);
    }
}
public record GenderStats(char Gender, int Count);
