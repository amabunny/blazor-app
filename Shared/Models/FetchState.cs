namespace Star_Wars_DataBase.Shared.Models
{
    public struct DerivedState<T>
    {
        public bool Loading;
        public T? Data;
    }
}