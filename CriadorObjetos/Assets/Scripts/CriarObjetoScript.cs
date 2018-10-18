using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CriarObjetoScript : MonoBehaviour
{
    public TMP_InputField posX;
    public TMP_InputField posY;
    public TMP_InputField posZ;

    public TMP_InputField rotX;
    public TMP_InputField rotY;
    public TMP_InputField rotZ;

    public Toggle useLocal;

    public GameObject novoObjeto;

    public void CriarObjeto()
    {
        if (useLocal.isOn)
        {
            Vector3 novaPos = Vector3.zero;
            Vector3 novaRot = Vector3.zero;
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
            catch (System.Exception)
            {
                Debug.LogWarning("Campo Vazio");
                return;
            }
            Instantiate(novoObjeto, novaPos, Quaternion.Euler(novaRot));
        }
        else
        {

        }
    }

    public void WriteAndReadFile()
    {
        string path = "Assets/file.txt";

        /*File.AppendAllText(path, System.String.Format("{0} {1} {2}\n",
        posX.text, posY.text, posZ.text));*/

        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        string[] splitted = line.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        Vector3 pos = new Vector3(float.Parse(splitted[0]), float.Parse(splitted[1]), float.Parse(splitted[2]));
        Debug.Log(pos);
    }
}
