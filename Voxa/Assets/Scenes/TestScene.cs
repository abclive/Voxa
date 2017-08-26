using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Objects;
using Voxa.Logic;
using Voxa.Primitives.Shape;
using Voxa.Rendering;
using Voxa.Objects.Renderer;
using Voxa.Utils;
using Voxa.Assets.Scripts;
using OpenTK.Graphics;

namespace Voxa.Assets.Scenes
{
    class TestScene : Scene
    {
        public TestScene()
        {
            // Setup camera
            this.activeCamera = new GameObject("Camera");
            Camera renderCamera = new Camera();
            this.activeCamera.AttachComponent(renderCamera);
            Engine.RenderingPool.SetRenderCamera(renderCamera);
            this.activeCamera.AttachComponent(new FirstPersonCamera());
            this.SceneLight.Color = Color4.White;
            this.SceneLight.Strength = 0.25f;
        }

        public override void Load()
        {
            GameObject testModel = new GameObject("Model test");
            GLTFLoader modelLoader = new GLTFLoader("Voxa.Assets.Models", "CesiumMilkTruck.gltf");

            StaticModel modelComponent = new StaticModel(modelLoader.GetAllMeshes().ToArray(), modelLoader.GetAllMaterials().ToArray());
            Logger.Info("Successfully loaded model");
            testModel.AttachComponent(modelComponent);
            testModel.AttachComponent(new ModelRenderer(modelComponent));
            testModel.AttachComponent(new RotateTest());

            this.AddGameObject(testModel);

            //GameObject truck = new GameObject("Model test");
            //modelLoader = new GLTFLoader("Voxa.Assets.Models", "Hotel.gltf");

            //modelComponent = new StaticModel(modelLoader.GetAllMeshes().ToArray(), modelLoader.GetAllMaterials().ToArray());
            //Logger.Info("Successfully loaded model");
            //truck.AttachComponent(modelComponent);
            //truck.AttachComponent(new ModelRenderer(modelComponent));
            //truck.Transform.Position = new Vector3(30, 0, 0);
            //truck.Transform.Scale = new Vector3(0.2f, 0.2f, 0.2f);

            //this.AddGameObject(truck);

            GameObject light = new GameObject("Test light");
            light.AttachComponent(new Light(Color4.White));
            light.AttachComponent(new LightTest());
            this.AddGameObject(light);

            base.Load();
        }
    }
}
