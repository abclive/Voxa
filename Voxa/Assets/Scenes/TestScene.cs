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
            GameObject hotel = new GameObject("Hotel");
            GLTFLoader modelLoader = new GLTFLoader("Voxa.Assets.Models", "Hotel.gltf");

            StaticModel modelComponent = new StaticModel(modelLoader.GetAllMeshes().ToArray(), modelLoader.GetAllMaterials().ToArray());
            Logger.Info("Successfully loaded model");
            hotel.AttachComponent(modelComponent);
            hotel.AttachComponent(new ModelRenderer(modelComponent));
            hotel.Transform.Scale = new Vector3(0.2f, 0.2f, 0.2f);

            GameObject truck = new GameObject("Model test");
            modelLoader = new GLTFLoader("Voxa.Assets.Models", "CesiumMilkTruck.gltf");

            modelComponent = new StaticModel(modelLoader.GetAllMeshes().ToArray(), modelLoader.GetAllMaterials().ToArray());
            Logger.Info("Successfully loaded model");
            truck.AttachComponent(modelComponent);
            truck.AttachComponent(new ModelRenderer(modelComponent));
            truck.AttachComponent(new RotateTest());
            truck.Transform.Position = new Vector3(5, 0, 0);

            this.AddGameObject(truck);
            //truck.Transform.SetParent(hotel.Transform);
            this.AddGameObject(hotel);

            GameObject light = new GameObject("Light");
            light.AttachComponent(new Light(Color4.White));
            light.AttachComponent(new LightTest());
            light.Transform.SetParent(this.activeCamera.Transform);
            this.AddGameObject(light);

            base.Load();
        }
    }
}
