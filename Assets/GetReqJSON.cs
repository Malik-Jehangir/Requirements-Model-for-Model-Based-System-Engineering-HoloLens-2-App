using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows.Speech;
using System.Linq;

public class GetReqJSON : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, Action>();



    string p_ID = "", p_pName = "", rm_lastm = "";

    public GameObject txt;
    public GameObject prefabMain;
    public GameObject plugMain;
    public GameObject megaSquare;

    public GameObject textPrefab;
    public GameObject LinePrefab;
  
    //public GameObject ProgressB;

    private Vector3 mzpos0 = new Vector3(-3.32f, 0.91f, 5.5f);
    private Vector3 mzposx =new Vector3(-10.28f, -0.72f, 5.5f);

    private Vector3 mzpos1 = new Vector3(0.08f, 0.91f, 5.56f);


    private RootObject myObject;
   
    int dec = 0;
    private float pos0 = 0.78f;
    private float pos1 = -3.19f;
    private float pos2 = 1.97f;
    private float pos3 = 250f;
    private float pos4 = 0.33f;
    private float posx = -9.8f;

    List<string> targetList=new List<string>();
    List<string> sourceList = new List<string>();
    List<Vector3> myposition = new List<Vector3>();



    private Transform newParent;
    private Transform newParent1;
    private string tmpStr;

    private GameObject clone = null;
    private GameObject clone2 = null;
    private GameObject clone3 = null;
    private GameObject clone4 = null;
    private GameObject clone5 = null;
    private GameObject clone6 = null;
    private GameObject clone7 = null;
    private GameObject clone8 = null;
    private GameObject clone9 = null;
    private GameObject clone10 = null;
    private GameObject clone11 = null;
    private GameObject clone12 = null;
    private GameObject clone13 = null;
    private GameObject clone14 = null;


    List<Vector3> sourcePos = new List<Vector3>();

    List<Vector3> SourcePosPC1 = new List<Vector3>();
    List<Vector3> targetPosPC1 = new List<Vector3>();
    List<Vector3> targetPosPC2 = new List<Vector3>();

    private Vector3 rootpos;

    private Vector3 childrootpos;

    [Serializable]
    public class SystemProperty
    {
        public string PropertyName;
    }

    [Serializable]
    public class SystemElement
    {
        public string systemElementId;
        public string name;
        //public string x-coordinate;
        //public string y-coordinate;
        public List<SystemProperty> systemProperties;
    }

    [Serializable]
    public class ElementProperty
    {
        public string PropertyName;
    }

    [Serializable]
    public class EnvironmentElement
    {
        public string name;
        public string environmentElementId;
        //public string x-coordinate;
        //public string y-coordinate;
        public List<ElementProperty> elementProperties;
    }

    [Serializable]
    public class FlowProperty
    {
        public string PropertyName;
    }

    [Serializable]
    public class Flow
    {
        public string flowID;
        public string flowType;
        public string flowName;
        public List<FlowProperty> flowProperties;
        public string source1;
        public string source2;
        public string target;
    }

    [Serializable]
    public class EnvironmentModel
    {
        public string lastModified;
        public SystemElement systemElement;
        public List<EnvironmentElement> environmentElements;
        public List<Flow> flow;

    }
    [Serializable]
    public class Requirement
    {
        public string requirementId;
        public string title;
        public string requirementType;
        public string description;
        public string demandOrWish;
        public string status;
        public string dateCreated;
        public string lastModified;
        public List<Requirement> subRequirements;
    }

    [Serializable]
    public class FlowProperty2
    {
        public string PropertyName;
    }

    [Serializable]
    public class Flow2
    {
        public string flowID;
        public string flowType;
        public string flowName;
        public List<FlowProperty2> flowProperties;
        public string source;
        public string target;
    }

    [Serializable]
    public class RequirementModel
    {
        public string lastModified;
        public List<Requirement> requirements;
        public List<Flow2> flow;
    }

    [Serializable]
    public class FlowProperty3
    {
        public string PropertyName;
    }

    [Serializable]
    public class Flow3
    {
        public string flowId;
        public string flowProperty;
        public string flowType;
        public string flowName;
        public List<FlowProperty3> flowProperties;
        public string source;
        public string target;
    }

    [Serializable]
    public class SubSubSystemElement
    {
        public string name;
        public string subsystemElementId;
        //public string x-coordinate;
        //public string y-coordinate;
    }

    [Serializable]
    public class SubSystemElement
    {
        public string name;
        public string subsystemElementId;
        //public string x-coordinate;
        //public string y-coordinate;
        public List<SubSubSystemElement> subSubSystemElements;
    }

    [Serializable]
    public class SystemElements
    {
        public string name;
        public string subsystemElementId;
        //public string x-coordinate;
        //public string y-coordinate;
        public List<SubSystemElement> subSystemElements;
    }

    [Serializable]
    public class ActiveStructureModel
    {
        public string lastModified;
        public List<Flow3> flow;
        public List<SystemElements> SystemElements;
    }

    [Serializable]
    public class ApplicationScenario
    {
        public string applicationScenarioID;
        public string title;
        public string situationDescription;
        public string behaviouralDescription;
        public string reference;
        public string dateCreated;
        public string lastModified;
    }

    [Serializable]
    public class ApplicationScenarioModel
    {
        public string lastModified;
        public List<ApplicationScenario> applicationScenarios;
    }

    [Serializable]
    public class InterModelConnections
    {
        public string connectionID;
        public string type;
        public string source;
        public string target;
    }

    [Serializable]
    public class RootObject
    {
        public int projectId;
        public string projectName;
        public string projectDescription;
        public List<string> projectTags;
        public string timeCreated;
        public string lastModified;
        public EnvironmentModel environmentModel;
        public RequirementModel requirementModel;
        public ActiveStructureModel activeStructureModel;
        public ApplicationScenarioModel applicationScenarioModel;
        public InterModelConnections InterModelConnections;
    }




    void Start()
    {

        GoCalled();
        //ProgressB.SetActive(false);

        keywords.Add("load", () =>
        {

            GoCalled();
        });

      

        keywords.Add("info", () =>
        {

            GoCalledInfo();
        });

        keywords.Add("refresh", () =>
        {

            GoCalledProgress();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
        keywordRecognizer.Start();

        //StartCoroutine(GetRequest("https://api.myjson.com/bins/j6fw2")); //online storage
    }

    void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();

        }
    }

    void GoCalled()
    {

        StartCoroutine(GetRequest("http://localhost:3000/projects?projectId=1")); //local server (JSON server via Node.js)

        Debug.Log("Requirements model without Flow lines loaded");

    }





    IEnumerator GetRequest(string uri)
    {
        WWW uwr = new WWW(uri);
        yield return uwr;

        if (uwr.error != null)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {

            tmpStr = uwr.text;
            getReady(tmpStr);
        }

    }

    void getReady(string tmpStr)
    {
       


        string newStr = tmpStr.Substring(1, tmpStr.Length - 2);


        myObject = JsonUtility.FromJson<RootObject>(newStr);
        Debug.Log("Received Project ID: " + myObject.projectId.ToString());
        Debug.Log("Received Project Name: " + myObject.projectName);
        Debug.Log("Requirements model last modified: " + myObject.requirementModel.lastModified.ToString());

        p_ID = myObject.projectId.ToString();
        p_pName = myObject.projectName;
        rm_lastm = myObject.requirementModel.lastModified.ToString();


        int c = 0;
        int mx = 0;

        //Debug.Log("Flow count: " + myObject.activeStructureModel.flow.Count.ToString());
        for (int mc = 0; mc < myObject.requirementModel.flow.Count; mc++)
        {
            mx += 1;
            Debug.Log("Flow member target " + mx.ToString() + ": " + myObject.requirementModel.flow[mc].target);
            Debug.Log("Flow member source " + mx.ToString() + ": " + myObject.requirementModel.flow[mc].source);

            sourceList.Add(myObject.requirementModel.flow[mc].source);
            targetList.Add(myObject.requirementModel.flow[mc].target);

        }

        //Only create a trace if the source and target list is not empty
        if (targetList.Count > 0 && sourceList.Count > 0)
        {
            //create tracebox depending on number of elements in target
            for (int z = 0; z < targetList.Count; z++)
            {
                //at a position create the element
                //print the ID box
                clone13 = Instantiate(plugMain, new Vector3(posx, -0.28f, 5.5f), Quaternion.identity) as GameObject;
                clone13.transform.localScale = new Vector3(140f, 120f, 1000f); //ID 


                //ID 
                clone14 = Instantiate(textPrefab, mzposx, Quaternion.identity) as GameObject;

                clone14.GetComponent<UnityEngine.TextMesh>().text = "Trace links connector";
                clone14.transform.position = mzposx;
                clone14.GetComponent<UnityEngine.TextMesh>().fontSize = 800;
                clone14.GetComponent<UnityEngine.TextMesh>().fontStyle = FontStyle.Bold;
                clone14.GetComponent<UnityEngine.TextMesh>().color = Color.yellow;

                myposition.Add(mzposx);


                mzposx.x -= 10f;
                posx -= 10f;

            }
        }











        for (int x = 0; x < myObject.requirementModel.requirements.Count; x++)
        {
          

           



            




            //print the ID box
            clone5 = Instantiate(prefabMain, new Vector3(pos1, pos0, 5.5f), Quaternion.identity) as GameObject;
            clone5.transform.localScale = new Vector3(80f, 100f, 250f); //ID 

            //print the description box
            clone6 = Instantiate(prefabMain, new Vector3(pos2, pos0, 5.56f), Quaternion.identity) as GameObject;
            clone6.transform.localScale = new Vector3(230f, 100f, 250f); //Des

            //ID 
            clone7 = Instantiate(textPrefab, mzpos0, Quaternion.identity) as GameObject;

            clone7.GetComponent<UnityEngine.TextMesh>().text = myObject.requirementModel.requirements[x].requirementId.ToString();
            clone7.transform.position = mzpos0;
            clone7.GetComponent<UnityEngine.TextMesh>().fontSize = 900;
            clone7.GetComponent<UnityEngine.TextMesh>().fontStyle = FontStyle.Bold;
            clone7.GetComponent<UnityEngine.TextMesh>().color = Color.cyan;

            //Description
            clone8 = Instantiate(textPrefab, mzpos0, Quaternion.identity) as GameObject;

            clone8.GetComponent<UnityEngine.TextMesh>().text = myObject.requirementModel.requirements[x].description.ToString();
            clone8.transform.position = mzpos1;
            clone8.GetComponent<UnityEngine.TextMesh>().fontSize = 800;
            clone8.GetComponent<UnityEngine.TextMesh>().fontStyle = FontStyle.Bold;
            clone8.GetComponent<UnityEngine.TextMesh>().color = Color.cyan;

          




            //prints subrequirements
            for (int i = 0; i < myObject.requirementModel.requirements[x].subRequirements.Count; i++)
            {






                pos0 -= 1;

                //print the ID box
                clone9 = Instantiate(prefabMain, new Vector3(pos1, pos0, 5.5f), Quaternion.identity) as GameObject;
                clone9.transform.localScale = new Vector3(80f, 100f, 250f); //ID 

                //print the description box
                clone10 = Instantiate(prefabMain, new Vector3(pos2, pos0, 5.56f), Quaternion.identity) as GameObject;
                clone10.transform.localScale = new Vector3(230f, 100f, 250f); //Des

                //pushing the elements downwards
                mzpos0.y -= 1f;

                mzpos1.y -= 1f;


                //ID 
                clone11 = Instantiate(textPrefab, mzpos0, Quaternion.identity) as GameObject;

                clone11.GetComponent<UnityEngine.TextMesh>().text = myObject.requirementModel.requirements[x].subRequirements[i].requirementId.ToString();
                clone11.transform.position = mzpos0;
                clone11.GetComponent<UnityEngine.TextMesh>().fontSize = 900;
                clone11.GetComponent<UnityEngine.TextMesh>().fontStyle = FontStyle.Bold;
                clone11.GetComponent<UnityEngine.TextMesh>().color = Color.magenta;

                //Description
                clone12 = Instantiate(textPrefab, mzpos0, Quaternion.identity) as GameObject;

                clone12.GetComponent<UnityEngine.TextMesh>().text = myObject.requirementModel.requirements[x].subRequirements[i].description.ToString();
                clone12.transform.position = mzpos1;
                clone12.GetComponent<UnityEngine.TextMesh>().fontSize = 800;
                clone12.GetComponent<UnityEngine.TextMesh>().fontStyle = FontStyle.Bold;
                clone12.GetComponent<UnityEngine.TextMesh>().color = Color.magenta;






                //connection between sub requirements as a SOURCE
                for (int p = 0; p < sourceList.Count; p++)
                {                    if (myObject.requirementModel.requirements[x].subRequirements[i].requirementId == sourceList[p])
                    {

                        mzpos0.z = 5.56f;
                        sourcePos.Add(mzpos0);
                        mzpos0.z = 5.5f;



                    }

                }

                //for target
                for (int w = 0; w < sourceList.Count; w++)
                {
                    if (myObject.requirementModel.requirements[x].subRequirements[i].requirementId == targetList[w])
                    {

                        mzpos0.z = 5.56f;
                        sourcePos.Add(mzpos0);  //sourcePos will contain both target and source to which the trace box will connect to
                        mzpos0.z = 5.5f;



                    }

                }

              
            }

            //Draw a line using a line renderer since now we have a source and a target

            DrawLines();
          


            //clear the flows arrays for connection between the next system element
            sourcePos.Clear();
           

            pos0 -= 1;

            mzpos0.y -= 1f;

            mzpos1.y -= 1f;


            pos3 += 50f;

            //increase size of the mega square and move it down by negation of -0.33, dynamic sizing according to the number of requirements

            megaSquare.gameObject.transform.localScale = new Vector3(350f, pos3, 350f);
            pos4 -= 0.33f;
            megaSquare.transform.position = new Vector3(0.55f, pos4, 5.61f);

        }


        GoCalledInfo();






    }


    void DrawLines()
    {
        GameObject newLineGen = Instantiate(LinePrefab);
        LineRenderer LRend = newLineGen.GetComponent<LineRenderer>();

        LRend.positionCount = sourcePos.Count*2;
   
        for(int i = 0; i < sourcePos.Count; i++)
        {
            LRend.SetPosition(dec, myposition[0]);
            dec += 1;
            LRend.SetPosition(dec, sourcePos[i]);
            dec += 1;
        }
       
    }



   

    void GoCalledInfo()
    {

        string newStr2 = "Recieved Project ID: " + p_ID + "\n" + "Received Project Name: " + p_pName + "\n" + "Requirements model last modified: " + rm_lastm;

        txt.GetComponent<UnityEngine.TextMesh>().text = newStr2;


    }





    void GoCalledProgress()
    {
        //ProgressB.SetActive(true);
        StartCoroutine(ExecuteAfterTime(5));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        //ProgressB.SetActive(false);
        //reload the whole scene 
    }
}
