using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private FMOD.Studio.VCA vca;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        slider = GetComponent<Slider>();

    }

    // Update is called once per frame
    public void SetVolume(float volume)
    { 
        vca.setVolume(slider.value);
    }
}
