using FlatBuffers;
using System;

namespace FlatBuffersPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTest();
            GC.Collect();
            RunTest();
            GC.Collect();
            RunTest();
            GC.Collect();
            RunTest();
            GC.Collect();
            RunTest();
            GC.Collect();
            RunTest();
            GC.Collect();
        }

        private static void RunTest()
        {
            FlatBufferBuilder builder = new FlatBufferBuilder(1024 * 1024 * 32);
            var start = System.DateTime.UtcNow;

            for (int x = 0; x < 1000000; ++x)
            {
                var offset = builder.CreateString("T");
                builder.StartObject(4);
                builder.AddDouble(3.2);
                builder.AddDouble(4.2);
                builder.AddDouble(5.2);
                builder.AddOffset(offset.Value);
                builder.EndObject();
                ++x;
            }

            var end = System.DateTime.UtcNow;

            var position = builder.DataBuffer.Position;
            var length = builder.DataBuffer.Length;
            var size = length - position;
            Console.WriteLine(size.ToString() + ", " + (end - start).TotalMilliseconds.ToString());
        }
    }
}
