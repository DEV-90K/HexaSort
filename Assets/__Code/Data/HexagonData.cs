using System.Data;

public enum HexagonType
{
    NONE,
    BLUE,
    RED,
    PURPLE,
    YELLOW
}
public class HexagonData
{
    public HexagonData()
    {
    }

    public HexagonType Type { get; private set; }
    public string HexColor { get; private set; }

    public HexagonData(HexagonType type, string hexColor)
    {
        Type = type;
        HexColor = hexColor;
    }
}