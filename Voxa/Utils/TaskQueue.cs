using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxa.Utils
{
    public class TaskQueue
    {
        public abstract class Callback
        {
            public abstract void Call();
        }

        public class Callback<T> : Callback
        {
            private Action<T> action;
            private T argument;

            public Callback(Action<T> action, T argument)
            {
                this.action = action;
                this.argument = argument;
            }

            public override void Call()
            {
                action.Invoke(argument);
            }
        }
        private List<Callback> pendingCallbacks;

        public TaskQueue()
        {
            this.pendingCallbacks = new List<Callback>();
        }

        public void CallOnMainThread(Callback callback)
        {
            this.pendingCallbacks.Add(callback);
        }

        public void CallPendingCallbacks()
        {
            foreach (Callback callback in this.pendingCallbacks.ToList()) {
                callback.Call();
                this.pendingCallbacks.Remove(callback);
            }
        }
    }
}
