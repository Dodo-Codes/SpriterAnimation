namespace SpriterAnimation;

internal class Entity
{
    public Dictionary<string, Bone> Bones { get; } = new();
    public Dictionary<string, Animation> Animations { get; } = new();
}