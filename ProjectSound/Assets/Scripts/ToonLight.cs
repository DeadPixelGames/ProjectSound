using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonLight : MonoBehaviour
{

    private Light lightF = null;
    // Start is called before the first frame update
    void Start()
    {
        lightF = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_ToonDirection", -this.transform.forward);
    }
}
