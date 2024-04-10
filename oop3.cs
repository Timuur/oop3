using System;
using System.ComponentModel;
using System.Diagnostics;

public abstract class Object
{
    public abstract void show();
    public abstract void clear();
}

public class Point : Object
{
    public int x, y, id;
    Random rnd = new Random();
    public Point()
    {
        x = 0;
        y = 0;
        id = rnd.Next(100);
    }
    public Point(int x, int y)
    {
        id = rnd.Next(100);
        this.x = x;
        this.y = y;
    }
    public Point(Point p)
    {
        id = rnd.Next(100);
        x = p.x;
        y = p.y;
    }
    public void set(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void set(Point p)
    {
        x = p.x;
        y = p.y;
    }
    public override void show()
    {
        Console.WriteLine("[Point::Show()] id = {0}, x = {1}, y = {2}", id, x, y);
    }
    public override void clear()
    {
        x = 0;
        y = 0;
        id = 0;
    }
}
public class Figure : Object
{
    protected Point point = new Point();
    protected int id;
    public Figure() {    }
    public Figure(int x, int y)
    {
        point.set(x, y);
    }

    public Figure(Figure f)
    {
        point.set(f.point);
    }

    public override void show()
    {
        Console.WriteLine("[Figure::Show()] id_figure = {0}, Point - id = {1}, x = {2}, y = {3}",id, point.id, point.x, point.y);
    }
    public override void clear()
    {
        id = 0;
        point.clear();
    }
}

public class Circle : Figure
{
    private int r;
    public Circle()
    {
        this.r = 0;
    }
    public Circle(int r)
    {
        this.r = r;
    }
    public Circle(Circle c) 
    {
        this.r = c.r;
    }
    public void setCenter(int x, int y)
    {
        point.x = x;
        point.y = y;
    }
    public override void show()
    {
        Console.WriteLine("[Circle::show()] r = {0}", r);
        point.show();
    }
    public float Space()
    {
        float ans = 3.14f * r * r;
        Console.WriteLine("[Circle::space()] space = {0}", ans);
        return ans;
    }
}

public class square : Figure
{
    private int a;
    public square()
    {
        a = 0;
    }
    public square(int r)
    {
        this.a = r;
    }
    public square(square c)
    {
        a = c.a;
    }
    public void setCenter(int x, int y)
    {
        point.x = x;
        point.y = y;
    }
    public override void show()
    {
        Console.WriteLine("[square::show()] r = {0}", a);
        point.show();
    }
    public float Space()
    {
        float ans = a * a;
        Console.WriteLine("[square::space()] space = {0}", ans);
        return ans;
    }
}

public class nothing : Object
{
    public nothing() { }
    public override void show() => Console.WriteLine("nothing");
    public override void clear() { }
}


public class Container
{
    private int cnt, index_last_item_cell, count_item = 0;
    private Object[] values;
    private nothing nothing = new nothing();

    public Container(int _cnt)
    {
        this.cnt = _cnt;
        this.index_last_item_cell = 0;
        this.count_item = 0;
        this.values = new Object[cnt];
        for (int i = 0; i < cnt; i++)
            this.values[i] = nothing;
    }
    public int get_count_items()
    {
        count_item = 0;
        for (int i = 0; i < cnt; i++)
            if (values[i] != nothing)
                count_item++;
        return count_item;
    }
    public int get_index_last_item()
    {
        index_last_item_cell = cnt - 1;
        for (int i = cnt - 1; i >= 0; i--)
            if (values[i] != nothing)
                index_last_item_cell--;
        if (index_last_item_cell < 0)
        {
            index_last_item_cell = 0;
        }
        return index_last_item_cell;
    }

    private bool validate_index(int index)
    {
        get_index_last_item();
        if(index < 0)
        {
            Console.WriteLine("меньше нуля");
            return false;
        } else if(index > this.cnt)
        {
            Console.WriteLine("Нет такого значения(слишком большое)");
            return false;
        }
        return true;
    }
    private void x2()
    {
        Object[] newContainer = new Object[cnt * 2];
        for (int i = 0; i < cnt; i++)
            newContainer[i] = values[i];
        for (int i = cnt; i <= cnt*2-1; i++)
            newContainer[i] = nothing;
        cnt *= 2;
        values = newContainer;
        newContainer = null;
    }

        
    public void add_first(Object temp)
    {
        if (values[0].ToString() == nothing.ToString()) values[0] = temp;
        else
        {
            if (index_last_item_cell >= cnt - 1) x2();
            push_right(0);
            values[0] = temp;
        }
    }
    public void insert(int i, Object temp)
    {
        if (validate_index(i) && values[i].ToString() == nothing.ToString()) values[i] = temp;
        else { push_right(i); values[i] = temp; }
    }
    public void add_last(Object temp)
    {
        if (index_last_item_cell >= cnt - 1) x2();
        values[index_last_item_cell+1] = temp;
        index_last_item_cell++;
    }

