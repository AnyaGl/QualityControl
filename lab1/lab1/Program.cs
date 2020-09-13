using System;

namespace triangle
{
    public class Program
    {
        private static string ordinaryTriangleMsg = "ordinary";
        private static string isoscelesTriangleMsg = "isosceles";
        private static string equilateralTriangleMsg = "equilateral";
        private static string notATriangleMsg = "not a triangle";
        private static string unknownErrorMsg = "unknown error";

        private const double EPSILON = 0.000001;
        struct Triangle
        {
            public double a, b, c;
        }

        public static void Main(string[] args)
        {
            Triangle? nullableTriangle = ParseArgs(args);
            if (!nullableTriangle.HasValue)
            {
                Console.Write(unknownErrorMsg);
                return;
            }
            Triangle triangle = nullableTriangle.Value;
            if (IsTriangle(triangle))
            {
                if (IsEquilateralTriangle(triangle))
                {
                    Console.Write(equilateralTriangleMsg);
                }
                else if (IsIsoscelesTriangle(triangle))
                {
                    Console.Write(isoscelesTriangleMsg);
                }
                else
                {
                    Console.Write(ordinaryTriangleMsg);
                }
                return;
            }
            Console.Write(notATriangleMsg);
        }
        private static Triangle? ParseArgs(string[] args)
        {
            if (args.Length != 3)
            {
                return null;
            }

            Triangle triangle;
            if (Double.TryParse(args[0], out triangle.a) &&
                Double.TryParse(args[1], out triangle.b) &&
                Double.TryParse(args[2], out triangle.c))
            {
                return triangle;
            }

            return null;
        }
        private static bool IsEquilateralTriangle(Triangle triangle)
        {
            return  Math.Abs(triangle.a - triangle.b) < EPSILON &&
                    Math.Abs(triangle.b - triangle.c) < EPSILON;
        }
        private static bool IsIsoscelesTriangle(Triangle triangle)
        {
            return  Math.Abs(triangle.a - triangle.b) < EPSILON ||
                    Math.Abs(triangle.b - triangle.c) < EPSILON ||
                    Math.Abs(triangle.a - triangle.c) < EPSILON;
        }
        private static bool IsTriangle(Triangle triangle)
        {
            return  triangle.a + triangle.b > triangle.c &&
                    triangle.b + triangle.c > triangle.a &&
                    triangle.c + triangle.a > triangle.b;
        }
    }
}
