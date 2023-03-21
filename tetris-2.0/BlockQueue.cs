namespace tetris_2._0;

public class BlockQueue
{
    private readonly Block[] _blocks = new Block[]
    {
        new IBlock(),
        new JBlock(),
        new LBlock(),
        new OBlock(),
        new SBlock(),
        new TBlock(),
        new ZBlock()
    };
    
    private readonly Random _random = new Random();
    
    public Block NextBlock { get; private set; }

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    private Block RandomBlock()
    {
        return _blocks[_random.Next(_blocks.Length)];
    }

    public Block GetAndUpdate()
    {
        Block block = NextBlock;

        do
        {
            NextBlock = RandomBlock();
        } while (block.Id == NextBlock.Id);

        
        return block;
    }
}