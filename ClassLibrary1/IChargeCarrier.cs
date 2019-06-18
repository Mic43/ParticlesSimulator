namespace ClassLibrary1
{
    public interface IChargeCarrier<out T> where T:struct
    {
        T Charge { get; }
    }
}