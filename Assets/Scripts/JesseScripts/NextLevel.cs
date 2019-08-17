using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour
{

    public string nxtLvl;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(nxtLvl);
    }
}
