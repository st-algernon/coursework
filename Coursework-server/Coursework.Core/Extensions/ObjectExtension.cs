namespace Coursework.Core.Extensions
{
    public static class ObjectExtension
    {
        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
             where T : class
             where TU : class
        {
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            if (source != null && dest != null)
            {
                foreach (var sourceProp in sourceProps)
                {
                    if (destProps.Any(x => x.Name == sourceProp.Name))
                    {
                        var p = destProps.First(x => x.Name == sourceProp.Name);

                        if (p.CanWrite)
                        {

                            object parsed = null;

                            if (p.PropertyType == typeof(DateTime))
                            {
                                if (sourceProp.PropertyType != typeof(DateTime))
                                {
                                    parsed = DateTime.Parse(sourceProp.GetValue(source).ToString());
                                }
                            }

                            if (p.PropertyType.BaseType == typeof(Enum))
                            {
                                parsed = Enum.Parse(p.PropertyType, sourceProp.GetValue(source).ToString());
                            }

                            if (p.PropertyType == typeof(Guid))
                            {
                                parsed = Guid.Parse(sourceProp.GetValue(source).ToString());
                            }

                            p.SetValue(dest, parsed ?? sourceProp.GetValue(source));
                        }
                    }
                }
            }
        }
    }
}
