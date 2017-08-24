using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxa.Objects
{
    class Component
    {
        protected GameObject gameObject;

        public virtual void Attach(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void Detach(GameObject gameObject)
        {
            this.gameObject = null;
        }

        public GameObject GetGameObject()
        {
            return this.gameObject;
        }

        public virtual void OnUpdate()
        {}

        public virtual void OnLoad()
        {}

        public virtual void OnDestroy()
        {}

        ~Component()
        {
            this.OnDestroy();
        }
    }
}
