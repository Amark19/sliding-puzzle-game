using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class logoSplash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SplashLogo());   
    }

    IEnumerator SplashLogo()
    {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene("home");
    }
}
