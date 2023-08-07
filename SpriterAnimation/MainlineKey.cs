namespace SpriterAnimation;

public partial class Spriter
{
    private class MainlineKey
    {
        int time = 0;
        public Ref[] boneRefs; // <bone_ref> tags
        public Ref[] objectRefs; // <object_ref> tags
    }
}