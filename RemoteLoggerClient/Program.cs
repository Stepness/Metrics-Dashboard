using System.Diagnostics;

static async Task Main()
{
    Console.WriteLine(GetMemoryStats().MemoryAvailable);
}

static MemoryStatsDto GetMemoryStats()
{
    MemoryStatsDto memoryStatsDto = new MemoryStatsDto();
    string memoryStatsPath = "//proc//meminfo";
    if (File.Exists(memoryStatsPath))
    {
        var file = File.ReadAllLines(memoryStatsPath);

        //Skipping all lines we are not interested in
        for (int i = 0; i < 3; i++)
        {
            int firstOccurenceOfDigit = 0;
            var memoryLine = file[i];
            //index of first number , start the string until the end and store it
            for (int j = 0; j < memoryLine.Length; j++)
            {
                if (Char.IsNumber(memoryLine[j]))
                {
                    firstOccurenceOfDigit = j;
                    break;
                }
            }

            var memoryValue = memoryLine.Substring(firstOccurenceOfDigit);

            switch (i)
            {
                case 0:
                    memoryStatsDto.MemoryTotal = memoryValue;
                    break;
                case 1:
                    memoryStatsDto.MemoryFree = memoryValue;
                    break;
                case 2:
                    memoryStatsDto.MemoryAvailable = memoryValue;
                    break;
                default: break;
            }
        }

        return memoryStatsDto;
    }
    else
    {
        memoryStatsDto.MemoryAvailable = "";
        memoryStatsDto.MemoryFree = "";
        memoryStatsDto.MemoryTotal = "";
        return memoryStatsDto;
    }
}

public class MemoryStatsDto
{
    public string MemoryAvailable { get; set; }
    public string MemoryFree { get; set; }
    public string MemoryTotal { get; set; }
}