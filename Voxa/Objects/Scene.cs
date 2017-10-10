using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Rendering.Uniforms;
using Voxa.Utils;

namespace Voxa.Objects
{
    public class Scene
    {
        public AmbientLight SceneLight = new AmbientLight(Color4.White, 1);

        protected GameObject     activeCamera;
        private List<GameObject> gameObjectList;

        private Vector3Uniform ambientLightColorUniform;
        private FloatUniform ambientLightStrengthUniform;

        public Scene()
        {
            this.gameObjectList = new List<GameObject>();
        }

        public void AddGameObject(GameObject gameObject)
        {
            this.gameObjectList.Add(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            this.gameObjectList.Remove(gameObject);
        }

        public Light GetClosestLight(GameObject target)
        {
            SortedDictionary<float, Light> lights = new SortedDictionary<float, Light>();
            foreach (GameObject go in this.gameObjectList) {
                Light light = go.GetComponent<Light>();
                if (light != null) {
                    lights.Add(target.Transform.Position.Distance(go.Transform.Position), light);
                }
            }
            return (lights.Count > 0) ? lights.First().Value : null;
        }

        public void SetShadingUniforms()
        {
            this.ambientLightColorUniform = new Vector3Uniform("ambientLightColor", this.SceneLight.Color);
            this.ambientLightColorUniform.Set(Engine.RenderingPool.ShaderProgam);
            this.ambientLightStrengthUniform = new FloatUniform("ambientLightStrength", this.SceneLight.Strength);
            this.ambientLightStrengthUniform.Set(Engine.RenderingPool.ShaderProgam);
        }

        public virtual void Load()
        {
            foreach (GameObject gameObject in this.gameObjectList) {
                gameObject.OnLoad();
            }
            this.activeCamera.OnLoad();
        }

        public virtual void UpdateAll()
        {
            foreach (GameObject gameObject in this.gameObjectList)
            {
                gameObject.Update();
            }
            this.activeCamera.Update();
        }

        public GameObject GetActiveCameraGameObject()
        {
            return this.activeCamera;
        }

        public Camera GetActiveCamera()
        {
            return this.activeCamera.GetComponent<Camera>();
        }

        public int GetGameObjectCount()
        {
            return this.gameObjectList.Count;
        }
    }
}
