namespace Core
{
    public interface IBundleCost
    {
        bool IsCanBuy();
        
        void Apply();
    }
}
