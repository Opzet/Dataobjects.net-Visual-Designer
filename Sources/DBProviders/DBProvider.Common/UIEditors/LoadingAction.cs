namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public class LoadingAction
    {
        private readonly object sync = new object();
        private int counter;

        public object SyncRoot
        {
            get { return this.sync; }
        }

        public bool IsActive { get; private set; }

        //public bool ShowProgressMarquee { get; set; }

        public LoadingAction()
        {
            this.counter = 0;
        }

        public bool SetActive()
        {
            counter++;
            if (!IsActive)
            {
                IsActive = true;
            }
            return counter == 1;
        }

        public bool Deactive()
        {
            counter--;
            if (counter < 0)
            {
                counter = 0;
            }

            if (counter == 0)
            {
                IsActive = false;
            }

            return !IsActive;
        }
    }
}