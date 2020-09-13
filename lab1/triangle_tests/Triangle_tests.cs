using System;
using System.IO;
using triangle;
using Xunit;

namespace triangle_tests
{
    public class Triangle_tests
    {
        private static string inputFileName = @"..\..\..\input.txt";
        private static string outputFileName = @"..\..\..\output.txt";

        [Fact]
        public void DetermineTriangleType_ArgsFromFile_ReturnsTriangleType()
        {
            using StreamReader input = new StreamReader(inputFileName);
            using StreamWriter output = new StreamWriter(outputFileName);

            uint testNumber = 0;
            string line;
            while ((line = input.ReadLine()) != null)
            {
                string[] args = line.Split(" ");
                string expected = input.ReadLine();

                using StringWriter sw = new StringWriter();
                Console.SetOut(sw);
                Program.Main(args);

                try
                {
                    Assert.Equal(expected, sw.ToString());
                    output.WriteLine($"{++testNumber}. success;");
                }
                catch (Exception e)
                {
                    output.WriteLine($"{++testNumber}. {e};");
                }
            }
        }
    }
}
