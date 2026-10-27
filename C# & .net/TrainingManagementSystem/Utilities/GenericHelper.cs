namespace GenericHelperPkg;

public static class GenericHelper
{
    public static void PrintAll<T>(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            Console.WriteLine(item);
        }
    }

    public static T? Find<T>(IEnumerable<T> items, Predicate<T> match)
        where T : class
    {
        foreach (T item in items)
        {
            if (match(item))
            {
                return item;
            }
        }

        return null;
    }
}
