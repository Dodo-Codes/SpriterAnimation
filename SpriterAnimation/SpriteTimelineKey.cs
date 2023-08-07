namespace SpriterAnimation;

public partial class Spriter
{
    private class SpriteTimelineKey : SpatialTimelineKey
    {
        int folder; // index of the folder within the ScmlObject
        int file;
        bool useDefaultPivot; // true if missing pivot_x and pivot_y in object tag
        float pivot_x = 0;
        float pivot_y = 1;

        /*void paint()
        {
            float paintPivotX;
            float paintPivotY;
            if(useDefaultPivot)
            {
                paintPivotX=Spriter.activeCharacterMap[folder].files[file].pivotX;
                paintPivotY=Spriter.activeCharacterMap[folder].files[file].pivotY;
            }
            else
            {
                paintPivotX=pivot_x;
                paintPivotY=pivot_y;
            }

            // paint image represented by
            // ScmlObject.activeCharacterMap[folder].files[file],fileReference
            // at x,y,angle (counter-clockwise), offset by paintPivotX,paintPivotY
        }*/

        /*TimelineKey linear(SpriteTimelineKey keyB,float t)
            // keyB must be SpriteTimelineKey
        {
            SpriteTimelineKey returnKey=this;
            returnKey.info=linear(info,keyB.info,spin,t);
            //if(!useDefaultPoint)
            {
                returnKey.pivot_x=linear(pivot_x,keyB.pivot_x,t);
                returnKey.pivot_y=linear(pivot_y,keyB.pivot_y,t);
            }

            return returnKey;
        }*/

    }
}