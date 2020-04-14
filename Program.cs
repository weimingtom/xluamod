/*
 * Created by SharpDevelop.
 * User: 
 * Date: 2020/1/18
 * Time: 9:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using XLua;

//FIXME: should be no namespace, see below CS.Point
[LuaCallCSharp]
[Hotfix]
public class Calc
{
    public int Add(int a, int b)
    {
        return a - b;
    }
}

[LuaCallCSharp]
[GCOptimize]
public struct Point
{
    public Point(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
    public float x;
    public float y;
}


public class AccessByGenGode
{
    public void Print(Point pos)
    {
        Console.WriteLine("by gen code: x=" + pos.x + ",y=" + pos.y);
    }
}

public class AccessByReflection
{
    public void Print(Point pos)
    {
        Console.WriteLine("by reflection: x=" + pos.x + ",y=" + pos.y);
    }
}

namespace xluatest
{
	public class Program
    {
        //FIXME: this delegate should be public, and the class Program shold be public too
        [CSharpCallLua]
        public delegate double LuaMax(double a, double b);

		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
            LuaEnv luaenv = new LuaEnv();
            luaenv.DoString("CS.System.Console.WriteLine('hello world')");

            var max = luaenv.Global.GetInPath<LuaMax>("math.max");
            Console.WriteLine("max:" + max(32, 12));

            luaenv.Global.Set("obj1", new AccessByGenGode());
            luaenv.Global.Set("obj2", new AccessByReflection());

            luaenv.DoString(@"
                local p = CS.Point(3, 4)
                print('-----------------------------')
                obj2:Print(p)
                print('-----------------------------')
                obj1:Print(p)
                print('-----------------------------')
            ");


            var calc = new Calc();
            luaenv.Global.Set("calc", calc);
            luaenv.DoString("print(calc:Add(2, 4))");
			

			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}