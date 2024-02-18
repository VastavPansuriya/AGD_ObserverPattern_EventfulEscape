using System.Collections.Generic;
using UnityEngine;

public partial class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    private SwitchState currentState;

    private void Start() => currentState = SwitchState.Off;

    public delegate void LightSwitchDelegate();

    public static event LightSwitchDelegate OnLightSwitch;

    private void OnEnable()
    {
        EventService.Instance.LightSwitchToggleEvent.AddListener(OnLightsToggled);
        EventService.Instance.LightsOffByGhostEvent.AddListener(OnLightsOffByGhostEvent);
    }

    private void OnDisable()
    {
        EventService.Instance.LightSwitchToggleEvent.RemoveListener(OnLightsToggled);
        EventService.Instance.LightsOffByGhostEvent.RemoveListener(OnLightsOffByGhostEvent);
    }

    public void Interact()
    {
        GameService.Instance.GetInstructionView().HideInstruction();
        EventService.Instance.LightSwitchToggleEvent.InvokeEvent();
    }
    private void ToggleLights()
    {
        bool lights = false;

        switch (currentState)
        {
            case SwitchState.On:
                currentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                currentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }

    private void SetLights(bool lights)
    {
        if (lights)
            currentState = SwitchState.On;
        else
            currentState = SwitchState.Off;

        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }
    private void OnLightsOffByGhostEvent()
    {
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
        SetLights(false);
    }
    private void OnLightsToggled()
    {
        ToggleLights();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
    }
}
