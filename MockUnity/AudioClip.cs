using System;
namespace UnityEngine
{
    public interface AudioClip {
        float length { get; }

        int samples { get; }

        int channels { get; }

        int frequency { get; }

        bool isReadyToPlay { get; }
    }
}
