public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var x1 = new Container(3.0,"L");
        var x2 = new Container(3.0,"C");
        Console.WriteLine(x1.serialNumber);
        Console.WriteLine(x2.serialNumber);
    }
}

public class Container
{
    private static int counter = 0;
    
    public double loadWeight { get; private set; } = 0; // in kg
    public double maxLoadWeight { get; private set; } = 0; // in kg
    public double selfWeight { get; private set; } = 0; // in kg
    
    public double height { get; private set; } = 0; // in cm
    public double depth { get; private set; } = 0; // in cm
    public string serialNumber { get; private set; } = "";

    public Container()
    {
    }

    public Container(double loadWeight, String conType)
    {
        this.loadWeight = loadWeight;
        this.serialNumber = GenerateSerialNumber(conType);
    }

    private static String GenerateSerialNumber(String conType)
    {
        return $"KON-{conType}-{++counter}";
    }

    private void Emptying()
    {
        this.loadWeight = 0;
    }

    private void Entrain()
    {
        
    }
    
}

public class LiquidContainer : Container, IHazardNotifier
{
    
    
}

public class GasContainer : Container, IHazardNotifier
{
    
}

public class FridgeContainer : Container
{
    
}

public interface IHazardNotifier
{
    
}

