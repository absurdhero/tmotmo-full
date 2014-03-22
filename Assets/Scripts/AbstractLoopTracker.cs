public interface AbstractLoopTracker {
    void startPlaying();
    void Repeat();
    void NextLoop(float loopLength);
    void ChangeLoopLength(float loopLength) ;
    void Rewind(float seconds);
    void PlayAll();
    void PlayAllButVocals();
    void Stop();
    bool IsLoopOver();
    float TimeLeftInCurrentLoop();
}
