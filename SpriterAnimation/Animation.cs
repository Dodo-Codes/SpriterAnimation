namespace SpriterAnimation;

public partial class Spriter
{
    private class Animation
    {
        string name;
        int length;
        bool isLooping; // enum : NO_LOOPING,LOOPING
        MainlineKey[] mainlineKeys; // <key> tags within a single <mainline> tag
        Timeline[] timelines; // <timeline> tags

        public void setCurrentTime(float newTime)
        {
            if (isLooping)
            {
                newTime %= length;
                return;
            }

            newTime = MathF.Min(newTime, length);

            updateCharacter(mainlineKeyFromTime(newTime), newTime);
        }

        void updateCharacter(MainlineKey mainKey, float newTime)
        {
            BoneTimelineKey[] transformedBoneKeys;
            /*
            foreach(var b in mainKey.boneRefs)
            {
                SpatialInfo parentInfo;
                Ref currentRef=mainKey.boneRefs[b];

                if(currentRef.parent>=0)
                {
                    parentInfo=transformBoneKeys[currentRef.parent].info;
                }
                else
                {
                    parentInfo=characterInfo;
                }

                TimelineKey currentKey=keyFromRef(currentRef,newTime);
                currentKey.info=currentKey.info.unmapFromParent(parentInfo);
                transformBoneKeys.push(currentKey);
            }

            TimelineKey[] objectKeys;
            foreach(var o in mainKey.objRefs)
            {
                SpatialInfo parentInfo;
                Ref currentRef=mainKey.objRefs[b];

                if(currentRef.parent>=0)
                {
                    parentInfo=transformBoneKeys[currentRef.parent].info;
                }
                else
                {
                    parentInfo=characterInfo;
                }

                TimelineKey currentKey=keyFromRef(currentRef,newTime);
                currentKey.info=currentKey.info.unmapFromParent(parentInfo);
                objectKeys.push(currentKey);
            }

            // <expose objectKeys to api users to retrieve AND replace objectKeys>

            foreach(var k in objectKeys)
            {
                objectKeys[k].paint();
            }
            */
        }

        MainlineKey mainlineKeyFromTime(float time)
        {
            /*
            int currentMainKey=0;
            foreach(var m in mainlineKeys)
            {
                if(mainlineKeys[m].time<=currentTime)
                {
                    currentMainKey=m;
                }
                if(mainlineKeys[m]>=currentTime)
                {
                    break;
                }
            }
            return mainlineKeys[currentMainKey];
            */
            return default;
        }

        TimelineKey keyFromRef(Ref r, float newTime)
        {
            /*
            Timeline timeline=timelines[r.timeline];
            TimelineKey keyA=timeline.keys[r.key];

            if(timeline.keys.size()==1)
            {
                return keyA;
            }

            int nextKeyIndex=r.key+1;

            if(nextKeyIndex>=timeline.keys.size())
            {
                if(isLooping=LOOPING)
                {
                    nextKeyIndex=0;
                }
                else
                {
                    return keyA;
                }
            }

            TimelineKey keyB=timeline.keys[nextKeyIndex];
            int keyBTime=keyB.time;

            if(keyBTime<keyA.time)
            {
                keyBTime=keyBTime+length;
            }

            return keyA.interpolate(keyB,keyBTime,newTime);
            */
            return default;
        }

    }
}