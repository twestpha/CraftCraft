using UnityEngine;

public class Timer {
    private float duration;
    private float startTime;
    private float pausedElapsed;
    private bool finishedLastFrame;

    public Timer(){
        pausedElapsed = -1.0f;
        duration = 0.0f;
    }

    public Timer(float duration_){
        pausedElapsed = -1.0f;
        duration = duration_;
        // Timer starts "finished"
        startTime = -duration;
    }

    public void Start(){
        finishedLastFrame = false;
        startTime = Time.time;
    }

    public void Pause(){
        if(pausedElapsed < 0.0f){
            pausedElapsed = Elapsed();
        } else {
            Debug.LogError("Timer is already paused");
        }
    }

    public void Unpause(){
        if(pausedElapsed >= 0.0f){
            startTime = Time.time - pausedElapsed;
            pausedElapsed = -1.0f;
        } else {
            Debug.LogError("Timer is not paused");
        }
    }

    public float Elapsed(){
        return Time.time - startTime;
    }

    public float Parameterized(){
        return Mathf.Max(Mathf.Min(Elapsed() / duration, 1.0f), 0.0f);
    }

    public float ParameterizedUnclamped(){
        return Elapsed() / duration;
    }

    public float ParameterizedLooping(){
        return ParameterizedUnclamped() % 1.0f;
    }

    public bool Finished(){
        return Elapsed() >= duration;
    }

    public bool FinishedThisFrame(){
        if(!finishedLastFrame && Finished()){
            finishedLastFrame = true;
            return true;
        }

        return false;
    }

    public void SetParameterized(float value){
        startTime = Time.time - (value * duration);
    }

    public void SetDuration(float duration_){
        duration = duration_;
    }

    public float Duration(){
        return duration;
    }
};
