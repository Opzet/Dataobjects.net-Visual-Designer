using System;
using System.Collections.Generic;
using System.Threading;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public class ControlSync
    {
        private bool running = false;
        private readonly Thread syncThread;

        private readonly Queue<AsyncCallbackItem> queue = new Queue<AsyncCallbackItem>();
        private readonly object queueSync = new object();
        private readonly AutoResetEvent callbackAvailable = new AutoResetEvent(false);
        private readonly IControlSyncUIContext uiContext;
        private static ControlSync instance;

        public static ControlSync Instance
        {
            get { return instance; }
        }

        public static void Initialize(IControlSyncUIContext uiContext)
        {
            if (instance != null)
            {
                instance.Stop();
            }

            instance = new ControlSync(uiContext);
            instance.Start();
        }

        private ControlSync(IControlSyncUIContext uiContext)
        {
            this.uiContext = uiContext;
            this.syncThread = new Thread(Sync);
        }

        public void Start()
        {
            running = true;
            syncThread.Start();
        }

        public void Stop()
        {
            running = false;
            syncThread.Join(20 * 1000);
        }

        private void Sync()
        {
            while (running)
            {
                bool eventSignaled = callbackAvailable.WaitOne(1000);
                if (eventSignaled)
                {
                    AsyncCallbackItem[] callbackItems = null;
                    lock (queueSync)
                    {
                        if (queue.Count > 0)
                        {
                            callbackItems = queue.ToArray();
                            queue.Clear();
                        }
                    }

                    if (callbackItems != null && callbackItems.Length > 0)
                    {
                        ThreadPool.QueueUserWorkItem(delegate(object state)
                            {
                                ExecuteCallback((AsyncCallbackItem[])state);
                            }, callbackItems);
                    }
                }
            }
        }

        private void ExecuteCallback(AsyncCallbackItem[] callbackItems)
        {
            foreach (var callbackItem in callbackItems)
            {
                Action<AsyncCallbackState> callback = callbackItem.ActionCallback;
                SynchronizationContext synchronizationContext = callbackItem.Context;
                object state = callbackItem.ActionCallbackData;
                Action finallyAction = callbackItem.FinallyAction;

                synchronizationContext.Post(o => uiContext.BeginLoadingAction(true), null);

                try
                {
                    callback(new AsyncCallbackState(synchronizationContext, state));
                }
                finally
                {
                    synchronizationContext.Post(delegate
                        {
                            if (finallyAction != null)
                            {
                                finallyAction();
                            }
                            uiContext.EndLoadingAction();
                        }, null);
                }
            }
        }

        public void Post(Action<AsyncCallbackState> actionCallback, object state)
        {
            Post(actionCallback, state, null);
        }

        public void Post(Action<AsyncCallbackState> actionCallback, object state, Action finallyAction)
        {
            lock (queueSync)
            {
                queue.Enqueue(new AsyncCallbackItem(SynchronizationContext.Current, actionCallback, state, finallyAction));
            }
            callbackAvailable.Set();
        }
    }

    #region class AsyncCallbackItem

    public class AsyncCallbackItem
    {
        internal Action<AsyncCallbackState> ActionCallback;

        internal Action FinallyAction;

        internal object ActionCallbackData;

        internal SynchronizationContext Context;

        public AsyncCallbackItem(SynchronizationContext context, Action<AsyncCallbackState> actionCallback, object actionCallbackData, 
            Action finallyAction)
        {
            this.ActionCallback = actionCallback;
            this.ActionCallbackData = actionCallbackData;
            this.Context = context;
            this.FinallyAction = finallyAction;
        }
    }

    #endregion class AsyncCallbackItem

    #region class AsyncCallbackState

    public class AsyncCallbackState
    {
        private readonly SynchronizationContext context;

        public object State { get; private set; }

        public AsyncCallbackState(SynchronizationContext context, object state)
        {
            this.context = context;
            this.State = state;
        }

        public void InvokeOnUI(SendOrPostCallback callback)
        {
            InvokeOnUI(callback, null);
        }

        public void InvokeOnUI(SendOrPostCallback callback, object state)
        {
            context.Post(callback, state);
        }
    }

    #endregion class AsyncCallbackState

    #region interface IControlSyncUIContext

    public interface IControlSyncUIContext
    {
        void BeginLoadingAction(bool showProgressMarquee);

        void EndLoadingAction();

        /*void UpdateProgressMarquee(bool visible);

        void ShowError(string caption, Exception exception);

        void ShowError(string caption, ErrorCollection errors);

        void ShowInfo(string caption, string message);*/
    }

    #endregion interface IControlSyncUIContext
}