    public void push_left(int startingFrom)
    {
        for (int i = startingFrom; i < index_last_item_cell - 1; i++)
            values[i] = values[i + 1];
        values[index_last_item_cell] = nothing;
    }
    public void push_right(int startIndex)
    {
        get_index_last_item();
        int lastIndex = startIndex;
        for (int i = startIndex; i < cnt; i++)
        {
            if (values[i].ToString() == nothing.ToString())
            {
                lastIndex = i;
                break;
            }
        }
        if (lastIndex == startIndex)
        {
            index_last_item_cell = cnt - 1;
        }

        if (index_last_item_cell >= cnt - 1)
            x2();
        for (int i = lastIndex-1; i >= startIndex; i--)
        {
            values[i+1] = values[i];
        }
        values[startIndex] = nothing;
    }

    public void show() { get_index_last_item(); get_count_items(); Console.WriteLine("[Storage::void show_properties()] всего обьектов = {0}, размер = {1}", count_item, cnt); }
    public void show_all_objects()
    {
        get_index_last_item();
        Console.WriteLine("[Storage::show()]");
        for (int i = 0; i < cnt; i++)
        {
            Console.Write("[" + i + "] = ");
            values[i].show();
        }
    }
    public Object get(int i)
    {
        if (validate_index(i))
            if (values[i] != nothing)
                return values[i];
        return nothing;
    }
    public Object get(Object temp)
    {
        for (int i = 0; i < cnt - 1; i++)
            if (values[i] != nothing)
                return values[i];
        return nothing;
    }
    public void del_on_idx(int INT)
    {
        if (validate_index(INT))  values[INT] = nothing;
    }
    public void del_on_obj(Object temp)
    {
        for (int i = 0; i < cnt - 1; i++)
            if (values[i] == temp)
            {
                values[i] = nothing;
                break;
            }
    }
    public void del_multy(int i1, int i2)
    {
        if (validate_index(i1) && validate_index(i2))
            for (int i = i1; i < i2; i++) 
                values[i] = nothing;
    }


    public Object get_with_del(int INT)
    {
        if (validate_index(INT))
        {
            Object temp = values[INT];
            values[INT] = nothing;
            return temp;
        }
        nothing figure = new nothing();
        return figure;
    }

    public int count_cells()
    {
        int count = cnt;
        return count;
    }
}

class Program
{
    static void add_rnd(Container storage, int i)
    {
        Random rnd = new Random();
        int r, r1 = 0;

        r = rnd.Next(1, 4);
        r1 = rnd.Next(1, 21);
        switch (r)
        {
            case 1:
                storage.insert(i, new square(r1));
                break;
            case 2:
                storage.insert(i, new Circle(r1));
                break;
            case 3:
                storage.insert(i, new Figure(r1, r1 / 2));
                break;
            default:
                break;
        }
    }

    static Object rnd_obj()
    {
        Random rnd = new Random();
        int r, r1 = 0;
        r = rnd.Next(1, 4);
        r1 = rnd.Next(1, 21);
        switch (r)
        {
            case 1:
                square temp = new square(r1);
                return temp;
            case 2:
                Circle temp1 = new Circle(r1);
                return temp1;
            case 3:
                Figure temp3 = new Figure(r1, r1 / 2);
                return temp3;
            default:
                nothing nothing = new nothing();
                return nothing;
        }
    }

    static void Main()
    {
        Container storage = new Container(10);
        int count = storage.count_cells();
        Random rnd = new Random();

        int r, r1, r2 = 0;
        nothing nothing = new nothing();

        Console.WriteLine(count);

        var sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < count; i++) {
            add_rnd(storage, i);
        }

        Console.WriteLine("_____________________");
        storage.show();
        storage.show_all_objects();
        Console.WriteLine("_____________________");

        for (int i = 0; i < 1000; i++)
        {
            count = storage.count_cells();
            r = rnd.Next(1, 7);
            r1 = rnd.Next(0, count);
            r2 = rnd.Next(0, count);
            switch (r)
            {
                case 1:
                    storage.show();
                    break;
                case 2:
                    Console.WriteLine("Удаление {0} элемента", r1);
                    storage.del_on_idx(r1);
                    break;
                case 3:
                    Console.WriteLine("Добавить случайный обьект в начало");
                    storage.add_first(rnd_obj());
                    break;
                case 4:
                    Console.WriteLine("Добавить случайный обьект в конец");
                    storage.add_last(rnd_obj());
                    break;
                case 5:
                    Console.WriteLine("Удалить элементы с {0} по {1}", r1, r2);
                    storage.del_multy(r1, r1 + r);
                    break;
                case 6:
                    Console.WriteLine("Вставить случайный обект в {0} ячейку", r1);
                    add_rnd(storage, r1);
                    break;
                default:
                    Console.WriteLine("Проблема");
                    break;
            }
        }


        Console.WriteLine("_____________________");
        storage.show_all_objects();
        Console.WriteLine("=+++++++++++++++++++=");
        storage.show();
        Console.WriteLine("_____________________");

        sw.Stop();
        Console.WriteLine(sw.Elapsed);
    }
}
