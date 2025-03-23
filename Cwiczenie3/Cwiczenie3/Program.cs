namespace Cwiczenie3;

public class Program
{
    public static void Main(string[] args)
    {
        {
            var lCon1 = new LiquidContainer("L", 1500.0, 200.0, 2.5, 5.0, false);
            var lCon2 = new LiquidContainer("L", 1500.0, 200.0, 2.5, 5.0, true);
            var gCon1 = new GasContainer("G", 1000.0, 150.0, 2.0, 8.0, 12.0);
            var gCon2 = new GasContainer("G", 1000.0, 150.0, 2.0, 8.0, 12.0);
            var cCon1 = new FridgeContainer("C", 2000.0, 500.0, 3.0, 10.0, 13.3);
            var cCon2 = new FridgeContainer("C", 2000.0, 500.0, 3.0, 10.0, -15.0);

            var ship1 = new ContainerShip(100, 200, 35000.0);
            var ship2 = new ContainerShip(100, 200, 35000.0);

            lCon1.Entrain(1300.0);
            // lCon2.Entrain(2000.0); // Overfill exception test
            lCon2.Entrain(700.0);
            gCon1.Entrain(500.0);
            gCon2.Entrain(1000.0);
            cCon1.Entrain(1500.0, "Bananas");
            // cCon2.Entrain(1500.0, "Bananas"); // Temperature exception
            cCon2.Entrain(1000.0, "Meat");
            
            var containerList = new List<Container>([lCon1, lCon2, gCon1, gCon2]);
            ship1.AddContainer(cCon1);
            ship1.AddContainers(containerList);
            
            ship1.RemoveContainer(lCon2.serialNumber);
            lCon1.Emptying();
            
            ship1.ReplaceContainer(lCon1.serialNumber, lCon2);
            ship1.MoveContainerToOtherShip(gCon1.serialNumber, ship2);
            
            Console.WriteLine(lCon1);
            Console.WriteLine(ship1);
        }
        // bool run = true;
        // while (run)
        // {
        //     List<ContainerShip> shipList = [];
        //     List<Container> containerList = new List<Container>();
        //     
        //     Console.WriteLine("Lista kontenerowców:");
        //     if (shipList.Count == 0)
        //     {
        //         Console.WriteLine("Brak");
        //     }
        //     else
        //     {
        //         foreach (var ship in shipList)
        //         {
        //             Console.WriteLine(ship);
        //         }
        //     }
        //
        //     Console.WriteLine("Lista kontenerów:");
        //     if (containerList.Count == 0)
        //     {
        //         Console.WriteLine("Brak");
        //     }
        //     else
        //     {
        //         foreach (var container in containerList)
        //         {
        //             Console.WriteLine(container);
        //         }
        //     }
        //     
        //     Console.WriteLine("Możliwe akcje:");
        //     Console.WriteLine("0. Zakończ działanie");
        //     Console.WriteLine("1. Dodaj kontenerowiec");
        //     Console.WriteLine("2. Usuń kontenerowiec");
        //     Console.WriteLine("3. Dodaj kontener");
        //     Console.WriteLine("4. Umieść kontener na statku");
        //     Console.WriteLine("5. Usuń kontener");
        //     string? input = Console.In.ReadLine();
        //     switch (input)
        //     {
        //         // case "1":
        //         default:
        //             run = false;
        //             break;
        //     }
        // }
    }
}

public class ContainerShip
{
    private static int _counter;
    private int id { get; set; }
    public List<Container?> containers { get; private set; }
    public double maxSpeed { get; private set; }
    private int maxNumOfContainers { get; set; }
    private double maxWeightOfContainers { get; set; } // in tones

    public ContainerShip(double maxSpeed, int maxNumOfContainers, double maxWeightOfContainers)
    {
        this.containers = [];
        this.id = ++_counter;
        this.maxSpeed = maxSpeed;
        this.maxNumOfContainers = maxNumOfContainers;
        this.maxWeightOfContainers = maxWeightOfContainers;
    }

    private Container? GetConBySn(string serialNumber)
    {
        foreach (var container in containers)
        {
            if(container?.serialNumber == serialNumber) return container;
        }
        return null;
    }

    public void AddContainer(Container? container)
    {
        this.containers.Add(container);
    }

    public void AddContainers(List<Container> addedContainers)
    {
        this.containers.AddRange(addedContainers);
    }

    public void RemoveContainer(string oldContainerSn)
    {
        this.containers.Remove(GetConBySn(oldContainerSn));
    }

