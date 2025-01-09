public enum InterceptionType
{
    Basic,
    ItemSwap,
}

public interface IMouseIntercept
{
    public InterceptionType GetInterceptionType();
}