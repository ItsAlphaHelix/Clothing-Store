namespace Clothing_Store.CustomExceptions
{
    public class InvalidSizeException : Exception
    {
        public InvalidSizeException(string message)
            : base(message)
        {
            
        }
    }
}
