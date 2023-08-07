namespace SpriterAnimation;

public partial class Spriter
{
    private class Timeline
    {
        string name;
        int objectType; // enum : SPRITE,BONE,BOX,POINT,SOUND,ENTITY,VARIABLE
        TimelineKey[] keys; // <key> tags within <timeline> tags    
    }
}