﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public GameObject effectPrefab;

    static EffectsManager __instance = null;

    static public EffectsManager instance {

        get {
            if (__instance == null)
                __instance = GameObject.FindObjectOfType<EffectsManager>();
            return __instance;
        }
    }

    /*
     * Creates an gets an effect created from effectPrefab. Generates a new Instance
     * if doesn't exist. If effect does exist, returns effect Instance found in parent.
     */
    static public ParticleSystem createEffect(Transform parent) {
       
        ParticleSystem effect = parent.GetComponentInChildren<ParticleSystem>();
        if (effect == null) {
            GameObject obj = Instantiate(instance.effectPrefab, parent);
            return obj.GetComponent<ParticleSystem>();
        }
        else
            return effect;
    }

    static public ParticleSystem createEffect(Transform parent, GameObject effectPrefab)
    {

        ParticleSystem effect = parent.GetComponentInChildren<ParticleSystem>();
        if (effect == null)
        {
            GameObject obj = Instantiate(effectPrefab, parent);
            return obj.GetComponent<ParticleSystem>();
        }
        else
            return effect;
    }
} 