public class Landform
{
    private int[,] _value;

    public int[,] Values => (int[,])_value.Clone();

    internal Landform(int[,] value)
    {
        _value = value;
    }
}