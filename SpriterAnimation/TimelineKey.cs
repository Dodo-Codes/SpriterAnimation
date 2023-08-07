namespace SpriterAnimation;

public partial class Spriter
{
    private class TimelineKey
    {
        int time = 0;
        CurveType curveType = CurveType.Linear; // enum : INSTANT,LINEAR,QUADRATIC,CUBIC
        float c1;
        float c2;

        TimelineKey interpolate(TimelineKey nextKey, int nextKeyTime, float currentTime)
        {
            return linear(nextKey, getTWithNextKey(nextKey, nextKeyTime, currentTime));
        }

        float getTWithNextKey(TimelineKey nextKey, int nextKeyTime, float currentTime)
        {
            if (curveType == CurveType.Instant || time == nextKey.time)
                return 0;

            var t = (currentTime - time) / (nextKey.time - time);

            if (curveType == CurveType.Linear)
                return t;
            else if (curveType == CurveType.Quadratic)
                return (Quadratic(0.0f, c1, 1.0f, t));
            else if (curveType == CurveType.Cubic)
                return (Cubic(0.0f, c1, c2, 1.0f, t));

            return 0; // Runtime should never reach here        
        }

        TimelineKey linear(TimelineKey keyB, float t)
        {
            // overridden in inherited types  return linear(this,keyB,t);
            return default;
        }

    }
}