namespace ImageSorter
{
    public class Form1_LogicBase<T>
    {
        public Form1 form1;
        public T logic { get; set; }
        public Form1_LogicBase(T logic, Form1 formInstance)
        {
            this.logic = logic;
            form1 = formInstance;
        }
    }
}
