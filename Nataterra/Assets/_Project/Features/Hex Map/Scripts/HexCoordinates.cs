using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{

    public int X { get; private set; }

    public int Z { get; private set; }

    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public HexCoordinates(int x, int z)
    {
        X = x;
        Z = z;
    }



    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z - (x - (x & 1)) / 2);
    }

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = (2f / 3f * position.x) / HexMetrics.outerRadius;
        float z = (-1f / 3f * position.x + Mathf.Sqrt(3f) / 3f * position.z) / HexMetrics.outerRadius;
        float y = -x - z;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iZ);
    }

    public static Vector3 ToPosition(HexCoordinates coordinates)
    {
        float x = HexMetrics.outerRadius * 1.5f * coordinates.X;
        float z = HexMetrics.outerRadius * Mathf.Sqrt(3f) * (coordinates.Z + coordinates.X * 0.5f);

        return new Vector3(x, 0f, z);
    }



    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Z.ToString();
    }
}