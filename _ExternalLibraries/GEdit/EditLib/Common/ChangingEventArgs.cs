namespace EditLib
{
    public class ChangingEventArgs
    {
        public string ChangingName { get; set; }

        public ChangingEventArgs(string changingName)
        {
            this.ChangingName = changingName;
        }
    }
}
