namespace SpriterAnimation;

public partial class Spriter
{
    private class Ref
    {
        int parent = -1; // -1==no parent - uses ScmlObject spatialInfo as parentInfo
        int timeline;
        int key;
    }
}