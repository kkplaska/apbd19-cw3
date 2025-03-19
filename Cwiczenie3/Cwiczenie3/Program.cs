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
    
    public double loadWeight { get; protected set; } = 0; // in kg
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

    public virtual void Emptying()
    {
        this.loadWeight = 0;
    }

    public virtual void Entrain(double loadWeight)
    {
        if (loadWeight > this.maxLoadWeight)
        {  
            throw new OverfillException();
        }
        this.loadWeight = loadWeight;
    }
    
}

public class LiquidContainer : Container, IHazardNotifier
{
    public bool hazardous { get; private set; } = false;

    public LiquidContainer(double loadWeight, string conType, bool hazardous) : base(loadWeight, conType)
    {
        this.hazardous = hazardous;
    }

    public override void Entrain(double loadWeight)
    {
        double tmpMaxLoadWeight = maxLoadWeight;
        if (hazardous)
        {
            tmpMaxLoadWeight /= 2;
        }
        else
        {
            tmpMaxLoadWeight *= 0.9;
        }

        if (loadWeight > tmpMaxLoadWeight)
        {
            throw new OverfillException();
        }
        base.loadWeight = loadWeight;
    }
}

public class GasContainer : Container, IHazardNotifier
{
    public GasContainer(double loadWeight, string conType) : base(loadWeight, conType)
    {
        
    }

    public override void Emptying()
    {
        base.loadWeight = maxLoadWeight * 0.05;
    }
}

public class FridgeContainer : Container
{
    public static Dictionary<string, double>  contentTemperatures { get; private set; } = new Dictionary<string, double>()
    {
        {"Bananas", 13.3},
        {"Chocolate", 18.0},
        {"Fish", 2.0},
        {"Meat", -15.0},
        {"Ice cream", -18.0},
        {"Frozen pizza", -30.0},
        {"Cheese", 7.2},
        {"Sausages", 5},
        {"Butter", 20.5},
        {"Eggs", 19.0}
    };
    
    public double temperature { get; private set; } = 0.0;
    public string contentType { get; private set; } = "";

    public FridgeContainer(double loadWeight, string conType, double temperature, string contentType) : base(loadWeight, conType)
    {
        this.temperature = temperature;
        this.contentType = contentType;
        if (contentTemperatures[contentType] > temperature)
        {
            throw new Exception("Content temperature is greater than temperature");
        }

        

    }
}

public interface IHazardNotifier
{
    
}

public class OverfillException : Exception
{
    public OverfillException()
    {
        Console.WriteLine("Masa ładunku jest większa niż pojemność kontenera");
    }
    public OverfillException(string message) : base(message)
    {
    }
    public OverfillException(string message, Exception inner) : base(message, inner)
    {
    }
}
