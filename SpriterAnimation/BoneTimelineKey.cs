namespace SpriterAnimation;

public partial class Spriter
{
    private class BoneTimelineKey : SpatialTimelineKey
    {
        // unimplemented in Spriter
        float length = 200;
        float width = 10;
        float height = 10;

        // override paint if you want debug visuals for bones
        /*void paint()
        {
            if (paintDebugBones)
            {
                float drawLength = length * scaleX;
                float drawHeight = height * scaleY;
                // paint debug bone representation
                // e.g. line starting at x,y,at angle,
                // of length drawLength, and height drawHeight
            }
        }*/

        /*TimelineKey linear(BoneTimelineKey keyB,float t)
            // keyB must be BoneTimelineKeys
        {
            BoneTimelineKey returnKey=this;
            returnKey.info=linear(info,keyB.info,spin,t);

            //if(paintDebugBones)
            {
                returnKey.length=linear(length,keyB.length,t);
                returnKey.width=linear(width,keyB.width,t);
            }

            return returnKey;
        }*/

    }
}