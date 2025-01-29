namespace IDCardSystem
{
    public interface IIDCard
    {
        string GetIDCardContent();
        string ObjectName { get; }
        string ObjectType { get; }
    }
    
}