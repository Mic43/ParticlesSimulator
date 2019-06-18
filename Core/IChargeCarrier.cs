namespace Core
{
    public interface IChargeCarrier<out T> where T:struct
    {
        T Charge { get; }
    }
}