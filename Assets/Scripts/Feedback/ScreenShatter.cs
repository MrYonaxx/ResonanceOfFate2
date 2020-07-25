/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class ScreenShatter: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [SerializeField]
        RawImage rawImage;


        Texture texture;
        public Material mat;
        List<TriData> triData;

        CanvasRenderer canvasRenderer;

        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        /*private void OnDrawGizmos()
        {
            GL.PushMatrix();
            texture = rawImage.mainTexture;
            mat = rawImage.material;

            GL.LoadOrtho();
            GL.Begin(GL.TRIANGLES); // Triangle
            mat.SetPass(0);
            GL.Color(new Color(1, 1, 1, 1));
            GL.Vertex3(0.50f, 0.25f, 0);
            GL.Vertex3(0.25f, 0.25f, 0);
            GL.Vertex3(0.375f, 0.5f, 0);
            GL.End();
            GL.PopMatrix();
        }*/
        public class TriData
        {
            public List<Vector3> vertices;
            public List<Vector3> uv;
            public Matrix4x4 matrixTransform;

            public Vector3 direction;
            public Vector3 rotation;

            public TriData(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 pointD)
            {
                /*Debug.Log("=============");
                Debug.Log(pointA.x + "   " + pointA.y);
                Debug.Log(pointC.x + "   " + pointC.y);
                Debug.Log(pointB.x + "   " + pointB.y);
                Debug.Log(pointD.x + "   " + pointD.y);
                Debug.Log("=============");*/
                vertices = new List<Vector3>();
                uv = new List<Vector3>();

                /*vertices.Add(new Vector3((pointA.x * 2) - 1, (pointA.y * 2) - 1, 0));
                uv.Add(pointA);

                vertices.Add(new Vector3((pointC.x * 2) - 1, (pointC.y * 2) - 1, 0));
                uv.Add(pointC);

                vertices.Add(new Vector3((pointB.x * 2) - 1, (pointB.y * 2) - 1, 0));
                uv.Add(pointB);

                vertices.Add(new Vector3((pointD.x * 2) - 1, (pointD.y * 2) - 1, 0));
                uv.Add(pointD);*/

                vertices.Add(new Vector3((pointD.x * 2) - 1, (pointD.y * 2) - 1, 0));
                uv.Add(new Vector3(pointD.x, -pointD.y + 1f));
                vertices.Add(new Vector3((pointB.x * 2) - 1, (pointB.y * 2) - 1, 0));
                uv.Add(new Vector3(pointB.x, -pointB.y + 1f));




                vertices.Add(new Vector3((pointA.x * 2) - 1, (pointA.y * 2) - 1, 0));
                uv.Add(new Vector3(pointA.x, -pointA.y + 1f));
                vertices.Add(new Vector3((pointC.x * 2) - 1, (pointC.y * 2) - 1, 0));
                uv.Add(new Vector3(pointC.x, -pointC.y + 1f));



                direction = new Vector2(pointA.x - 0.5f, pointA.y - 0.5f);


                matrixTransform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector4(1, 1, 1));
            }

            public void SetMatrix(Matrix4x4 a)
            {
                matrixTransform = a;
            }
        }

        // Slice a la main tout pourri pour tester /  pas opti du tout
        private void SliceNaive()
        {
            triData = new List<TriData>();
            int lineNumber = 10;
            float xPadding = 0.1f;
            float yPadding = 0.1f;
            bool canCreateTriangle = false;

            List<Vector2> previousPointX = new List<Vector2>();
            List<Vector2> pointX = new List<Vector2>();

            /////////////////    =========   Ligne 0
            for(int i = 0; i < lineNumber; i++)
            {
                if (i == 0)
                    previousPointX.Add(new Vector2(0, 0f));
                else if (i == lineNumber-1)
                    previousPointX.Add(new Vector2(1f, 0f));
                else
                    previousPointX.Add(new Vector2(i * xPadding + Random.Range(-xPadding * 0.8f, xPadding * 0.8f), 0f));
            }
            canCreateTriangle = true;


            ////////////////   =========   Ligne X
            for (int j = 1; j < lineNumber; j++)
            {
                for (int i = 0; i < lineNumber; i++)
                {
                    float y = j * yPadding + Random.Range(-yPadding * 0.8f, yPadding * 0.8f);

                    if (i == 0)
                    {
                        pointX.Add(new Vector2(0, y));
                        continue;
                    }
                    else if (i == lineNumber - 1)
                        pointX.Add(new Vector2(1f, y));
                    else
                        pointX.Add(new Vector2(i * xPadding + Random.Range(-xPadding * 0.8f, xPadding * 0.8f), y));
                    triData.Add(new TriData(previousPointX[i - 1], previousPointX[i], pointX[i - 1], pointX[i]));
                }


                /*if(canCreateTriangle == true)
                {
                    for (int i = 0; i < pointX.Count-1; i++)
                    {
                        
                    }
                }*/
                //canCreateTriangle = !canCreateTriangle;

                previousPointX.Clear();
                for (int i = 0; i < pointX.Count; i++)
                {
                    previousPointX.Add(new Vector2(pointX[i].x, pointX[i].y));
                }
                pointX.Clear();
            }


            /////////////////    =========   Ligne Final
            //previousPointX = pointX;
            ///pointX.Clear();
            /*for (int i = 0; i < lineNumber; i++)
            {
                if (i == 0)
                    pointX.Add(new Vector2(0, 1f));
                else if (i == lineNumber-1)
                    pointX.Add(new Vector2(1f, 1f));
                else
                    pointX.Add(new Vector2(i * xPadding + Random.Range(-xPadding * 0.9f, xPadding * 0.9f), 1f));
            }
            for (int i = 0; i < pointX.Count-1; i++)
            {
                triData.Add(new TriData(previousPointX[i + 1], previousPointX[i], pointX[i + 1], pointX[i]));
            }*/
        }

        private void Start()
        {
            SliceNaive();

            texture = rawImage.mainTexture;
            mat = rawImage.material;

            var shader = Shader.Find("J/Shatter");
            mat = new Material(shader);
            mat.SetFloat("_ScreenRatio", 1);
            mat.SetFloat("_Alpha", 1);
            mat.hideFlags = HideFlags.HideAndDontSave;

            mat.mainTexture = texture;
            Debug.Log(triData.Count);
        }


        private void OnPostRender()
        {
            if (triData == null)
                return;

            GL.PushMatrix();

            mat.SetPass(0);
            GL.LoadOrtho();



            /*GL.Begin(GL.TRIANGLES);

            GL.MultiTexCoord(0, new Vector3(0.5f, 0.5f, 0));
            GL.Vertex3(0f, 0f, 0);

            GL.MultiTexCoord(0, new Vector3(1f, 0.5f, 0));
            GL.Vertex3(1f, 0, 0);

            GL.MultiTexCoord(0, new Vector3(0.75f, 1f, 0));
            GL.Vertex3(0.5f, -1f, 0);

            GL.End();



            GL.Begin(GL.TRIANGLES);

            GL.MultiTexCoord(0, new Vector3(0f, 0.5f, 0));
            GL.Vertex3(-1f, 0f, 0);

            GL.MultiTexCoord(0, new Vector3(0.5f, 0.5f, 0));
            GL.Vertex3(0f, 0, 0);

            GL.MultiTexCoord(0, new Vector3(0.25f, 1f, 0));
            GL.Vertex3(-0.5f, -1f, 0);
            
            GL.End();*/

            for (int i = 0; i < triData.Count; i++)
            {
                GL.Begin(GL.QUADS);

                //Debug.Log("Start");
                for (int j = 0; j < 4; j++)
                {
                    //Debug.Log(triData[i].vertices[j]);
                    GL.MultiTexCoord(0, triData[i].uv[j]);
                    GL.Vertex(triData[i].vertices[j]);
                }
                //Debug.Log("End");


                //triData[i].SetMatrix(triData[i].matrixTransform * Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 0.97f)));
                triData[i].matrixTransform = triData[i].matrixTransform * Matrix4x4.Translate(triData[i].direction * 0.2f);
                triData[i].matrixTransform = triData[i].matrixTransform * Matrix4x4.Rotate(Quaternion.identity);
                triData[i].matrixTransform = triData[i].matrixTransform * Matrix4x4.Scale(new Vector3(1.02f, 1.02f, 1.02f));
                GL.MultMatrix(triData[i].matrixTransform);
                GL.End();
            }

            GL.PopMatrix();
            //for (int i = 0; i < triData.Count; i++)
            //{

            /*var c = m_triData[i].Center;
            c.x *= screenratio;
            m_triData[i].Matrix = Matrix4x4.Translate(m_triData[i].Dir * offset);
            m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Translate(c);
            m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Rotate(Quaternion.Euler(m_triData[i].Rotation * rotation));
            m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 0.97f));
            m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Translate(-c);

            GL.MultMatrix(m_triData[i].Matrix);

            GL.End();*/
            //}



        }

        /*public void ShatterScreen()
        {
            texture = rawImage.mainTexture;
            mat = rawImage.material;
            //rawImage.mainTexture.bl
            //m_triData[index].vertices
            GL.LoadOrtho();
            mat.SetPass(0);


            var shader = Shader.Find("J/Shatter");
            mat = new Material(shader);
            mat.SetFloat("_ScreenRatio", 1);
            mat.SetFloat("_Alpha", 1);
            mat.hideFlags = HideFlags.HideAndDontSave;
            mat.mainTexture = texture;

            GL.Begin(GL.TRIANGLES);
            GL.MultiTexCoord(0, new Vector4(0, 0.2f, 0.3f, 0.5f));
            GL.Vertex(new Vector3(0,0.1f,0.2f));
            //GL.MultMatrix(m_triData[i].Matrix);
            GL.End();
        }*/



        /*IEnumerator RenderTriangles()
        {
            if (texture == null) yield return null;

            float offset = 0;
            float alpha = 1;
            float rotation = 0;
            float transformDelay = 0.5f;
            float transitionTime = 0;

            while (alpha > 0)
            {
                yield return new WaitForEndOfFrame();

                if (!mat)
                {
                    var shader = Shader.Find("J/Shatter");
                    mat = new Material(shader);
                    mat.hideFlags = HideFlags.HideAndDontSave;

                    mat.mainTexture = texture;
                }

                GL.LoadOrtho();
                mat.SetPass(0);

                var screenratio = (float)Screen.width / Screen.height;
                mat.SetFloat("_ScreenRatio", screenratio);
                mat.SetFloat("_Alpha", alpha);

                // Dessine un triangle avec GL
                for (int i = 0; i < m_triData.Count; i++)
                {
                    GL.Begin(GL.TRIANGLES);

                    for (int j = 0; j < 3; j++)
                    {
                        GL.MultiTexCoord(0, m_triData[i].UV[j]);
                        GL.Vertex(m_triData[i].Vertices[j]);
                        //GL.MultiTexCoord(2, m_triData[i].BC[j]);
                    }

                    

                    var c = m_triData[i].Center;
                    c.x *= screenratio;
                    m_triData[i].Matrix = Matrix4x4.Translate(m_triData[i].Dir * offset);
                    m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Translate(c);
                    m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Rotate(Quaternion.Euler(m_triData[i].Rotation * rotation));
                    m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Scale(new Vector3(0.97f, 0.97f, 0.97f));
                    m_triData[i].Matrix = m_triData[i].Matrix * Matrix4x4.Translate(-c);

                    GL.MultMatrix(m_triData[i].Matrix);

                    GL.End();
                }

                if (transformDelay < transitionTime)
                {
                    alpha -= 0.4f * Time.deltaTime;
                    offset += 0.1f * Time.deltaTime;
                    rotation += 0.4f * Time.deltaTime;
                }
                else
                {
                    transitionTime += Time.deltaTime;
                }
            }
        }*/

        #endregion

    } 

} // #PROJECTNAME# namespace