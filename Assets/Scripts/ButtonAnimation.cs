using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public Animation anim;
    [SerializeField] public float delay = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        gameObject.SetActive(false);
        Invoke("PlayAnimation",delay);
    }
    void PlayAnimation()
    {
        gameObject.SetActive(true);
        anim.Play("ButtonAnimation");
    }
    void init() {
        new WaitForSeconds(delay);
    }

    // Update is called once per frame
    void Update()
    {


    }
}
