namespace SysbotMemoryViewer
{
    public class Parameter<T>
    {
        public T Value { get; set; }

        public Parameter(T val)
        {
            Value = val;
        }

    }
}
