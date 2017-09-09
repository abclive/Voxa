using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Voxa.Objects
{
    class Transform : Component
    {
        private Vector3 position;
        private Vector3 eulerRotation;
        private Quaternion rotation;
        private Vector3 scale;

        private Vector3 localPosition;
        private Vector3 localEulerRotation;
        private Quaternion localRotation;
        private Vector3 localScale;

        private Matrix4 localToWorldMatrix = Matrix4.Identity;
        private Matrix4 worldToLocalMatrix = Matrix4.Identity;
        private Transform parent;
        private List<Transform> children;
        private bool isDirty = false;
        private bool isInverseDirty = false;
        private bool isValuesDirty = false;

        public Vector3 Position
        {
            get {
                if (this.isValuesDirty)
                    this.UpdateValuesFromParent();
                return this.position;
            }
            set { this.position = value; this.updateLocalFromWorldPosition(); this.setDirty(); }
        }

        public Vector3 EulerRotation
        {
            get { return this.eulerRotation; }
            set { this.eulerRotation = value; this.rotation = Quaternion.FromEulerAngles(this.eulerRotation); this.updateLocalFromWorldRotation(); this.setDirty(); }
        }

        public Quaternion Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; this.updateLocalFromWorldRotation(); this.setDirty(); }
        }

        public Vector3 Scale
        {
            get { return this.scale; }
            set { this.scale = value; this.updateLocalFromWorldScale(); this.setDirty(); }
        }

        public Vector3 LocalPosition
        {
            get { return this.localPosition; }
            set { this.localPosition = value; this.updateWorldFromLocalPosition(); this.setDirty(); }
        }

        public Vector3 LocalEulerRotation
        {
            get { return this.localEulerRotation; }
            set { this.localEulerRotation = value; this.localRotation = Quaternion.FromEulerAngles(this.localEulerRotation); this.updateWorldFromLocalRotation(); this.setDirty(); }
        }

        public Quaternion LocalRotation
        {
            get { return this.localRotation; }
            set { this.localRotation = value; this.updateWorldFromLocalRotation(); this.setDirty(); }
        }

        public Vector3 LocalScale
        {
            get { return this.localScale; }
            set { this.localScale = value; this.updateWorldFromLocalScale(); this.setDirty(); }
        }

        public Transform()
        {
            this.position = Vector3.Zero;
            this.eulerRotation = Vector3.Zero;
            this.rotation = Quaternion.Identity;
            this.scale = Vector3.One;

            this.localPosition = this.position;
            this.localEulerRotation = this.eulerRotation;
            this.localRotation = this.rotation;
            this.localScale = this.scale;

            this.children = new List<Transform>();
        }

        public void SetParent(Transform parent)
        {
            if (this.parent != null) {
                this.parent.RemoveChild(this);
            }
            this.parent = parent;
            this.parent.AddChild(this);
            this.setDirty();
        }

        public void DetachFromParent()
        {
            if (this.parent != null) {
                this.parent.RemoveChild(this);
            }
            this.parent = null;
        }

        public void RemoveChild(Transform child)
        {
            this.children.Remove(child);
        }

        public void AddChild(Transform child)
        {
            children.Add(child);
        }

        private void setDirty()
        {
            this.isDirty = true;
            this.isInverseDirty = true;
            this.isValuesDirty = true;

            // set all children to be dirty since any modification
            // of a parent transform also effects its children's
            // localToWorldTransform
            foreach (Transform child in this.children) {
                child.setDirty();
            }
        }

        public void UpdateValuesFromParent()
        {
            foreach (Transform child in this.children) {
                child.UpdateValuesFromParent();
            }
            if (this.parent != null) {
                this.position = this.parent.position + this.localPosition;
            }
            this.isValuesDirty = false;
        }

        public Matrix4 GetLocalToParentMatrix()
        {
            Matrix4 translationMatrix = Matrix4.CreateTranslation(this.position);
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(this.rotation);
            Matrix4 scaleMatrix = Matrix4.CreateScale(this.scale);

            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        public Matrix4 GetLocalToWorldMatrix()
        {
            if (this.isDirty) {
                if (this.parent == null) {
                    this.localToWorldMatrix = this.GetLocalToParentMatrix();
                } else {
                    this.localToWorldMatrix = this.parent.GetLocalToWorldMatrix() * this.GetLocalToParentMatrix();
                }
                this.isDirty = false;
            }

            return localToWorldMatrix;
        }

        public Matrix4 GetWorldToLocalMatrix()
        {
            if (this.isInverseDirty) {
                this.worldToLocalMatrix = this.GetLocalToWorldMatrix().Inverted();
                this.isInverseDirty = false;
            }
            return this.worldToLocalMatrix;
        }

        private void updateWorldFromLocalPosition()
        {
            Matrix4 translationMatrix = Matrix4.CreateTranslation(this.localPosition);
            Vector4 updatedPos = new Vector4(this.position, 1) * translationMatrix;
            this.position.X = updatedPos.X;
            this.position.Y = updatedPos.Y;
            this.position.Z = updatedPos.Z;
        }

        private void updateLocalFromWorldPosition()
        {
            Matrix4 translationMatrix = Matrix4.CreateTranslation(this.position);
            Vector4 updatedPos = new Vector4(this.localPosition, 1) * translationMatrix;
            this.localPosition.X = updatedPos.X;
            this.localPosition.Y = updatedPos.Y;
            this.localPosition.Z = updatedPos.Z;
        }

        private void updateWorldFromLocalRotation()
        {
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(this.localRotation);
            Vector4 updatedRotation = new Vector4(this.rotation.X, this.rotation.Y, this.rotation.Z, this.rotation.W) * rotationMatrix;
            this.rotation.X = updatedRotation.X;
            this.rotation.Y = updatedRotation.Y;
            this.rotation.Z = updatedRotation.Z;
            this.rotation.W = updatedRotation.W;
        }

        private void updateLocalFromWorldRotation()
        {
            Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(this.rotation);
            Vector4 updatedRotation = new Vector4(this.localRotation.X, this.localRotation.Y, this.localRotation.Z, this.localRotation.W) * rotationMatrix;
            this.localRotation.X = updatedRotation.X;
            this.localRotation.Y = updatedRotation.Y;
            this.localRotation.Z = updatedRotation.Z;
            this.localRotation.W = updatedRotation.W;
        }

        private void updateWorldFromLocalScale()
        {
            Matrix4 scaleMatrix = Matrix4.CreateScale(this.localScale);
            Vector4 updatedScale = new Vector4(this.scale, 1) * scaleMatrix;
            this.scale.X = updatedScale.X;
            this.scale.Y = updatedScale.Y;
            this.scale.Z = updatedScale.Z;
        }

        private void updateLocalFromWorldScale()
        {
            Matrix4 scaleMatrix = Matrix4.CreateScale(this.scale);
            Vector4 updatedPos = new Vector4(this.localScale, 1) * scaleMatrix;
            this.localScale.X = updatedPos.X;
            this.localScale.Y = updatedPos.Y;
            this.localScale.Z = updatedPos.Z;
        }

        public Transform GetParent()
        {
            return this.parent;
        }
    }
}