    public void ReplaceContainer(string oldContainerSn, Container? newContainer)
    {
        this.containers[this.containers.IndexOf(GetConBySn(oldContainerSn))] = newContainer;
    }

    public void MoveContainerToOtherShip(string containerSn, ContainerShip containerShip)
    {
        Container? con = GetConBySn(containerSn);
        this.containers.Remove(con);
        containerShip.containers.Add(con);
    }

    public override string ToString()
    {
        return $"Container ship {id} | containers: {containers.Count}/{maxNumOfContainers} | max weight of containers: {maxWeightOfContainers} t | max speed: {maxSpeed} |"; 
    }
}

public class Container
{
    private static int _counter;
    
    public double loadWeight { get; protected set; } // in kg
    protected double maxLoadWeight { get; private set; } // in kg
    public double selfWeight { get; private set; } // in kg
    
    public double height { get; private set; } // in cm
    public double depth { get; private set; } // in cm
    public string serialNumber { get; private set; }

    public Container(string containerType, double maxLoadWeight, double selfWeight, double height,
        double depth) : this(containerType)
    {
        this.maxLoadWeight = maxLoadWeight;
        this.selfWeight = selfWeight;
        this.height = height;
        this.depth = depth;
    }

    public Container(string containerType)
    {
        this.serialNumber = GenerateSerialNumber(containerType);
    }
    
    public override string ToString()
    {
        return $"{serialNumber} | load weight {loadWeight} kg | max load weight {maxLoadWeight} kg | self weight {selfWeight} kg | height {height} m | depth {depth} m";
    }

    private static string GenerateSerialNumber(string conType)
    {
        return $"KON-{conType}-{++_counter}";
    }

    public virtual void Emptying()
    {
        this.loadWeight = 0;
    }

    public virtual void Entrain(double weight)
    {
        if (weight + this.loadWeight > this.maxLoadWeight)
        {  
            throw new OverfillException(this.serialNumber);
        }
        this.loadWeight = weight;
    }

    
}

public class LiquidContainer : Container, IHazardNotifier
{
    public bool hazardous { get; private set; }

    public LiquidContainer(string containerType, double maxLoadWeight, double selfWeight, double height, double depth, bool hazardous) : base(containerType, maxLoadWeight, selfWeight, height, depth)
    {
        this.hazardous = hazardous;
    }

    public override string ToString()
    {
        return base.ToString() + " | hazardous: " + hazardous;
    }

    public override void Entrain(double weight)
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

        if (weight + loadWeight > tmpMaxLoadWeight)
        {
            SendNotification("overfill try");
            throw new OverfillException(this.serialNumber);
        }
        this.loadWeight = weight;
    }
    
    public string SendNotification(string notification)
    {
        return this.serialNumber + ": " + notification;
    }
}

public class GasContainer : Container, IHazardNotifier
{
    public double pressure { get; private set; }

    public GasContainer(string containerType, double maxLoadWeight, double selfWeight, double height, double depth, double pressure) : base(containerType, maxLoadWeight, selfWeight, height, depth)
    {
        this.pressure = pressure;
    }
    
    public override string ToString()
    {
        return base.ToString() + " | pressure: " + pressure;
    }

    public override void Emptying()
    {
        this.loadWeight = maxLoadWeight * 0.05;
    }

    public string SendNotification(string notification)
    {
        return this.serialNumber + ": " + notification;
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
    
    public double temperature { get; private set; }
    public string contentType { get; private set; } = "";

    public FridgeContainer(string containerType, double maxLoadWeight, double selfWeight, double height, double depth, double temperature) : base(containerType, maxLoadWeight, selfWeight, height, depth)
    {
        this.temperature = temperature;
    }

    public void Entrain(double weight, string contentType)
    {
        this.contentType = contentType;
        if (contentTemperatures[contentType] > temperature)
        {
            throw new Exception($"{this.serialNumber}: Content temperature is greater than temperature");
        }
        base.Entrain(weight);
    }

    public override string ToString()
    {
        return base.ToString() + " | temperature: " + temperature + " | content type: " + contentType;
    }
}

public interface IHazardNotifier
{
    string SendNotification(string notification);
}

public class OverfillException : Exception
{
    public OverfillException()
    {
        Console.WriteLine("The weight of content is bigger than maximum acceptable weight");
    }
    public OverfillException(string message) : base(message)
    {
        Console.WriteLine($"{message}: the weight of content is bigger than maximum acceptable weight");
    }
    // public OverfillException(string message, Exception inner) : base(message, inner)
    // {
    // }
}