namespace FolkerKinzel.Contacts;

public abstract class BC<T> : ICleanable where T : BC<T>
{
    public abstract bool IsEmpty { get; }

    public abstract void Clean();

    public abstract void Merge(T? source);
}

public class Foo : BC<Foo>
{
    public override bool IsEmpty => throw new NotImplementedException();

    public override void Clean() => throw new NotImplementedException();

    public override void Merge(Foo? source)
    {
        if(source is null)
        {
            return;
        }
    }
}

public class Derived : Foo
{
    public int NewProp { get; set; }

    public override void Merge(Foo? source)
    {
        if(source is Derived der)
        {
            Merge(der);
            return;
        }

        base.Merge(source);
    }

    public void Merge(Derived? source)
    {
        if(source is null)
        {
            return;
        }
        base.Merge(source);
        NewProp = source.NewProp;
    }
}


public class Bar
{
    public Bar()
    {
        var der = new Derived();

        der.Merge(null);
    }
}