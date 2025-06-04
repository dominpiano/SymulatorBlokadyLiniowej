using System;
using System.Linq;
using UnityEngine.UIElements;

public class Semaphore {

    public string Name;
    public SemaphoreState State;
    public bool IsOn => State != SemaphoreState.Stop;
    public bool IsLocked => RouteChecker.CheckIfLocked(this);
    public VisualElement VisualSemaphore;

    public Semaphore(VisualElement _visSem, string _name, SemaphoreState _state = SemaphoreState.Stop) {
        VisualSemaphore = _visSem;
        Name = _name;
        State = _state;
    }

    public void ChangeLight(SemaphoreSignal toSignal) {
        var lights = VisualSemaphore.Children();

        switch (toSignal) {
            case SemaphoreSignal.S1:
                lights.Where(l => l.name.Contains("Red", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !l.name.Contains("Red", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S2:
                lights.Where(l => l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S5:
                lights.Where(l => l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !(l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase))).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
            case SemaphoreSignal.S10:
                if (lights.Count(l => l.name.Contains("Orange")) == 0) return;

                lights.Where(l => l.name.Contains("Green", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase)).First().RemoveFromClassList("semLightOff");
                lights.Where(l => !(l.name.Contains("OrangeLight", StringComparison.OrdinalIgnoreCase) || l.name.Contains("Orange1Light", StringComparison.OrdinalIgnoreCase) ||
                                    l.name.Contains("Green", StringComparison.OrdinalIgnoreCase))).ToList().ForEach(light => light.AddToClassList("semLightOff"));
                break;
        }
    }
}