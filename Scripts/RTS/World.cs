namespace RTS;

public partial class World : Node
{
    // Static is convienant but arguably makes code more confusing to read / decouple
    public static World Instance { get; private set; }

    public static List<Atlas> Atlases { get; } = new();

    public static int ChunkSize { get; } = 10;
    public static int TileSize { get; } = 16;
    public static int SpawnRadius { get; } = 3;
    public static Dictionary<Vector2I, bool> ChunkGenerated { get; } = new();

    [Export] public TileMap Grass { get; set; }
    [Export] public TileMap Trees { get; set; }

    Dictionary<string, AtlasWeight> tileDataGrass { get; } = TransformWeightsToRange(new()
    {
        { "grass_1", new AtlasWeight(new Vector2I(3, 1), 10f) },
        { "grass_2", new AtlasWeight(new Vector2I(0, 8), 10f) }
    });

    Dictionary<string, AtlasWeight> tileDataTrees { get; } = TransformWeightsToRange(new()
    {
        { "tree_1",  new AtlasWeight(new Vector2I(6, 4), 10f) }
    });

    public override void _Ready()
    {
        Instance = this;

        var grassNoise = new FastNoiseLite
        {
            Frequency = 0.1f
        };

        var treeNoise = new FastNoiseLite
        {
            Frequency = 0.3f,
            Offset = new Vector3(1000, 0, 0)
        };

        Atlases.Add(new(Grass, grassNoise, tileDataGrass));
        Atlases.Add(new(Trees, treeNoise, tileDataTrees));

        GenerateSpawn();
    }

    public void GenerateChunk(int x, int y)
    {
        World.ChunkGenerated[new Vector2I(x, y)] = true;
        new Chunk(x, y);
    }

    void GenerateSpawn()
    {
        for (int x = -SpawnRadius; x <= SpawnRadius; x++)
        {
            for (int y = -SpawnRadius; y <= SpawnRadius; y++)
            {
                new Chunk(x, y);
            }
        }
    }

    public static Dictionary<string, AtlasWeight> TransformWeightsToRange(Dictionary<string, AtlasWeight> dictionary)
    {
        Dictionary<string, AtlasWeight> result = new();

        float totalWeight = 0f;
        foreach (var pair in dictionary) totalWeight += pair.Value.Weight;
    
        // Set current value to the lowerbound of the range
        float currentValue = -1;

        // Transform the weights to be between [-1, 1], as per FastNoiseLite range
        foreach (var pair in dictionary)
        {
            AtlasWeight atlasWeight = pair.Value;
            float weight = atlasWeight.Weight;
    
            currentValue += weight / totalWeight * 2;
    
            result.Add(pair.Key, new AtlasWeight(atlasWeight.TilePosition, currentValue));
        }
        return result;
    }

}
