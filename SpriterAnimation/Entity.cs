namespace SpriterAnimation;

public partial class Spriter
{
    private class Entity
    {
        string name;
        CharacterMap[] characterMaps; // <character_map> tags
        public Animation[] animations; // <animation> tags
    }
}