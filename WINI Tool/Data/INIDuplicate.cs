using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WINI_Tool.Data
{
    public abstract class INIDuplicate<T>
    {

    }

    public class INIDuplicateKey : INIDuplicate<INIKey>
    {

    }

    public class INIDuplicateSection : INIDuplicate<INISection>
    {

    }
}
