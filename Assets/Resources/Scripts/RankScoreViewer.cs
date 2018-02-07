using UnityEngine;
using UnityEngine.UI;

public class RankScoreViewer : MonoBehaviour
{

    public LogoText logoText;

    public Text point;

	void Start ()
    {
        logoText = GetComponent<LogoText>();
        if (logoText == null) Debug.Log("Errorrrrrrrrrrrrrrrrr");
	}
}
