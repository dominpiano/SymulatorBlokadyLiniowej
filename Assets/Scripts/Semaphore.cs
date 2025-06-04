public class Semaphore {

    public string Name;
    public SemaphoreState State;
    public bool IsOn => State != SemaphoreState.Stop;
    public bool IsLocked => RouteChecker.CheckIfLocked(this);

    public Semaphore(string _name, SemaphoreState _state = SemaphoreState.Stop) {
        Name = _name;
        State = _state;
    }
}