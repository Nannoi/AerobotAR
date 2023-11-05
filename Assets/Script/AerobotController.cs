using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TextSpeech;

[RequireComponent(typeof(Animator))]
public class AerobotController : MonoBehaviour
{
    private Text uiText;
    private Text previousText;
    private Animator animator;
    private InputField text;
    private string height="2 m";
    private string state ="standby";
    private string stringLength ="1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m" + System.Environment.NewLine +
                "1.5 m";

    private Text heightData;
    private Text stateData;
    private Text stringLData;


    private void Start()
    {
        GameObject canvasObject = GameObject.Find("Canvas");
        Transform speechResultTransform = canvasObject.transform.Find("SpeechResult");
        text = speechResultTransform.GetComponent<InputField>();
        // Add a listener to the onValueChanged event of the InputField component
        text.onValueChanged.AddListener(OnInputFieldValueChanged);

        GameObject heightG = FindObjectByName(canvasObject.transform, "HeightText");
        heightData = heightG.GetComponent<Text>();

        GameObject stateG = FindObjectByName(canvasObject.transform, "StateText");
        stateData = stateG.GetComponent<Text>();

        GameObject stringG = FindObjectByName(canvasObject.transform, "Stringdata");
        stringLData = stringG.GetComponent<Text>();

    }
    public GameObject FindObjectByName(Transform parent, string objectName)
    {
        if (parent.name == objectName)
        {
            return parent.gameObject;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            GameObject foundObject = FindObjectByName(child, objectName);

            if (foundObject != null)
            {
                return foundObject;
            }
        }

        return null;
    }
    void OnInputFieldValueChanged(string result)
    {
        animator = GetComponent<Animator>();
        AerobotAnim(result);

        heightData.text = height.ToString(); // Convert the Action to a string
        stateData.text = state.ToString();
        stringLData.text = stringLength.ToString();

    }

    // Update is called once per frame

    void AerobotAnim(string result)
    {
        string[] Aerokeys = new string[]
            {
            "standby",
            "closed",
            "cover",
            "special",
            "open",
            "reverse",
            "Bloom",
            "wave"
            };

        if (Aerokeys.Any(Aerokeys => result.Contains(Aerokeys)))
        {
            Debug.Log("Aerobot Action: " + result);
            if (result.Contains("standby"))
            {
                height = "2 m";
                state = "standby";
                stringLength = "1.5 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", true);
            }

            else if (result.Contains("closed"))
            {
                height = "1.8 m";
                state = "closed";
                stringLength = "1.5 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.2 m" + System.Environment.NewLine +
                    "1.2 m";

                animator.SetBool("Bend01", true);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);
            }

            else if (result.Contains("cover"))
            {
                height = "1.5 m";
                state = "cover";
                stringLength = "1.5 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.0 m" + System.Environment.NewLine +
                    "1.0 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", true);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);
            }

            else if (result.Contains("special"))
            {
                height = "1.5 m";
                state = "special";
                stringLength = "1.2 m" + System.Environment.NewLine +
                    "1.2 m" + System.Environment.NewLine +
                    "1.2 m" + System.Environment.NewLine +
                    "1.2 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", true);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);
            }

            else if (result.Contains("reverse"))
            {
                height = "1.8 m";
                state = "reverse";
                stringLength = "1.2 m" + System.Environment.NewLine +
                    "1.2 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", true);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);
            }

            else if (result.Contains("open"))
            {
                height = "1.5 m";
                state = "open";
                stringLength = "1.0 m" + System.Environment.NewLine +
                    "1.0 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", true);
                animator.SetBool("StandBy", false);
            }

            else if (result.Contains("Bloom"))
            {
                state = "bloom";
                StartCoroutine(BloomCoroutine());
            }
            else if (result.Contains("wave"))
            {
                state = "wave";
                StartCoroutine(WaveCoroutine());
            }

            IEnumerator BloomCoroutine()
            {
                height = "1.5 m";
                state = "open";
                stringLength = "1.0 m" + System.Environment.NewLine +
                    "1.0 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", true);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", true);
                animator.SetBool("StandBy", false);
            }

            IEnumerator WaveCoroutine()
            {
                height = "1.5 m";
                state = "open";
                stringLength = "1.0 m" + System.Environment.NewLine +
                    "1.0 m" + System.Environment.NewLine +
                    "1.5 m" + System.Environment.NewLine +
                    "1.5 m";

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", true);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", true);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.5f);

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", true);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("Bend01", true);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", false);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", true);
                animator.SetBool("StandBy", false);

                yield return new WaitForSeconds(1.0f);

                animator.SetBool("Bend01", false);
                animator.SetBool("Bend02", true);
                animator.SetBool("Bend03", false);
                animator.SetBool("Bend04", false);
                animator.SetBool("Bend05", false);
                animator.SetBool("StandBy", false);
            }
           
        }

    }
}
     

