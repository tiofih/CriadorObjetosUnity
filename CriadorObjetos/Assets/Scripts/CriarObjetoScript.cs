using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Vuforia;

public class CriarObjetoScript : MonoBehaviour
{
    public TMP_InputField posX;
    public TMP_InputField posY;
    public TMP_InputField posZ;

    public TMP_InputField rotX;
    public TMP_InputField rotY;
    public TMP_InputField rotZ;

    public Toggle localToggle;
    public Toggle vuforiaToggle;

    public TMP_Text errorText;

    public GameObject novoObjeto;
    public Transform imageTarget;
    public GameObject arCam;
    public GameObject cam;

    Vector3 novaPos;
    Vector3 novaRot;

    private void Start()
    {
        AtivarEDesativarVuforia(false);
    }

    public void CriarObjeto()
    {
        if (localToggle.isOn)
        {
            PegarCoordenadasLocal();
        }
        else
        {
            StartCoroutine(PegarCoordenadasGithub());
        }
    }

    public void AtivarEDesativarVuforia(bool isOn)
    {
        if (isOn)
        {
            arCam.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            VuforiaRuntime.Instance.InitVuforia();
            arCam.GetComponent<VuforiaBehaviour>().enabled = true;
            arCam.GetComponent<DefaultInitializationErrorHandler>().enabled = true;
        }
        else
        {
            arCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            VuforiaRuntime.Instance.Deinit();
            arCam.GetComponent<VuforiaBehaviour>().enabled = false;
            arCam.GetComponent<DefaultInitializationErrorHandler>().enabled = false;
        }
    }

    void PegarCoordenadasLocal()
    {
        GameObject obj = Instantiate(novoObjeto, imageTarget);
        try
        {
            novaPos = new Vector3(
            float.Parse(posX.text),
            float.Parse(posY.text),
            float.Parse(posZ.text));

            novaRot = new Vector3(
            float.Parse(rotX.text),
            float.Parse(rotY.text),
            float.Parse(rotZ.text));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            if (e.Message == "Invalid format.")
            {
                errorText.text = "Por favor, preencha todos os campos";
            }
            return;
        }
        if (vuforiaToggle.isOn)
        {
            obj.transform.localPosition = novaPos;
            obj.transform.localRotation = Quaternion.Euler(novaRot);
        }
        else
        {
            obj.transform.position = novaPos;
            obj.transform.rotation = Quaternion.Euler(novaRot);
        }
    }

    IEnumerator PegarCoordenadasGithub()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://raw.githubusercontent.com/tiofih/CriadorObjetosUnity/master/file.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string downloadText = www.downloadHandler.text;
            Debug.Log(downloadText);
            string[] split = downloadText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            novaPos = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            novaRot = new Vector3(float.Parse(split[3]), float.Parse(split[4]), float.Parse(split[5]));

            GameObject obj = Instantiate(novoObjeto, imageTarget);

            if (vuforiaToggle.isOn)
            {
                obj.transform.localPosition = novaPos;
                obj.transform.localRotation = Quaternion.Euler(novaRot);
            }
            else
            {
                obj.transform.position = novaPos;
                obj.transform.rotation = Quaternion.Euler(novaRot);
            }
        }
    }
}
