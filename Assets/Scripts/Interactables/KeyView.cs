using System.Security.Cryptography;
using UnityEngine;

public class KeyView : MonoBehaviour, IInteractable
{
    [SerializeField] GameUIView gameUIView;
    public void Interact()
    {

        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.KeyPickUp);
        int keys = ++GameService.Instance.GetPlayerController().KeysEquipped;
        EventService.Instance.OnKeyPinkedUp.InvokeEvent(keys);

        gameObject.SetActive(false);
    }
}
