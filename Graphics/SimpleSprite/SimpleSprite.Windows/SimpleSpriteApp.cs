
namespace SimpleSprite
{
    class SimpleSpriteApp
    {
        static void Main(string[] args)
        {
            // Profiler.EnableAll();
            using (var game = new SimpleSpriteGame())
            {
                game.Run();
            }
        }
    }
}
