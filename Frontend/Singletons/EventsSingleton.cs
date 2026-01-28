namespace Frontend.Singletons
{
    public delegate void VlanChangedHandler();
    public delegate void LldpPortChangedHandler();
    public class EventsSingleton
    {
        private EventsSingleton() { }

        public static EventsSingleton Instance = new EventsSingleton();

        public event VlanChangedHandler? VlanChanged;
        public event LldpPortChangedHandler? LldpPortChanged;

        public void InvokeVlanChangeHandler()
        {
            this.VlanChanged?.Invoke();
        }

        public void InvokeLldpPortHandler()
        {
            this.LldpPortChanged?.Invoke();
        }
    }
}
