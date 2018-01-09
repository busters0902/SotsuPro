using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankScoreViewer : MonoBehaviour
{

    public LogoText logoText;

	void Start ()
    {
        logoText = GetComponent<LogoText>();
        if (logoText == null) Debug.Log("Errorrrrrrrrrrrrrrrrr");
	}
}
