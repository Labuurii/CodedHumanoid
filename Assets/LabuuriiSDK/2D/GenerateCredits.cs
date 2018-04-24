using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class GenerateCredits : MonoBehaviour {

    public Text TextPrefab;
    public Credit[] Credits;

    [Serializable]
    public struct Credit
    {
        public string object_name, object_link, profile_name, profile_link;
    }

    // Use this for initialization
    void Start () {
        var sb = new StringBuilder(100);

		foreach(var credit in Credits)
        {
            sb.Length = 0;
            sb.Append(credit.object_name);
            sb.Append("\nLink: ");
            sb.Append(credit.object_link);
            sb.Append("\nCreator: ");
            sb.Append(credit.profile_name);
            sb.Append("\nProfile: ");
            sb.Append(credit.profile_link);

            var text = Instantiate(TextPrefab.gameObject, transform).GetComponent<Text>();
            text.text = sb.ToString();
        }
	}
}
