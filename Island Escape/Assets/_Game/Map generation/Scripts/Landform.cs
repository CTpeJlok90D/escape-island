public class Landform
{
    private float[,] _value;

    public float[,] Values => (float[,])_value.Clone();

    internal Landform(float[,] value)
    {
        _value = value;
    }

    public override string ToString()
    {
        string result = "";

        for (int x = 0; x < _value.GetLength(0); x++)
        {
            for (int y = 0; y < _value.GetLength(1); y++)
            {
                result += $"{_value[x, y]}]";
            }

            result += "\n";
        }

        return result;
    }
}