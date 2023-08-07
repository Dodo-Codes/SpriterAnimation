namespace SpriterAnimation;

internal class Animation
{
    public string Name { get; set; }
    public int Duration { get; set; }
    public int Interval { get; set; }

    public Dictionary<string, List<KeyFrame>> KeyFrames { get; } = new();
}