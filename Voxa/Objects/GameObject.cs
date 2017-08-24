using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Voxa.Objects
{
    class GameObject
    {
        private List<Component> componentList;
        public string           Name { get; private set; }
        public Transform        Transform { get; private set; }

        public GameObject()
        {
            this.Transform = new Transform();
            this.Transform.Attach(this);
            this.componentList = new List<Component>();
        }

        public GameObject(string name)
        {
            this.Transform = new Transform();
            this.Transform.Attach(this);
            this.componentList = new List<Component>();
            this.Name = name;
        }

        public GameObject(Vector3 position)
        {
            this.Transform = new Transform();
            this.Transform.Attach(this);
            this.componentList = new List<Component>();
            this.Transform.Position = position;
        }

        public GameObject(Vector3 position, string name)
        {
            this.Transform = new Transform();
            this.Transform.Attach(this);
            this.componentList = new List<Component>();
            this.Transform.Position = position;
            this.Name = name;
        }

        public void AttachComponent(Component component)
        {
            this.componentList.Add(component);
            component.Attach(this);
        }

        public void DetachComponent(Component component)
        {
            this.componentList.Remove(component);
            component.Detach(this);
        }

        public TComponent GetComponent<TComponent>()
        {
            return this.componentList.OfType<TComponent>().FirstOrDefault();
        }

        public void Update()
        {
            foreach (Component component in componentList) {
                component.OnUpdate();
            }
            this.OnUpdate();
        }

        public virtual void OnLoad()
        {
            foreach (Component component in componentList) {
                component.OnLoad();
            }
        }

        public virtual void OnUpdate()
        {}

        public virtual void OnDestroy()
        {}

        ~GameObject()
        {
            this.OnDestroy();
        }
    }
}
